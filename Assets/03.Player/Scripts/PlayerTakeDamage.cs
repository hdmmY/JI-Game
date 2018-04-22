using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTakeDamage : MonoBehaviour
{
    // The animation Object that used to show player death animation.
    public AnimationToggle.ExplosionAnimHelper m_Explosion;

    // The player's god mode time after it destroied.
    public float m_godModeTime;

    // The player property. 
    public PlayerProperty m_playerProperty;

    private SpriteRenderer _playerSprite;

    private int _defaultPlayerHealth;

    private void OnEnable ()
    {
        _defaultPlayerHealth = m_playerProperty.m_playerHealth;
        _playerSprite = m_playerProperty.m_spriteReference;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        string otherTag = other.tag;

        // Enemy bullet or Enemy
        if (!m_playerProperty.m_god)
        {
            switch (otherTag)
            {
                case "Enemy":
                    PlayerDeath ();
                    break;
                case "EnemyBullet":
                    DamagePlayerByState (other.transform.parent.name, other.transform.parent.GetComponent<JIBulletProperty> ());
                    break;
                case "EnemyLaser":
                    PlayerDeath ();
                    break;
                case "EnemyBullet_DontDestoryOutBound":
                    PlayerDeath ();
                    break;

            }
        }
    }

    // Debug use
    private void Update ()
    {
        if ((Input.GetKey (KeyCode.J) && Input.GetKeyDown (KeyCode.I)) ||
            Input.GetKey (KeyCode.I) && Input.GetKeyDown (KeyCode.J))
        {
            m_playerProperty.m_god = !m_playerProperty.m_god;
        }
    }

    private void PlayerDeath ()
    {
        var explHelper = Instantiate (m_Explosion, transform.position, Quaternion.identity, transform.parent);
        switch (m_playerProperty.m_playerState)
        {
            case JIState.White:
                explHelper.PlayAnimation ("Magic_White");
                break;
            case JIState.Black:
                explHelper.PlayAnimation ("Magic_Black");
                break;
        }
        StartCoroutine (TurnOnGodMode ());

        m_playerProperty.m_playerLife--;
        m_playerProperty.m_playerHealth = _defaultPlayerHealth;

        if (m_playerProperty.m_playerLife < 0)
        {
            m_playerProperty.m_playerLife = 0;
            JITimer.Instance.TimeScale = 0;
            EventManager.Instance.Raise (new GameOverEvent (SceneManager.GetActiveScene ().name));
        }
        m_playerProperty.TakeDamage (m_playerProperty.m_playerLife, m_playerProperty.m_playerHealth);
    }

    private IEnumerator TurnOnGodMode ()
    {
        Color prevColor = _playerSprite.color;

        _playerSprite.color = new Color (prevColor.r,
            prevColor.g,
            prevColor.b,
            0.3f);
        m_playerProperty.m_god = true;

        yield return new WaitForSeconds (m_godModeTime);

        m_playerProperty.m_god = false;
        _playerSprite.color = prevColor;
    }

    private void DamagePlayerByState (string bulletName, JIBulletProperty enemyBullet)
    {
        if (enemyBullet == null) return;

        bulletName = bulletName.ToLower ();
        JIState bulletType;

        if (bulletName.Contains ("black"))
        {
            bulletType = JIState.Black;
        }
        else if (bulletName.Contains ("white"))
        {
            bulletType = JIState.White;
        }
        else
        {
            bulletType = JIState.All;
        }

        BulletPool.Instance.ReleaseGameObject (enemyBullet.gameObject);

        if (bulletType == m_playerProperty.m_playerState)
        {
            m_playerProperty.m_playerHealth--;
            if (m_playerProperty.m_playerHealth <= 0)
            {
                PlayerDeath ();
            }
            else
            {
                m_playerProperty.TakeDamage (m_playerProperty.m_playerLife, m_playerProperty.m_playerHealth);
            }
        }
        else
        {
            PlayerDeath ();
        }
    }
}
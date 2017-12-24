using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        _defaultPlayerHealth = m_playerProperty.m_playerHealth;
        _playerSprite = m_playerProperty.m_spriteReference;
    }

    // Player Death.
    void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.tag;

        // Enemy bullet or Enemy
        if (!m_playerProperty.m_tgm)
        {
            switch (otherTag)
            {
                case "Enemy":
                    PlayerDeath();
                    break;
                case "EnemyBullet":
                    DamagePlayerByState(other.transform.parent.name, other.transform.parent.GetComponent<JIBulletController>());
                    break;
                case "EnemyLaser":
                    PlayerDeath();
                    break;
                case "EnemyBullet_DontDestoryOutBound":
                    PlayerDeath();
                    break;

            }
        }
    }


    void PlayerDeath()
    {
        var explHelper = Instantiate(m_Explosion, transform.position, Quaternion.identity, transform.parent);
        switch (m_playerProperty.m_playerState)
        {
            case JIState.White:
                explHelper.PlayAnimation("Magic_White");
                break;
            case JIState.Black:
                explHelper.PlayAnimation("Magic_Black");
                break;
        }
        StartCoroutine(TurnOnGodMode());

        if(m_playerProperty.m_playerLife == 0)
        {
            JITimer.Instance.TimeScale = 0;
            StartCoroutine(GameOver());    
        }
        else
        {
            m_playerProperty.m_playerLife--;
            ResetPlayerHealth();
        }
        
    }

    IEnumerator TurnOnGodMode()
    {
        Color prevColor = _playerSprite.color;

        _playerSprite.color = new Color(prevColor.r,
                                         prevColor.g,
                                         prevColor.b,
                                         0.3f);
        m_playerProperty.m_tgm = true;

        yield return new WaitForSeconds(m_godModeTime);

        m_playerProperty.m_tgm = false;
        _playerSprite.color = prevColor;
    }

    void ResetPlayerHealth()
    {
        m_playerProperty.m_playerHealth = _defaultPlayerHealth;
    }


    void DamagePlayerByState(string enemyName, JIBulletController enemyBullet)
    {
        if (enemyBullet == null) return;

        enemyName = enemyName.ToLower();
        JIState enemyType;

        if (enemyName.Contains("black"))
        {
            enemyType = JIState.Black;
        }
        else if (enemyName.Contains("white"))
        {
            enemyType = JIState.White;
        }
        else
        {
            enemyType = JIState.All;
        }

        if (enemyType == m_playerProperty.m_playerState)
        {
            m_playerProperty.m_playerHealth--;
            if (m_playerProperty.m_playerHealth <= 0)
            {
                PlayerDeath();
            }
            UbhObjectPool.Instance.ReleaseGameObject(enemyBullet.gameObject);
        }
        else
        {
            PlayerDeath();
        }
    }


    private IEnumerator GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);

        yield return null;
    }

}
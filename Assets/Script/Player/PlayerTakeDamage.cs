using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    // The animation Object that used to show player death animation.
    public GameObject m_Explosion;

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
                    PlayerProperty.PlayerStateType bulletType = other.transform.parent.name.Contains("Black") ?
                                    PlayerProperty.PlayerStateType.Black :
                                    PlayerProperty.PlayerStateType.White;
                    DamagePlayerByState(bulletType, other.transform.parent.GetComponent<UbhBullet>());
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
        Animator explositionAnim = Instantiate(m_Explosion, transform.position, Quaternion.identity, transform.parent).GetComponent<Animator>();
        switch (m_playerProperty.m_playerState)
        {
            case PlayerProperty.PlayerStateType.White:
                explositionAnim.Play("Player_White_Destroy");
                break;
            case PlayerProperty.PlayerStateType.Black:
                explositionAnim.Play("Player_Black_Destory");
                break;
        }             
        StartCoroutine(TurnOnGodMode());
        ResetPlayerHealth();
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


    void DamagePlayerByState(PlayerProperty.PlayerStateType enemyType, UbhBullet enemyBullet)
    {
        if (enemyBullet == null) return;

        Debug.Log(enemyType);

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

}
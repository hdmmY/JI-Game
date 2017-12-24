using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JIUI
{
    public class PlayerLifeMonitor : MonoBehaviour
    {
        public Sprite m_activeSprite;
        public Sprite m_deathSprite;

        public List<SpriteRenderer> m_lifeHUDs;

        private PlayerProperty _player;

        private void Start()
        {
            _player = JIGlobalRef.Player;

            ShowLifeHUD(5);
        }


        private void Update()
        {
            ShowLifeHUD(_player.m_playerLife);
        }


        private void ShowLifeHUD(int curHealth)
        {
            if (m_lifeHUDs != null)
            {
                int i = 0;
                for (; i < curHealth; i++)
                {
                    m_lifeHUDs[i].sprite = m_activeSprite;
                }

                for (; i < m_lifeHUDs.Count; i++)
                {
                    m_lifeHUDs[i].sprite = m_deathSprite;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JIUI
{
    public class PlayerHealthMonitor : MonoBehaviour
    {
        public List<GameObject> m_healthHUDs;

        private PlayerProperty _player;

        private void Start ()
        {
            _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();

            ShowHealthHUD (4);
        }

        private void Update ()
        {
            ShowHealthHUD (_player.m_playerHealth);
        }

        private void ShowHealthHUD (int curHealth)
        {
            if (m_healthHUDs != null)
            {
                int i = 0;
                for (; i < curHealth; i++)
                {
                    m_healthHUDs[i].SetActive (true);
                }

                for (; i < m_healthHUDs.Count; i++)
                {
                    m_healthHUDs[i].SetActive (false);
                }
            }
        }
    }
}
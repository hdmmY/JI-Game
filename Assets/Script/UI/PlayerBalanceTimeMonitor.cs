using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JIUI
{
    public class PlayerBalanceTimeMonitor : MonoBehaviour
    {
        public SpriteFade m_balanceValueFade;

        private PlayerProperty _player;

        private void Start ()
        {
            _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();

            // ShowBalanceHUD(_player.m_playerNeutralization);
        }

        private void Update ()
        {
            //  ShowBalanceHUD(_player.m_playerNeutralization);
        }

        private void ShowBalanceHUD (float curBalanceValue)
        {
            if (m_balanceValueFade != null)
            {
                m_balanceValueFade.m_fadeFactor = curBalanceValue / 100f;
            }
        }
    }
}
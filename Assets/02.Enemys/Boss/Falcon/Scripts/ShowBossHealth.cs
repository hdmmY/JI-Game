using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Boss.Falcon
{
    public class ShowBossHealth : MonoBehaviour
    {
        public TMP_Text HealthText;

        public EnemyProperty BossProperty;

        private void Update ()
        {
            HealthText.text = BossProperty.m_health.ToString ();
        }

    }

}
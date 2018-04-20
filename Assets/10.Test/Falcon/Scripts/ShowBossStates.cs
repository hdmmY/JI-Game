using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Boss.Falcon
{
    public class ShowBossStates : MonoBehaviour
    {
        public TMP_Text StateText;

        public FalconStageOne StageOne;

        public FalconStageTwo StageTwo;

        private void Update ()
        {
            if (StageOne.isActiveAndEnabled)
            {
                StateText.text = StageOne.CurrentState.ToString ();
            }
            else if (StageTwo.isActiveAndEnabled)
            {
                StateText.text = StageTwo.CurrentState.ToString ();
            }
            else
            {
                StateText.text = "NULL";
            }
        }

    }

}
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
            if (StageOne.Active)
            {
                StateText.text = StageOne.CurrentState.ToString ();
            }
            else if (StageTwo.Active)
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
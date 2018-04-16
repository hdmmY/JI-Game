using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        public class PlayerMoveCtrl_2_3 : MonoBehaviour
        {
            public float TimeBeforeChange;

            public Vector2 WindDirection;

            [Range (0, 10)]
            public float PlayerRestoreStrength;

            IPlayerInputCtrl _prevInputCtrl;

            PlayerWindInputCtrl _windInputCtrl;

            private void OnEnable ()
            {
                _prevInputCtrl = InputManager.Instance.InputCtrl;

                _windInputCtrl = new PlayerWindInputCtrl ();

                Invoke ("ChangeInputControl", TimeBeforeChange);
            }

            private void Update ()
            {
                if (_windInputCtrl != null)
                {
                    _windInputCtrl.WindDirection = WindDirection;
                    _windInputCtrl.PlayerRestoreStrength = PlayerRestoreStrength;
                }
            }

            private void OnDisable ()
            {
                InputManager.Instance.InputCtrl = _prevInputCtrl;
                _prevInputCtrl = null;
            }

            private void ChangeInputControl ()
            {
                InputManager.Instance.InputCtrl = _windInputCtrl;
            }
        }
    }
}
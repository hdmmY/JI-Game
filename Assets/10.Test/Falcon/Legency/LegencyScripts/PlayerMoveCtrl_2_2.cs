using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        public class PlayerMoveCtrl_2_2 : MonoBehaviour
        {
            public Vector2 WindDirection;

            [Range (0, 10)]
            public float PlayerRestoreStrength;

            IPlayerInputCtrl _prevInputCtrl;

            PlayerWindInputCtrl _windInputCtrl;

            private void OnEnable ()
            {
                _prevInputCtrl = InputManager.Instance.InputCtrl;

                _windInputCtrl = new PlayerWindInputCtrl ();

                InputManager.Instance.InputCtrl = _windInputCtrl;
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
        }
    }
}
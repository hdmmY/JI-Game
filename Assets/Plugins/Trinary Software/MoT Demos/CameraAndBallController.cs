using UnityEngine;

// /////////////////////////////////////////////////////////////////////////////////////////
//                     CAMERA AND BALL CONTROLLER FOR MOVEMENT DEMO #3
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
//
// This is a simple camera controller that also pushes the invisible ball using physics forces
// that are relative to the camera. 
// /////////////////////////////////////////////////////////////////////////////////////////

namespace MovementDemos
{
    public class CameraAndBallController : MonoBehaviour
    {
        public Rigidbody Ball;
        public float Strength = 5f;
        public Vector2 CameraSpeed = Vector2.one;
        public Vector4 RoomDimensions = Vector4.one;
        public float MinTilt = 20f;
        public float MaxTilt = 60f;
        public float DistanceAboveBall = 3f;
        public float DistanceBehindBall = 3f;

        public int TargetFramerate = 0;

        private Vector3 _pushVector = Vector3.zero;
        private Vector3 _camPos = Vector3.zero;
        private Vector2 _cameraAngle;
        private Transform cameraTransform;
        private Transform ballTransform;

        void Start()
        {
            cameraTransform = Camera.main.transform;
            ballTransform = Ball.transform;
            _cameraAngle = new Vector2(0f, MinTilt);
        }

        void FixedUpdate()
        {
            _pushVector.Set(Input.GetAxis("Horizontal") * Strength, 0f, Input.GetAxis("Vertical") * Strength);

            if(_pushVector.sqrMagnitude > 0.001f)
                Ball.AddForce(cameraTransform.rotation * _pushVector);

            Application.targetFrameRate = TargetFramerate;
        }

        void LateUpdate()
        {
            _cameraAngle.x += Input.GetAxis("Mouse X") * CameraSpeed.x;
            _cameraAngle.y -= Input.GetAxis("Mouse Y") * CameraSpeed.y;

            _cameraAngle.y = Mathf.Clamp(_cameraAngle.y, -MaxTilt, MinTilt);

            if(_cameraAngle.x < -360f)
                _cameraAngle.x += 360f;
            else if (_cameraAngle.x > 360f)
                _cameraAngle.x -= 360f;

            cameraTransform.rotation = Quaternion.Euler(new Vector3(_cameraAngle.y, _cameraAngle.x, 0f));

            _camPos = cameraTransform.rotation * (Vector3.back * DistanceBehindBall);
            _camPos.Set(Mathf.Clamp(ballTransform.position.x + _camPos.x, -RoomDimensions.w, RoomDimensions.y), DistanceAboveBall,
                        Mathf.Clamp(ballTransform.position.z + _camPos.z, -RoomDimensions.z, RoomDimensions.x));
            cameraTransform.position = _camPos;
        }
    }
}

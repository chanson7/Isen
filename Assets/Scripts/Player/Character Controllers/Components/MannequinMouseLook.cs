using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM.Components
{
    /// <summary>
    /// MouseLook.
    /// 
    /// Component used to 'look around' with the mouse.
    /// This rotate the character along its y-axis (yaw) and a child camera along its local x-axis (pitch).
    /// 
    /// This must be attached to the game object with 'CharacterMovement' component.
    /// </summary>

    public class MannequinMouseLook : MouseLook
    {
        #region EDITOR EXPOSED FIELDS

        [Tooltip("The minimum yaw angle while stationary (in degrees).")]
        [SerializeField]
        private float _minYawAngle = -45.0f;

        [Tooltip("The maximum yaw angle while stationary (in degrees).")]
        [SerializeField]
        private float _maxYawAngle = 45.0f;

        #endregion

        #region FIELDS

        protected bool _isMoving = false;
        protected float _horizontalRotation = 0f;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// The minimum yaw angle (in degrees).
        /// </summary>
        public float minYawAngle
        {
            get { return _minYawAngle; }
            set { _minYawAngle = Mathf.Clamp(value, -180.0f, 180.0f); }
        }

        /// <summary>
        /// The maximum yaw angle (in degrees).
        /// </summary>

        public float maxYawAngle
        {
            get { return _maxYawAngle; }
            set { _maxYawAngle = Mathf.Clamp(value, -180.0f, 180.0f); }
        }
        #endregion

        #region INPUT
        void OnMove(InputValue input)
        {
            _isMoving = input.Get<Vector2>().magnitude > 0 ? true : false;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Perform 'Look' rotation.
        /// This rotate the character along its y-axis (yaw) and a child camera along its local x-axis (pitch).
        /// </summary>
        /// <param name="movement">The character movement component.</param>
        /// <param name="cameraTransform">The camera transform.</param>

        public override void LookRotation(CharacterMovement movement, Transform cameraPivotTransform)
        {

            if (_isMoving)
            {
                base.LookRotation(movement, cameraPivotTransform);
             
                _horizontalRotation = 0f;
            }
            else
            {
                _yaw *= lateralSensitivity * Time.deltaTime;
                _pitch *= verticalSensitivity * Time.deltaTime;

                _verticalRotation -= _pitch;
                _verticalRotation = Mathf.Clamp(_verticalRotation, _minPitchAngle, _maxPitchAngle);

                _horizontalRotation += _yaw;
                _horizontalRotation = Mathf.Clamp(_horizontalRotation, _minYawAngle, _maxYawAngle);

                cameraPivotTransform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f);
            }

        }

        #endregion

        #region MONOBEHAVIOUR

        public override void OnValidate()
        {
            base.OnValidate();

            minYawAngle = _minYawAngle;
            maxYawAngle = _maxYawAngle;
        }

        #endregion

    }
}

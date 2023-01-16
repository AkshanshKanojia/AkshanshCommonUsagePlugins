using UnityEngine;
using AkshanshKanojia.Inputs.Mobile;

namespace AkshanshKanojia.Controllers.PointClick
{
    public class PointClickManager : MonoBehaviour
    {
        //this class manage player transforms based on inputs recived. Use this to extend for animators and collisions
        #region SerializeFields
        [SerializeField] float moveSpeed = 1.5f;
        [SerializeField] enum AvailableTrackAaxis { X, Y, Z, XY, XZ,YZ, XYZ }
        [SerializeField] AvailableTrackAaxis curtTrackAxis;
        [SerializeField,Tooltip("Detrmine if can change direction while moving")] bool overrideTargetWhileMoving = true;
        #endregion

        #region PrivateFields
        Vector3 curtTarget,tempRotationTarget;
        bool isTracking = false;
        #endregion

        #region PublicFields
        public bool RotatePlayerWhileMoving;
        [HideInInspector] public float RotSpeed = 2f;
        public enum RotationOptions { X, Y, Z, XY, XZ,YZ, XYZ }
        [HideInInspector] public RotationOptions RotationAxis;
        [HideInInspector] public bool ClampRotation = false;
        [HideInInspector] public Vector3 MinRotationClamp, MaxRotationClamp;

        //events
        public delegate void HasReachedDestination();
        public event HasReachedDestination OnReached;//use this to assign listeners when required. Add a custom class if needed for required inputs on reach
        #endregion

        private void FixedUpdate()
        {
            if(isTracking)
            {
                TrackTarget();
            }
        }
        void TrackTarget()
        {
            Vector3 _tempDir = curtTarget - transform.position;
            switch(curtTrackAxis)
            {
                case AvailableTrackAaxis.X:
                    _tempDir.y = 0;
                    _tempDir.z = 0;
                    break;
                case AvailableTrackAaxis.Y:
                    _tempDir.x = 0;
                    _tempDir.z = 0;
                    break;
                case AvailableTrackAaxis.Z:
                    _tempDir.x = 0;
                    _tempDir.y = 0;
                    break;
                case AvailableTrackAaxis.XY:
                    _tempDir.z = 0;
                    break;
                case AvailableTrackAaxis.XZ:
                    _tempDir.y = 0;
                    break;
                case AvailableTrackAaxis.YZ:
                    _tempDir.x = 0;
                    break;
                default:
                    break;
            }
            transform.position += moveSpeed * Time.deltaTime * _tempDir.normalized;
            if(_tempDir.magnitude<0.2f)
            {
                isTracking = false;
                OnReached?.Invoke();
            }

            //rotation
            RotationManager(_tempDir);
        }

        private void RotationManager(Vector3 _tempDir)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles), Quaternion.LookRotation(_tempDir),
                Time.deltaTime * RotSpeed);
            Vector3 _tempRotAxis = transform.eulerAngles;
            if (ClampRotation)
            {
                _tempRotAxis.x = Mathf.Clamp(_tempRotAxis.x, MinRotationClamp.x, MaxRotationClamp.x);
                _tempRotAxis.y = Mathf.Clamp(_tempRotAxis.y, MinRotationClamp.y, MaxRotationClamp.y);
                _tempRotAxis.z = Mathf.Clamp(_tempRotAxis.z, MinRotationClamp.z, MaxRotationClamp.z);
            }
            switch (RotationAxis)
            {
                case RotationOptions.X:
                    _tempRotAxis.y = 0;
                    _tempRotAxis.z = 0;
                    break;
                case RotationOptions.Y:
                    _tempRotAxis.x = 0;
                    _tempRotAxis.z = 0;
                    break;
                case RotationOptions.Z:
                    _tempRotAxis.x = 0;
                    _tempRotAxis.y = 0;
                    break;
                case RotationOptions.XY:
                    _tempRotAxis.z = 0;
                    break;
                case RotationOptions.XZ:
                    _tempRotAxis.y = 0;
                    break;
                case RotationOptions.YZ:
                    _tempRotAxis.x = 0;
                    break;
                default:
                    break;
            }
            transform.eulerAngles = _tempRotAxis;
        }

        public void SetTarget(Vector3 _dir)
        {
            if (!overrideTargetWhileMoving && isTracking)
                return;
            curtTarget = _dir;
            isTracking = true;
        }
    }
}

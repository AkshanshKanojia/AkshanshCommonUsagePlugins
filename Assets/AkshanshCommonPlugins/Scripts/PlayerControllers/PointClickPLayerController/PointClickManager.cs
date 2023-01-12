using UnityEngine;

namespace AkshanshKanojia.Controllers.PointClick
{
    public class PointClickManager : MonoBehaviour
    {
        //this class manage player transforms based on inputs recived. Use this to extend for animators and collisions
        #region PublicFields
        public bool RotatePlayerWhileMoving;
        [HideInInspector] public float RotSpeed = 2f;
        public enum RotationOptions { X, Y, Z, XY, XZ, XYZ }
        [HideInInspector] public RotationOptions RotationAxis;

        //events
        public delegate void HasReachedDestination();
        public event HasReachedDestination OnReached;//use this to assign listneres when required. Add a custom class if needed for required inputs on reach
        #endregion

        #region SerializeFields
        [SerializeField] float moveSpeed = 1.5f;
        #endregion

        #region PrivateFields
        Vector3 curtTarget;
        bool isTracking = false;
        #endregion

    }
}

using UnityEditor;

namespace AkshanshKanojia.Controllers.PointClick
{
    [CustomEditor(typeof(PointClickManager))]
    public class PointClickEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var _tempMang = (PointClickManager)target;
            if(_tempMang.RotatePlayerWhileMoving)
            {
            }
        }
    }
}
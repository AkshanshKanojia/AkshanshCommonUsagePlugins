using UnityEditor;
using UnityEngine;

namespace AkshanshKanojia.Controllers.PointClick
{
    [CustomEditor(typeof(PointClickManager))]
    public class PointClickEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var _tempMang = (PointClickManager)target;
            #region Rotation Properties
            if (_tempMang.RotatePlayerWhileMoving)
            {
                GUILayout.Label("Rotation Properties");
                _tempMang.RotSpeed = EditorGUILayout.FloatField("Rotation Speed", _tempMang.RotSpeed);
                _tempMang.RotationAxis = (PointClickManager.RotationOptions)EditorGUILayout.EnumPopup("Rotation Axis", _tempMang.RotationAxis);
                _tempMang.ClampRotation = EditorGUILayout.Toggle("Clam Rotation", _tempMang.ClampRotation);
                if(_tempMang.ClampRotation)
                {
                    _tempMang.MinRotationClamp = EditorGUILayout.Vector3Field("MinRotationClamp", _tempMang.MinRotationClamp);
                    _tempMang.MaxRotationClamp = EditorGUILayout.Vector3Field("MaxRotationClamp", _tempMang.MaxRotationClamp);
                }
            }
            #endregion
        }
    }
}
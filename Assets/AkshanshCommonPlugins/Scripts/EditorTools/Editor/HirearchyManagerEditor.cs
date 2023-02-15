using UnityEditor;
using UnityEngine;

namespace AkshanshKanojia.EditorTools
{
    [CustomEditor(typeof(HirearchyManager))]
    public class HirearchyManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var _tempMang = (HirearchyManager)target;
            switch(_tempMang.CurtAction)
            {
                case HirearchyManager.AvailableActions.SortChildrenSpacing:
                    EditorGUILayout.LabelField("Child Sorting Properties");
                    _tempMang.ChildSortAxis = (HirearchyManager.SortingAxis)EditorGUILayout.EnumPopup
                        ("Sorting Axis", _tempMang.ChildSortAxis);
                    _tempMang.ChildSpacing = EditorGUILayout.FloatField("Spacing", _tempMang.ChildSpacing);
                    _tempMang.ChildstartingPos = EditorGUILayout.FloatField("Starting Position", _tempMang.ChildstartingPos);
                    _tempMang.ArrangeChildInLocalSpace = EditorGUILayout.Toggle("Use Local Space",_tempMang.ArrangeChildInLocalSpace);
                    _tempMang.UseNegetiveAxis = EditorGUILayout.Toggle("Use Negetive Axis",_tempMang.UseNegetiveAxis);
                    if(GUILayout.Button("Sort Children"))
                    {
                        _tempMang.SortChildrenSpacing();
                    }
                    if (GUILayout.Button("Reset Children Pos"))
                    {
                        _tempMang.ResetChildPos();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

using UnityEditor;

namespace AkshanshKanojia.LevelEditors
{
    [CustomEditor(typeof(ObjectSpwaner))]
    public class ObjectSpwanerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}

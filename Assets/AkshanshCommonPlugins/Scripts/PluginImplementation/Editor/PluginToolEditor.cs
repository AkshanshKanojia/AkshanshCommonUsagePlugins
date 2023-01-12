using UnityEngine;
using UnityEditor;
using AkshanshKanojia.Inputs.Mobile;
namespace AkshanshKanojia.PluginManager
{
    public class PluginToolEditor : EditorWindow
    {
        [MenuItem("Akshansh/Open Plugin Manager")]
        static void GeneratePluginManager()
        {
            EditorWindow _window = GetWindow<PluginToolEditor>();
            _window.titleContent = new GUIContent("Common Plugins Manager");
        }

        private void OnGUI()
        {
            //EditorGUILayout.LabelField("");
            if (GUILayout.Button("Generate Mobile Input", GUILayout.Width(150),GUILayout.Height(20)))
            {
                GameObject _tempObj = new GameObject("Input manager");
                _tempObj.AddComponent<MobileInputManager>();
                Debug.Log("Generated Mobile Input Manager Dummy Object!");
            }
            if (GUILayout.Button("Close", GUILayout.Width(100), GUILayout.Height(20)))
            {
                GetWindow<PluginToolEditor>().Close();
            }
        }
    }
}

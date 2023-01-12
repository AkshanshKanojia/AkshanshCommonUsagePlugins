using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using AkshanshKanojia.Inputs.Mobile;
namespace AkshanshKanojia.PluginManager
{
    public class PluginToolEditor : EditorWindow
    {
        [SerializeField] List<string> customFolderNames;

        [MenuItem("Akshansh/Open Plugin Manager")]
        static void GeneratePluginManager()
        {
            EditorWindow _window = GetWindow<PluginToolEditor>();
            _window.titleContent = new GUIContent("Common Plugins Manager");
        }

        private void OnGUI()
        {
            //EditorGUILayout.LabelField("");
            if (GUILayout.Button("Generate My Custome Folders", GUILayout.Width(200), GUILayout.Height(20)))
            {
                AssetDatabase.CreateFolder("Assets", "Akshansh");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Scripts");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Models");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Animations");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Prefabs");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Textures");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Sprites");
                AssetDatabase.CreateFolder("Assets/Akshansh", "Materials");
            }
            if (GUILayout.Button("Generate Mobile Input", GUILayout.Width(150), GUILayout.Height(20)))
            {
                GameObject _tempObj = new GameObject("Mobile Input manager");
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

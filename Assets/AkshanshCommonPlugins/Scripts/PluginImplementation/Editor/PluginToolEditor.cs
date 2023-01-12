using UnityEngine;
using UnityEditor;
namespace AkshanshKanoji.PluginManager
{
    public class PluginToolEditor : EditorWindow
    {
        [MenuItem("Akshansh/Generate Plugin Manager")]
        static void GeneratePluginManager()
        {
            EditorWindow _window = GetWindow<PluginToolEditor>();
            _window.titleContent = new GUIContent("Common Plugins Manager");
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.Linq;

namespace HackedDesign.Editor
{
    public class ScriptDependencyAnalyzer : EditorWindow
    {
        private MonoScript targetScript;
        private string targetClassName;

        [MenuItem("Tools/Script Dependency Analyzer")]
        private static void OpenWindow()
        {
            GetWindow<ScriptDependencyAnalyzer>("Script Dependency Analyzer");
        }

        private void OnGUI()
        {
            targetScript = (MonoScript)EditorGUILayout.ObjectField("Target Script", targetScript, typeof(MonoScript), false);

            if (GUILayout.Button("Find Dependencies"))
            {
                if (targetScript == null)
                {
                    Debug.LogWarning("No target script selected.");
                    return;
                }

                targetClassName = targetScript.name;
                FindDependencies();
            }
        }

        private void FindDependencies()
        {
            string[] scriptGuids = AssetDatabase.FindAssets("t:Script");

            foreach (string guid in scriptGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string scriptText = System.IO.File.ReadAllText(path);

                if (scriptText.Contains(targetClassName))
                {
                    Debug.Log($"Script '{path}' references '{targetClassName}'.");
                }
            }
        }
    }
}
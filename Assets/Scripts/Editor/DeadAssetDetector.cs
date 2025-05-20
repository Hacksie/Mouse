using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace HackedDesign.Editor
{
    public class DeadAssetDetector : EditorWindow
    {
        [MenuItem("Tools/Dead Asset Detector")]
        private static void OpenWindow()
        {
            GetWindow<DeadAssetDetector>("Dead Asset Detector");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Find Unused Assets"))
            {
                FindUnusedAssets();
            }
        }

        private void FindUnusedAssets()
        {
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<string> unusedAssets = new List<string>();

            foreach (string path in allAssetPaths)
            {
                if (!path.StartsWith("Assets/")) continue;
                if (AssetDatabase.IsValidFolder(path)) continue;

                string[] dependencies = AssetDatabase.GetDependencies(path, false);
                if (dependencies.Length == 1 && dependencies[0] == path)
                {
                    unusedAssets.Add(path);
                }
            }

            Debug.Log($"Found {unusedAssets.Count} unused assets.");
            foreach (string asset in unusedAssets)
            {
                Debug.Log($"Unused: {asset}");
            }
        }
    }
}
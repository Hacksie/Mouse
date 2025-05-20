using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace HackedDesign.Editor
{
    public class ScriptableObjectGenerator : EditorWindow
    {
        private Type[] scriptableTypes;
        private int selectedIndex = 0;

        [MenuItem("Tools/ScriptableObject Generator")]
        private static void OpenWindow()
        {
            GetWindow<ScriptableObjectGenerator>("ScriptableObject Generator");
        }

        private void OnEnable()
        {
            scriptableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ScriptableObject)) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        private void OnGUI()
        {
            if (scriptableTypes.Length == 0)
            {
                GUILayout.Label("No ScriptableObject types found.");
                return;
            }

            selectedIndex = EditorGUILayout.Popup("ScriptableObject Type", selectedIndex, scriptableTypes.Select(t => t.Name).ToArray());

            if (GUILayout.Button("Create ScriptableObject"))
            {
                CreateAsset(scriptableTypes[selectedIndex]);
            }
        }

        private void CreateAsset(Type type)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            string path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Specify where to save.");
            if (path.Length > 0)
            {
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace HackedDesign.Editor
{
    public class FindScriptReferences : EditorWindow
    {
        private MonoScript scriptToFind;
        private System.Type scriptType;

        [MenuItem("Tools/Find Script References")]
        private static void OpenWindow()
        {
            GetWindow<FindScriptReferences>("Find Script References");
        }

        private void OnGUI()
        {
            scriptToFind = (MonoScript)EditorGUILayout.ObjectField("Script to find", scriptToFind, typeof(MonoScript), false);

            if (GUILayout.Button("Find References"))
            {
                if (scriptToFind == null)
                {
                    Debug.LogWarning("No script selected.");
                    return;
                }

                scriptType = scriptToFind.GetClass();

                if (scriptType == null)
                {
                    Debug.LogError("Selected script is not a valid MonoBehaviour.");
                    return;
                }

                FindInScenes();
                FindInPrefabs();
            }
        }

        private void FindInScenes()
        {
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
            foreach (string guid in sceneGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                var roots = scene.GetRootGameObjects();

                foreach (var root in roots)
                {
                    var components = root.GetComponentsInChildren(scriptType, true);
                    foreach (var comp in components)
                    {
                        Debug.Log($"Found in Scene '{scene.name}': {comp.gameObject.name}", comp.gameObject);
                    }
                }

                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private void FindInPrefabs()
        {
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null)
                    continue;

                var components = prefab.GetComponentsInChildren(scriptType, true);
                foreach (var comp in components)
                {
                    Debug.Log($"Found in Prefab '{path}': {comp.gameObject.name}", comp.gameObject);
                }
            }
        }
    }
}
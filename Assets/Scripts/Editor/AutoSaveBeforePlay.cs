using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace HackedDesign.Editor
{
    [InitializeOnLoad]
    public static class AutoSaveBeforePlay
    {
        static AutoSaveBeforePlay()
        {
            EditorApplication.playModeStateChanged += SaveOnPlay;
        }

        private static void SaveOnPlay(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    Debug.Log("Auto-saved scenes before entering Play mode.");
                }
                else
                {
                    Debug.LogWarning("User canceled scene save. Play mode aborted.");
                    EditorApplication.isPlaying = false;
                }
            }
        }
    }
}
#if UNITY_EDITOR
using UnityEditor;

namespace HackedDesign
{
    [CustomEditor(typeof(EnemyController))]
    public class EnemyControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EnemyController myTarget = (EnemyController)target;
            EditorGUILayout.LabelField("Current Enemy State", myTarget.CurrentState?.ToString());
            EditorGUILayout.LabelField("Current Character State", myTarget.Character.CurrentState?.ToString());
            EditorGUILayout.LabelField("Wall In Front", myTarget.WallInFront.ToString());
            EditorGUILayout.LabelField("Drop In Front", myTarget.DropInFront.ToString());
        }
    }
}

#endif

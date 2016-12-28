using UnityEngine;
using UnityEditor;
using CapturedFlag.Engine.Scriptables;

namespace CapturedFlag.Engine
{
    [CustomEditor(typeof(Entity), true)]
    public class EntityEditor : Editor
    {
        private Entity _instance;
        private PropertyField[] _fields;

        private GUIStyle heading;
        private GUIStyle heading2;

        void OnEnable()
        {
            _instance = (Entity)target;
            _fields = ExposeProperties.GetProperties(_instance);

            heading = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            heading2 = new GUIStyle()
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUIUtility.fieldWidth = 100f;

            EditorGUILayout.BeginVertical();
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Entity Info", heading);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Player ID ", GUILayout.Width(100));
            _instance.myID.playerID = EditorGUILayout.IntField(_instance.myID.playerID);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Faction ID ", GUILayout.Width(100));
            _instance.myID.factionID = EditorGUILayout.IntField(_instance.myID.factionID);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Instance ID ", GUILayout.Width(100));
            EditorGUILayout.LabelField(_instance.GetInstanceID().ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
using UnityEngine;
using UnityEditor;

namespace CapturedFlag.Engine
{
    [CustomEditor(typeof(Factory))]
    public class FactoryEditor : Editor
    {
        private Factory _factory;
        private PropertyField[] _fields;

        void OnEnable()
        {
            _factory = (Factory)target;
            _fields = ExposeProperties.GetProperties(_factory);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUIUtility.fieldWidth = 100f;

            EditorGUILayout.BeginVertical();
            DrawDefaultInspector();

            ExposeProperties.Expose(_fields);
            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
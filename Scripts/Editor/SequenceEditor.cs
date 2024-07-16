namespace UnitySequenceManager.Editor
{
    using System;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnitySequenceManager;

    /// <summary>
    /// Custom editor for ISequence implementations marked with [SequenceAttribute] to provide functionality for testing, stopping, and playing sequences.
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class SequenceEditor : Editor
    {
        private ISequence _sequence;
        private string[] _methodNames;
        private int _selectedMethodIndex;

        public override void OnInspectorGUI()
        {
            MonoBehaviour targetMonoBehaviour = (MonoBehaviour)target;
            _sequence = targetMonoBehaviour as ISequence;

            // Check if the MonoBehaviour has the SequenceAttribute
            var hasSequenceAttribute = targetMonoBehaviour.GetType().GetCustomAttributes(typeof(SequenceAttribute), true).Length > 0;

            if (_sequence == null || !hasSequenceAttribute)
            {
                DrawDefaultInspector();
                return;
            }

            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Sequence Control", EditorStyles.boldLabel);

            if (_sequence.IsRunning)
            {
                EditorGUILayout.LabelField("Status", "Running");
            }
            else
            {
                EditorGUILayout.LabelField("Status", "Stopped");
            }

            if (GUILayout.Button("Start Sequence"))
            {
                _sequence.StartSequence();
            }

            if (GUILayout.Button("Stop Sequence"))
            {
                _sequence.StopSequence();
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Add Actions", EditorStyles.boldLabel);

            _methodNames = targetMonoBehaviour.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
                .Select(m => m.Name)
                .ToArray();

            _selectedMethodIndex = EditorGUILayout.Popup("Action Method", _selectedMethodIndex, _methodNames);

            if (GUILayout.Button("Add Selected Action"))
            {
                AddSelectedAction(targetMonoBehaviour);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddSelectedAction(MonoBehaviour targetMonoBehaviour)
        {
            if (_methodNames == null || _methodNames.Length == 0)
            {
                Debug.LogError("Invalid method selection.");
                return;
            }

            string methodName = _methodNames[_selectedMethodIndex];
            MethodInfo methodInfo = targetMonoBehaviour.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);

            if (methodInfo != null)
            {
                Action action = (Action)Delegate.CreateDelegate(typeof(Action), targetMonoBehaviour, methodInfo);
                _sequence.AddToSequence(action);
                Debug.Log($"Added action: {methodName} from {targetMonoBehaviour.name}");
            }
            else
            {
                Debug.LogError("Failed to create delegate for the selected method.");
            }
        }
    }
}

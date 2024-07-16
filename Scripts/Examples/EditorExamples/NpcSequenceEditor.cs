namespace UnitySequenceManager.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnitySequenceManager.Examples;

    /// <summary>
    /// Custom editor for NpcPatrolSequence2D to add a Run Sequence button.
    /// </summary>
    [CustomEditor(typeof(NpcPatrolSequence2D))]
    public class NpcSequenceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            NpcPatrolSequence2D sequence = (NpcPatrolSequence2D)target;
            if (GUILayout.Button("Run Sequence"))
            {
                sequence.StartSequence();
            }

            if (GUILayout.Button("Stop Sequence"))
            {
                sequence.StopSequence();
            }
        }
    }
}

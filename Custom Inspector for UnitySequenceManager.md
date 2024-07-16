### Custom Inspector Documentation README

```markdown
# Custom Inspector for UnitySequenceManager

This document provides information about the custom inspector tool included in the UnitySequenceManager package. The custom inspector allows for easy management of sequences directly within the Unity Editor.

## Overview

The custom inspector, implemented in the `SequenceEditor` script, enhances the Unity Editor by providing a user-friendly interface for interacting with any component that implements the `ISequence` interface. This includes starting and stopping sequences, viewing sequence status, and dynamically adding actions to sequences.

## Features

- **Start and Stop Sequences**: Easily control the sequence execution directly from the inspector.
- **View Sequence Status**: See whether the sequence is currently running or stopped.
- **Add Actions**: Dynamically add actions to the sequence by selecting methods from the component implementing `ISequence`.

## Installation

1. Add the `SequenceEditor.cs` script to your Unity project under an appropriate folder (e.g., `Editor`).

## Usage

### Step-by-Step Guide

1. **Attach a Component Implementing ISequence**:
    - Attach any component that implements `ISequence` (e.g., `MoveSequence`, `NpcPatrolSequence2D`) to a GameObject in your scene.

2. **Open the Inspector**:
    - Select the GameObject with the attached component to view its inspector.

3. **Sequence Control**:
    - The inspector will display the sequence control section, showing the sequence's running status and buttons to start and stop the sequence.

4. **Add Actions**:
    - In the "Add Actions" section, you can select public methods from the component to add as actions to the sequence.
    - Use the dropdown menu to select a method and click "Add Selected Action" to add it to the sequence.

### Example

Here is an example of using the custom inspector with a `MoveSequence` component:

1. **Attach `MoveSequence` to a GameObject**:
    - Create an empty GameObject in your scene.
    - Attach the `MoveSequence` component to the GameObject.

2. **Open the Inspector**:
    - Select the GameObject to view the inspector.

3. **Sequence Control**:
    - The inspector shows whether the sequence is running or stopped.
    - Use the "Start Sequence" and "Stop Sequence" buttons to control the sequence.

4. **Add Actions**:
    - Select a public method from the `MoveSequence` component using the dropdown menu.
    - Click "Add Selected Action" to add the method as an action to the sequence.

### Code Example

Here is the `SequenceEditor.cs` script used for the custom inspector:

```csharp
namespace UnitySequenceManager.Editor
{
    using System;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnitySequenceManager;

    /// <summary>
    /// Custom editor for ISequence implementations to provide functionality for testing, stopping, and playing sequences.
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class SequenceEditor : Editor
    {
        private ISequence _sequence;
        private MonoBehaviour _actionTarget;
        private string[] _methodNames;
        private int _selectedMethodIndex;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            MonoBehaviour targetMonoBehaviour = (MonoBehaviour)target;
            _sequence = targetMonoBehaviour as ISequence;

            if (_sequence == null)
            {
                EditorGUILayout.HelpBox("This component does not implement ISequence.", MessageType.Warning);
                return;
            }

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
```

## Additional Notes

- Ensure that the component you are using with the custom inspector implements the `ISequence` interface.
- Only public methods with no parameters and a void return type can be added as actions to the sequence.

This custom inspector provides a powerful and flexible way to manage sequences directly within the Unity Editor, enhancing your workflow and making it easier to test and configure sequences.
```

This `README` file provides comprehensive documentation for the custom inspector tool, explaining how to install, use, and integrate it with components that implement the `ISequence` interface. It also includes a step-by-step guide and example to help users understand how to utilize the custom inspector effectively.

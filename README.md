# UnitySequenceManager

A flexible Unity tool for creating and managing action sequences using async/await and Dependency Injection with Zenject.

## Features

- Create and manage sequences of actions.
- Add, remove, and clear actions in a sequence.
- Run sequences with optional delays.
- Utilize Dependency Injection for easy integration and testing.
- Support for conditional, looping, and delayed actions.
- Custom inspector for managing sequences directly in the Unity Editor.
- Custom attribute to specify classes using the custom inspector

## Installation

1. Clone or download the repository.
2. Add the `UnitySequenceManager` folder to your Unity project.
3. Install Zenject via the Unity Package Manager or import the Zenject package.

## Setup

1. Add a GameObject to your scene and attach the `ProjectInstaller` script.
2. Add your custom sequence scripts (e.g., `MoveSequence`) to the appropriate GameObjects.

## Usage

### Example 1: Simple Move Sequence

```csharp
using UnityEngine;
using Zenject;

[Sequence] // Add this attribute
public class SimpleMoveSequence : MonoBehaviour, ISequence
{
    public bool IsRunning { get; private set; }
    private ISequenceManager _sequenceManager;

    public List<Action> ActionSequence { get; private set; }

    [Inject]
    public void Initialize(ISequenceManager sequenceManager)
    {
        _sequenceManager = sequenceManager;
        _sequenceManager.SetSequence(this);
    }

    private void Awake()
    {
        ActionSequence = new List<Action>();
        ClearSequence();
    }

    public void AddToSequence(Action action)
    {
        ActionSequence.Add(action);
        Debug.Log($"Added {nameof(action)} to sequence");
    }

    public void AddDelayedAction(Action action, float delay)
    {
        Action delayedAction = async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            action();
        };
        AddToSequence(delayedAction);
    }

    public void AddLoopingAction(Action action, int count)
    {
        Action loopingAction = () =>
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        };
        AddToSequence(loopingAction);
    }

    public void AddConditionalAction(Action action, Func<bool> condition)
    {
        Action conditionalAction = () =>
        {
            if (condition())
            {
                action();
            }
        };
        AddToSequence(conditionalAction);
    }

    public void ClearSequence()
    {
        ActionSequence.Clear();
    }

    public void RemoveFromSequence(Action action)
    {
        ActionSequence.Remove(action);
    }

    public void StartSequence()
    {
        IsRunning = true;
    }

    public void StopSequence()
    {
        IsRunning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _sequenceManager.RunSequence(.5f);
            Debug.Log("Starting sequence with delay of 0.5 seconds");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AddToSequence(() => Move(Vector3.up));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddToSequence(() => Move(Vector3.left));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddToSequence(() => Move(Vector3.down));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddToSequence(() => Move(Vector3.right));
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;
    }
}

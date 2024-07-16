using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UnitySequenceManager
{

    /// <summary>
    /// Example implementation of ISequence for moving a GameObject.
    /// </summary>
    public class MoveSequence : MonoBehaviour, ISequence
    {
        public bool IsRunning { get; private set; }
        private SequenceManager _sequenceManager;

        public List<Action> ActionSequence { get; private set; }

        [Inject]
        public void Initialize(SequenceManager sequenceManager)
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
}

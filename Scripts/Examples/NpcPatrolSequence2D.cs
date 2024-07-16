using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UnitySequenceManager.Examples
{
    /// <summary>
    /// Example implementation of ISequence for NPC patrolling in 2D.
    /// </summary>
    public class NpcPatrolSequence2D : MonoBehaviour, ISequence
    {
        public bool IsRunning { get; private set; }
        private ISequenceManager _sequenceManager;
        private List<Vector2> patrolPoints = new List<Vector2> {
            new Vector2(0, 5),
            new Vector2(5, 5),
            new Vector2(5, 0),
            new Vector2(0, 0) 
        };
        private int currentPoint = 0;

        public List<Action> ActionSequence { get; private set; }

        [Inject]
        public void Initialize(ISequenceManager sequenceManager)
        {
            _sequenceManager = sequenceManager;
            IsRunning = true;
            PopulateSequence();
        }

        private void PopulateSequence()
        {
            ActionSequence = new List<Action>();
            foreach (var point in patrolPoints)
            {
                ActionSequence.Add(() => MoveToPoint(point));
            }
        }

        private void MoveToPoint(Vector2 point)
        {
            transform.position = Vector2.MoveTowards(transform.position, point, 1f); // Move at a speed factor
            if ((Vector2)transform.position == point)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Count;
                _sequenceManager.RunSequence(2f); // Wait 2 seconds before moving to the next point
            }
        }

        public void ClearSequence()
        {
            ActionSequence.Clear();
        }

        public void AddToSequence(Action action)
        {
            ActionSequence.Add(action);
        }

        public void RemoveFromSequence(Action action)
        {
            ActionSequence.Remove(action);
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

        public void StartSequence()
        {
            IsRunning = true;
        }

        public void StopSequence()
        {
            IsRunning = false;
        }
    }
}

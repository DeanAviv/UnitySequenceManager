using System;
using System.Collections.Generic;

namespace UnitySequenceManager
{
    /// <summary>
    /// Interface for defining a sequence of actions.
    /// </summary>
    public interface ISequence
    {
        bool IsRunning { get; }
        List<Action> ActionSequence { get; }
        void AddToSequence(Action action);
        void RemoveFromSequence(Action action);
        void ClearSequence();
        void StartSequence();
        void StopSequence();
        void AddDelayedAction(Action action, float delay);
        void AddLoopingAction(Action action, int count);
        void AddConditionalAction(Action action, Func<bool> condition);
    }
}

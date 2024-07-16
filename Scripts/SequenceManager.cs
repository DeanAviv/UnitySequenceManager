using System;
using System.Threading.Tasks;

namespace UnitySequenceManager
{
    /// <summary>
    /// Default implementation of a sequence manager.
    /// </summary>
    public class SequenceManager : ISequenceManager
    {
        public Action OnSequenceComplete;

        private ISequence _sequence;

        public void SetSequence(ISequence sequence)
        {
            _sequence = sequence;
        }

        public void RunSequence()
        {
            RunSequenceAsync().ConfigureAwait(false);
        }

        public void RunSequence(float delay)
        {
            RunSequenceAsync(delay).ConfigureAwait(false);
        }

        public void StopSequence()
        {
            _sequence?.StopSequence();
        }

        private async Task RunSequenceAsync()
        {
            await ExecuteSequenceAsync(_sequence);
        }

        private async Task RunSequenceAsync(float delay)
        {
            await SequenceDelayAsync(_sequence, delay);
        }

        private async Task ExecuteSequenceAsync(ISequence sequence)
        {
            sequence.StartSequence();
            foreach (var action in sequence.ActionSequence)
            {
                if (!sequence.IsRunning) break;
                action?.Invoke();
                await Task.Yield();
            }
            sequence.StopSequence();
            OnSequenceComplete?.Invoke();
        }

        private async Task SequenceDelayAsync(ISequence sequence, float delay)
        {
            sequence.StartSequence();
            foreach (var action in sequence.ActionSequence)
            {
                if (!sequence.IsRunning) break;
                await Task.Delay(TimeSpan.FromSeconds(delay));
                action?.Invoke();
            }
            sequence.StopSequence();
            OnSequenceComplete?.Invoke();
        }
    }
}

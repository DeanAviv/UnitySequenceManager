using System.Threading.Tasks;

namespace UnitySequenceManager
{

    /// <summary>
    /// Custom implementation of a sequence manager.
    /// </summary>
    public class CustomSequenceManager : ISequenceManager
    {
        private ISequence _sequence;

        public CustomSequenceManager(ISequence sequence)
        {
            _sequence = sequence;
        }

        public void RunSequence()
        {
            ExecuteSequenceAsync().ConfigureAwait(false);
        }

        public void RunSequence(float delay)
        {
            ExecuteSequenceWithDelayAsync(delay).ConfigureAwait(false);
        }

        public void StopSequence()
        {
            _sequence?.StopSequence();
        }

        private async Task ExecuteSequenceAsync()
        {
            _sequence.StartSequence();
            foreach (var action in _sequence.ActionSequence)
            {
                if (!_sequence.IsRunning) break;
                action.Invoke();
                await Task.Yield();
            }
            _sequence.StopSequence();
        }

        private async Task ExecuteSequenceWithDelayAsync(float delay)
        {
            _sequence.StartSequence();
            foreach (var action in _sequence.ActionSequence)
            {
                if (!_sequence.IsRunning) break;
                await Task.Delay((int)(delay * 1000));
                action.Invoke();
            }
            _sequence.StopSequence();
        }
    }
}

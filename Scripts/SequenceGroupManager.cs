using System.Collections.Generic;

namespace UnitySequenceManager
{
    /// <summary>
    /// Manages a group of sequence managers, allowing for complex, coordinated sequences of actions.
    /// </summary>
    public class SequenceGroupManager
    {
        /// <summary>
        /// List of sequence managers under this group.
        /// </summary>
        private List<ISequenceManager> sequenceManagers = new List<ISequenceManager>();

        /// <summary>
        /// Adds a sequence manager to the group.
        /// </summary>
        /// <param name="manager">The sequence manager to add.</param>
        public void AddSequenceManager(ISequenceManager manager)
        {
            sequenceManagers.Add(manager);
        }

        /// <summary>
        /// Runs all sequences managed by each sequence manager in the group.
        /// </summary>
        public void RunAllSequences()
        {
            foreach (var manager in sequenceManagers)
            {
                manager.RunSequence();
            }
        }

        /// <summary>
        /// Runs all sequences with a specified delay between each sequence.
        /// </summary>
        /// <param name="delay">The delay in seconds before starting each sequence.</param>
        public void RunAllSequences(float delay)
        {
            foreach (var manager in sequenceManagers)
            {
                manager.RunSequence(delay);
            }
        }

        /// <summary>
        /// Stops all sequences currently running in the group.
        /// </summary>
        public void StopAllSequences()
        {
            foreach (var manager in sequenceManagers)
            {
                manager.StopSequence();
            }
        }
    }
}

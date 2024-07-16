namespace UnitySequenceManager
{
    /// <summary>
    /// Interface for managing the execution of action sequences.
    /// </summary>
    public interface ISequenceManager
    {
        void RunSequence();
        void RunSequence(float delay);
        void StopSequence();
    }
}

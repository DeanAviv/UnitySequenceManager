using Zenject;

namespace UnitySequenceManager
{
    /// <summary>
    /// Installs dependencies for the Unity Sequence Manager.
    /// </summary>
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISequence>().To<MoveSequence>().AsSingle();
            Container.Bind<ISequenceManager>().To<SequenceManager>().AsSingle().NonLazy();
        }
    }
}

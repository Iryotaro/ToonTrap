using Ryocatusn.Characters;
using Ryocatusn.Games;
using Ryocatusn.Games.Repository;
using Ryocatusn.Games.Stages;
using Ryocatusn.Games.Stages.Repository;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.AttackableObjects.Repository;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.JankenableObjects.Repository;
using Ryocatusn.Janken.Repository;
using Ryocatusn.Photographers;
using Ryocatusn.StageCreaters;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [CreateAssetMenu(fileName = "Installer", menuName = "Installers/Installer")]
    public class Installer : ScriptableObjectInstaller<Installer>
    {
        public override void InstallBindings()
        {
            Container.Bind<IHandRepository>().To<HandRepository>().AsSingle();
            Container.Bind<IJankenableObjectRepository>().To<JankenableObjectRepository>().AsSingle();
            Container.Bind<IAttackableObjectRepository>().To<AttackableObjectRepository>().AsSingle();
            Container.Bind<IGameRepository>().To<InMemoryGameRepository>().AsSingle();
            Container.Bind<IStageRepository>().To<StageRepository>().AsSingle();

            Container.Bind<HandApplicationService>().AsTransient();
            Container.Bind<JankenableObjectApplicationService>().AsTransient();
            Container.Bind<AttackableObjectApplicationService>().AsTransient();
            Container.Bind<GameApplicationService>().AsTransient();
            Container.Bind<StageApplicationService>().AsTransient();

            Container.Bind<CharacterManager>().AsSingle();
            Container.Bind<BulletFactory>().AsTransient();
            Container.Bind<AreaService>().AsTransient();
            Container.Bind<StageDataCreater>().AsTransient();
            Container.Bind<PhotographerSubjectManager>().AsSingle();
        }
    }
}

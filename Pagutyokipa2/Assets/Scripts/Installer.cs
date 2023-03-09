using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken;
using Ryocatusn.Janken.Repository;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.JankenableObjects.Repository;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.AttackableObjects.Repository;
using Ryocatusn.Games;
using Ryocatusn.Games.Repository;
using Ryocatusn.Games.Stages;
using Ryocatusn.Games.Stages.Repository;

namespace Ryocatusn
{
    public class Installer
    {
        public static Installer installer { get; } = new Installer();

        public ServiceProvider serviceProvider { get; }

        private Installer()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IHandRepository, HandRepository>();
            serviceCollection.AddSingleton<IJankenableObjectRepository, JankenableObjectRepository>();
            serviceCollection.AddSingleton<IAttackableObjectRepository, AttackableObjectRepository>();
            serviceCollection.AddSingleton<IGameRepository, InMemoryGameRepository>();
            serviceCollection.AddSingleton<IStageRepository, StageRepository>();

            serviceCollection.AddTransient<HandApplicationService>();
            serviceCollection.AddTransient<JankenableObjectApplicationService>();
            serviceCollection.AddTransient<AttackableObjectApplicationService>();
            serviceCollection.AddTransient<GameApplicationService>();
            serviceCollection.AddTransient<StageApplicationService>();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}

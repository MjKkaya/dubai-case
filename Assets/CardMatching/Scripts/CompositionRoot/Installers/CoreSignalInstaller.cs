using CardMatching.Core.Events.Signals;
using MessagePipe;
using VContainer;


namespace CardMatching.CompositionRoot.Installers
{
    public static class CoreSignalInstaller
    {
        public static void RegisterCoreSignals(this IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<NewGameStartingSignal>(options);
            builder.RegisterMessageBroker<UnfinishedGameStartingSignal>(options);
            builder.RegisterMessageBroker<GameStartedSignal>(options);
            builder.RegisterMessageBroker<GameOverSignal>(options);
            builder.RegisterMessageBroker<ApplicationPausedSignal>(options);
        }
    }
}
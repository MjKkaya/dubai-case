using CardMatching.Features.LevelProgression.Signals;
using MessagePipe;
using VContainer;


namespace CardMatching.Features.LevelProgression.Installers
{
    public static class LevelProgressionSignalInstaller
    {
        public static void RegisterLevelProgressionSignals(this IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<UnfinishedLevelProgressPanelShowSignal>(options);
        }
    }
}
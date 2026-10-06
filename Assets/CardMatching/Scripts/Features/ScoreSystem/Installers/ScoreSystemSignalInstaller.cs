using CardMatching.Features.ScoreSystem.Signals;
using MessagePipe;
using VContainer;


namespace CardMatching.Features.ScoreSystem.Installers
{
    public static class ScoreSystemSignalInstaller
    {
        public static void RegisterScoreSystemSignals(this IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<EarnedPointSignal>(options);
            builder.RegisterMessageBroker<EarnedComboPointSignal>(options);
        }
    }
}
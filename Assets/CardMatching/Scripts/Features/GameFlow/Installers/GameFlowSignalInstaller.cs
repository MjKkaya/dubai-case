using CardMatching.Features.GameFlow.Signals;
using MessagePipe;
using VContainer;


namespace CardMatching.Features.GameFlow.Installers
{
    public static class GameFlowSignalInstaller
    {
        public static void RegisterGameFlowSignals(this IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<NewGameRequestedSignal>(options);
            builder.RegisterMessageBroker<BeginningPanelShowSignal>(options);
        }
    }
}
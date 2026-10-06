using CardMatching.Features.MatchMechanic.Signals;
using MessagePipe;
using VContainer;


namespace CardMatching.Features.MatchMechanic.Installers
{
    public static class MatchMechanicSignalInstaller
    {
        public static void RegisterMatchMechanicSignals(this IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<MatchingCardSignal>(options);
            builder.RegisterMessageBroker<MismatchingCardSignal>(options);
            builder.RegisterMessageBroker<CardFlippedSignal>(options);
            builder.RegisterMessageBroker<Signals.CardSelectedSignal>(options);
        }
    }
}
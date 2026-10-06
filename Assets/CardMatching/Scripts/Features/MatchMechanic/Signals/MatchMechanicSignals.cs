using CardMatching.Core.Interfaces;


namespace CardMatching.Features.MatchMechanic.Signals
{
    public struct MatchingCardSignal 
    { 
        public IGridBoxCardItem FirstCard; 
        public IGridBoxCardItem SecondCard; 
    }
    public struct MismatchingCardSignal { }

    public struct CardFlippedSignal 
    { 
        public IGridBoxCardItem FlippedCard; 
    }

    public struct CardSelectedSignal { }
}
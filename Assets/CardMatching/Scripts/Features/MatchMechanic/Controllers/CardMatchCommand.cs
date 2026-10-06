using CardMatching.Core.Interfaces;
using CardMatching.Core.Utils;
using CardMatching.Features.MatchMechanic.Signals;
using CardMatching.Features.MatchMechanic.View;
using MessagePipe;
using UnityEngine;


namespace CardMatching.Features.MatchMechanic.Controllers
{
    public class CardMatchCommand : ICommand
    {
        private GridBoxCardItem _firstSelectedItem;
        private GridBoxCardItem _secondSelectedItem;

        private readonly IPublisher<MatchingCardSignal> _matchingCardPub;
        private readonly IPublisher<MismatchingCardSignal> _mismatchingCard;


        public CardMatchCommand(IPublisher<MatchingCardSignal> matchingCardPub, IPublisher<MismatchingCardSignal> mismatchingCard)
        {
            _matchingCardPub = matchingCardPub;
            _mismatchingCard = mismatchingCard;
        }


        public void AddCard(IGridBoxCardItem selectedItemData)
        {
            if (_firstSelectedItem == null)
                _firstSelectedItem = (GridBoxCardItem)selectedItemData;
            else if (_secondSelectedItem == null)
                _secondSelectedItem = (GridBoxCardItem)selectedItemData;
            else
                Debug.LogWarning($"{this}-AddCard: Item is full!!!");
        }

        public void Clear()
        {
            CustomDebug.Log($"{this}-Clear:{_firstSelectedItem.name} / {_secondSelectedItem.name}");
            _firstSelectedItem = null;
            _secondSelectedItem = null;
        }


        public bool IsFull()
        {
            return _firstSelectedItem != null && _secondSelectedItem != null;
        }

        public bool IsMatch()
        {
            return _firstSelectedItem.CardIconIndex == _secondSelectedItem.CardIconIndex;
        }

        public void Execute()
        {
            if (IsMatch())
            {
                _matchingCardPub.Publish(new MatchingCardSignal(){FirstCard = _firstSelectedItem, SecondCard = _secondSelectedItem});
            }
            else
            {
                _firstSelectedItem.StartFlipAniamtion();
                _secondSelectedItem.StartFlipAniamtion();
                _mismatchingCard.Publish(new MismatchingCardSignal());
            }
        }

        public void Undo()
        {
        }
    }
}
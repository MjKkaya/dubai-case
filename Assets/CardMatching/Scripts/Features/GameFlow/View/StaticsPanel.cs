using UnityEngine;
using TMPro;
using VContainer;
using CardMatching.Core.Settings;
using MessagePipe;
using CardMatching.Features.MatchMechanic.Signals;
using CardMatching.Core.Utils;
using System;
using CardMatching.Features.ScoreSystem.Signals;
using CardMatching.Core.Events.Signals;
using VContainer.Unity;


namespace CardMatching.Features.GameFlow.View
{
    public class StaticsPanel : MonoBehaviour, IInitializable
    {
        private float _currentScore;
        private float CurrentScore
        {
            set
            {
                _currentScore = value;
                _scoreText.text = _currentScore.ToString();
            }
        }

        private int _matchesCount;
        private int MatchesCount
        {
            set
            {
                _matchesCount = value;
                _matchesText.text = _matchesCount.ToString();
            }
        }

        private int _turnsCount;
        private int TurnsCount
        {
            set
            {
                _turnsCount = value;
                _turnsText.text = _turnsCount.ToString();
            }
        }

        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _turnsText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private ISubscriber<UnfinishedGameStartingSignal> _unfinishedGameStartingSub;
        private ISubscriber<NewGameStartingSignal> _newGameStartingSub;
        private ISubscriber<MatchingCardSignal> _matchingCardSub;
        private ISubscriber<MismatchingCardSignal> _mismatchingCardSub;
        private ISubscriber<EarnedPointSignal> _earnedPointSub;
        private ISubscriber<EarnedComboPointSignal> _earnedComboPointSub;
        private IDisposable _disposables;
        
        
        [Inject]
        public void Construct(ISubscriber<UnfinishedGameStartingSignal> unfinishedGameStartingSub,
        ISubscriber<NewGameStartingSignal> newGameStartingSub,
        ISubscriber<MatchingCardSignal> matchingCardSub,
        ISubscriber<MismatchingCardSignal> mismatchingCardSub,
        ISubscriber<EarnedPointSignal> earnedPointSub,
        ISubscriber<EarnedComboPointSignal> earnedComboPointSub)
        {
            CustomDebug.Log("StaticsPanel-Initialize");
            _unfinishedGameStartingSub = unfinishedGameStartingSub;
            _newGameStartingSub = newGameStartingSub;
            _matchingCardSub = matchingCardSub;
            _mismatchingCardSub = mismatchingCardSub;
            _earnedPointSub = earnedPointSub;
            _earnedComboPointSub = earnedComboPointSub;
        }

        public void Initialize()
        {
            CustomDebug.Log("StaticsPanel-Initialize");
            var bag = DisposableBag.CreateBuilder();
            _unfinishedGameStartingSub.Subscribe(OnUnfinishedGameStartingSignal).AddTo(bag);
            _newGameStartingSub.Subscribe(OnNewGameStartingSignal).AddTo(bag);
            _matchingCardSub.Subscribe(OnMatchingCardSignal).AddTo(bag);
            _mismatchingCardSub.Subscribe(OnMismatchingCardSignal).AddTo(bag);
            _earnedPointSub.Subscribe(OnEarnedPointSignal).AddTo(bag);
            _earnedComboPointSub.Subscribe(OnEarnedComboPointSignal).AddTo(bag);

            _disposables = bag.Build();
        }


        private void OnDestroy()
        {
            _disposables?.Dispose();
        }


        private void ResetDataAndText()
        {
            CurrentScore = 0;
            MatchesCount = 0;
            TurnsCount = 0;
        }


        private void OnUnfinishedGameStartingSignal(UnfinishedGameStartingSignal signal)
        {
            CurrentGameDataSO currentGameDataSO = signal.GameData;
            CurrentScore = currentGameDataSO.Score;
            MatchesCount = currentGameDataSO.MatchesCount;
            TurnsCount = currentGameDataSO.TurnCount;
        }

        private void OnNewGameStartingSignal(NewGameStartingSignal signal)
        {
            ResetDataAndText();
        }

        private void OnMismatchingCardSignal(MismatchingCardSignal signal)
        {
            TurnsCount = ++_turnsCount;
        }

        private void OnMatchingCardSignal(MatchingCardSignal signal)
        {
            MatchesCount = ++_matchesCount;
            TurnsCount = ++_turnsCount;
        }

        private void OnEarnedPointSignal(EarnedPointSignal signal)
        {
            CurrentScore = _currentScore + signal.Point;
        }

        private void OnEarnedComboPointSignal(EarnedComboPointSignal signal)
        {
            CurrentScore = _currentScore + signal.Point;
        }
    }
}
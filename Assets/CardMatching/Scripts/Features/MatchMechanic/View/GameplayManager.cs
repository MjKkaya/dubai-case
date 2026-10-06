using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using CardMatching.Features.MatchMechanic.Controllers;
using CardMatching.Core.Utils;
using MessagePipe;
using CardMatching.Core.Events.Signals;
using CardMatching.Features.MatchMechanic.Signals;


namespace CardMatching.Features.MatchMechanic.View
{
    /// <summary>
    /// IInitializable ve IDisposable kullanmak şart mı?
    /// Şart değil, eğer istersen eski usul Start ve OnDisable kullanmaya devam edebilirsin.
    /// Ancak Unity'nin OnEnable veya Start metodlarının çalışma sırası (Execution Order) bazen değişkendir.
    /// Eğer bir sınıf Start içinde _gameEvents'i kullanmaya çalışırsa ve
    /// VContainer o an henüz [Inject] işlemini tamamlamadıysa "NullReferenceException" alırsın.
    /// Oysa IInitializable kullandığında VContainer şunu garanti eder:
    /// "Ben bu sınıfa ihtiyacı olan her şeyi %100 enjekte ettim, artık güvenle Initialize() metodunu tetikleyebilirim."
    /// Bu yüzden VContainer ile çalışırken MonoBehavior'larda bile IInitializable kullanmak en güvenli yoldur.
    /// </summary>
    public class GameplayManager : MonoBehaviour, IInitializable, IDisposable
    {
        private CardMatchCommandInvoker _cardMatchCommandInvoker;
        
        private ISubscriber<GameStartedSignal> _gameStartedSub;
        private ISubscriber<CardFlippedSignal> _cardFlipped;

        private IDisposable _disposables;
        

        [Inject]
        public void Construct(CardMatchCommandInvoker invoker, ISubscriber<GameStartedSignal> gameStartedSub, ISubscriber<CardFlippedSignal> cardFlipped)
        {
            _cardMatchCommandInvoker = invoker;
            _gameStartedSub = gameStartedSub;
            _cardFlipped = cardFlipped;
        }
        

        public void Initialize()
        {
            CustomCoroutines.Initialize(this);

            DisposableBagBuilder bag = DisposableBag.CreateBuilder(2);
            
            _gameStartedSub.Subscribe(OnGameStarted).AddTo(bag);
            _cardFlipped.Subscribe(OnCardFlipped).AddTo(bag);

            _disposables = bag.Build();

            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate}, screen:{Screen.currentResolution.refreshRateRatio} ");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate} ");

            //Debug.unityLogger.logEnabled = false;
        }


        public void Dispose()
        {
            _disposables?.Dispose();
        }


        #region Events

        private void OnGameStarted(GameStartedSignal signal)
        {
            _cardMatchCommandInvoker.ResetList();
        }

        private void OnCardFlipped(CardFlippedSignal signal)
        {
            _cardMatchCommandInvoker.AddSelectedCardItem(signal.FlippedCard);
        }

        #endregion
    }
}
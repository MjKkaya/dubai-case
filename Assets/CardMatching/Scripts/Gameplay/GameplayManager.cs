using CardMatching.Core.Interfaces;
using CardMatching.Core.Datas;
using CardMatching.Core.Events;
using CardMatching.Utilities;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace CardMatching.Gameplay
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
        private GameEvents _gameEvents;
        
        
        [Inject]
        public void Construct(CardMatchCommandInvoker invoker, GameEvents gameEvents)
        {
            _cardMatchCommandInvoker = invoker;
            _gameEvents = gameEvents;
        }
        

        public void Initialize()
        {
            CustomCoroutines.Initialize(this);
            
            _gameEvents.GameStarted += GameEvents_GameStarted;
            //_gameEvents.GameOver += GameEvents_GameOver;
            _gameEvents.CardFlipped += GameEvents_CardFlipped;

            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate}, screen:{Screen.currentResolution.refreshRate} ");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate} ");

            //Debug.unityLogger.logEnabled = false;
        }


        public void Dispose()
        {
            _gameEvents.GameStarted -= GameEvents_GameStarted;
            //_gameEvents.GameOver -= GameEvents_GameOver;
            _gameEvents.CardFlipped -= GameEvents_CardFlipped;
        }


        private void GameEvents_GameStarted(GridDimension gridDimension, IGridBoxCardItem[,] gridBoxCardItems)
        {
            _cardMatchCommandInvoker.ResetList();
        }

        //private void GameEvents_GameOver()
        //{
        //    _cardMatchCommandInvoker.ResetList();
        //}



        private void GameEvents_CardFlipped(IGridBoxCardItem gridBoxCardItem)
        {
            _cardMatchCommandInvoker.AddSelectedCardItem(gridBoxCardItem);
        }
    }
}
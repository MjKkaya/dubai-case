using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.Gameplay;
using CardMatching.GridBox;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.Managers
{
    [RequireComponent(typeof(CardMatchCommandInvoker))]
    public class GameplayManager : MonoBehaviour
    {
        private CardMatchCommandInvoker _cardMatchCommandInvoker;


        private void Awake()
        {
            _cardMatchCommandInvoker = GetComponent<CardMatchCommandInvoker>();
            CustomCoroutines.Initialize(this);
        }

        private void OnEnable()
        {
            GameEvents.GameStarted += GameEvents_GameStarted;
            //GameEvents.GameOver += GameEvents_GameOver;
            GameEvents.CardFlipped += GameEvents_CardFlipped;

            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate}, screen:{Screen.currentResolution.refreshRate} ");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            CustomDebug.Log($"vSyncCount: {QualitySettings.vSyncCount}, fps: { Application.targetFrameRate} ");

            //Debug.unityLogger.logEnabled = false;
        }

        private void OnDisable()
        {
            GameEvents.GameStarted -= GameEvents_GameStarted;
            //GameEvents.GameOver -= GameEvents_GameOver;
            GameEvents.CardFlipped -= GameEvents_CardFlipped;
        }


        private void GameEvents_GameStarted(GridDimension gridDimension, GridBoxCardItem[,] gridBoxCardItems)
        {
            _cardMatchCommandInvoker.ResetList();
        }

        //private void GameEvents_GameOver()
        //{
        //    _cardMatchCommandInvoker.ResetList();
        //}



        private void GameEvents_CardFlipped(GridBoxCardItem gridBoxCardItem)
        {
            _cardMatchCommandInvoker.AddSelectedCardItem(gridBoxCardItem);
        }
    }
}
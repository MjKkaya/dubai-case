using CardMatching.Events;
using CardMatching.ScriptableObjects;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.Managers
{
    public class UnfinishedLevelProgressManager: MonoBehaviour
    {
        private const string _unfinishedGameDataKey = "unfinished-game-data";

        [SerializeField] private CurrentGameDataSO currentGameData;


        private void OnEnable()
        {
            GameEvents.NewGameStarting += GameEvents_GameStarting;
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDisable()
        {
            GameEvents.NewGameStarting -= GameEvents_GameStarting;
            GameEvents.GameOver -= GameEvents_GameOver;
        }


        private void Start()
        {
            LoadLastUnfinishedGameData();
            if (currentGameData.TurnCount > 0)
                UIEvents.UnfinishedLevelProgressPanelShow?.Invoke(currentGameData);
            else
                UIEvents.BeginningPanelShow?.Invoke();
        }

        private void OnApplicationQuit()
        {
            CustomDebug.Log($"{this}-OnApplicationQuit-TurnCount:{currentGameData.TurnCount}");
            if (currentGameData.TurnCount > 0)
            {
                currentGameData.PrepareOneDimensionArray();
                SaveLastUnfinishedGameData();
            }
        }


        private void SaveLastUnfinishedGameData()
        {
            CustomDebug.Log($"{this}-SaveLastUnfinishedGameData");
            PlayerPrefs.SetString(_unfinishedGameDataKey, JsonUtility.ToJson(currentGameData));
            PlayerPrefs.Save();
        }

        private void LoadLastUnfinishedGameData()
        {
            string data = PlayerPrefs.GetString(_unfinishedGameDataKey, null);
            if(!string.IsNullOrEmpty(data))
                JsonUtility.FromJsonOverwrite(data, currentGameData);
        }

        private void DeleteUnfinishedGameData()
        {
            PlayerPrefs.DeleteKey(_unfinishedGameDataKey);
        }


        private void GameEvents_GameStarting()
        {
            DeleteUnfinishedGameData();
        }

        private void GameEvents_GameOver()
        {
            DeleteUnfinishedGameData();
        }
    }
}
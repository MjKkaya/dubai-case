using CardMatching.Core.Interfaces;
using UnityEngine;


namespace CardMatching.Core.Infrastructure
{
    public class PlayerPrefsSaveRepository : ISaveRepository
    {
        public void Save(string key, object data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public bool LoadInto(string key, object target)
        {
            if (Exists(key))
            {
                string json = PlayerPrefs.GetString(key);
                JsonUtility.FromJsonOverwrite(json, target);
                return true;
            }
            return false;
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save(); 
        }

        public bool Exists(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}
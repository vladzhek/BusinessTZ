using Data;
using UnityEngine;

namespace Systems
{
    public class SaveLoadService
    {
        private const string SavesKey = "Saves";
        public PlayerProgress PlayerProgress { get; private set; }

        public void SaveProgress()
        {
            PlayerPrefs.SetString(SavesKey, JsonUtility.ToJson(PlayerProgress));
            Debug.Log("[Save] \n" + JsonUtility.ToJson(PlayerProgress));
        }
        
        public void LoadProgress()
        {
            Debug.Log("[Load2] \n" + JsonUtility.ToJson(PlayerProgress));
            InitializeProgress(GetOrCreate());
        }
        
        private PlayerProgress GetOrCreate()
        {
            if(PlayerPrefs.HasKey(SavesKey))
            {
                var saves = PlayerPrefs.GetString(SavesKey);
                return JsonUtility.FromJson<PlayerProgress>(saves);
            }

            var progress = new PlayerProgress();
            return progress;
        }
        
        private void InitializeProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;
        }
    }
}
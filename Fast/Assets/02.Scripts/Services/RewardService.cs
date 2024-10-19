using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    /// <summary>
    /// Service in charge to manage the rewards, add them, complete them and save the files changes.
    /// </summary>
    public class RewardService : MonoBehaviour
    {
        public static RewardService instance;

        /// <summary>
        /// File with all the rewards saved
        /// </summary>
        private static string _rewardsFolderPath;
        
        /// <summary>
        /// All rewards load found in memory
        /// </summary>
        public List<Reward> Rewards { get; private set; }

        /// <summary>
        /// True if it's necessary to update the rewards loaded
        /// </summary>
        private bool _reloadRewards = false;

        /// <summary>
        /// True if the list of tasks is not update
        /// </summary>
        public bool listDirty { get; private set; }

        /// <summary>
        /// Action on touch a Reward in the task list
        /// </summary>
        public UnityAction<Reward> OnTouchReward;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            InitializeService();
        }

        /// <summary> Create folders and load tasks if exist. </summary>
        private void InitializeService()
        {
            _rewardsFolderPath = Path.Combine(Application.persistentDataPath, "rewards");
            if (!Directory.Exists(_rewardsFolderPath))
                Directory.CreateDirectory(_rewardsFolderPath);
            _reloadRewards = true;
            listDirty = true;
            LoadRewards();
        }

        /// <summary> Search and load rewards in the memory</summary>
        private void LoadRewards()
        {
            if (!_reloadRewards) return;

            Rewards = new List<Reward>();
            List<string> rewardFiles = Directory.EnumerateFiles(_rewardsFolderPath).ToList();

            if (rewardFiles.Count == 0) return;

            foreach (string filePath in rewardFiles)
            {
                if (File.Exists(filePath))
                {
                    string jsonText = File.ReadAllText(filePath);
                    RewardData rewardData = JsonUtility.FromJson<RewardData>(jsonText);
                    Rewards.Add(new Reward(rewardData));
                }
            }
            _reloadRewards = false;
            Debug.Log($"Rewards loaded {Rewards.Count}");
        }

        /// <summary> Create a new file to the given reward</summary>
        /// <param name="reward">Reward to be saved</param>
        /// <returns>true success on save, false otherwise</returns>
        public bool SaveTask(Reward reward)
        {
            string filePath = Path.Combine(_rewardsFolderPath, reward.id + ".json");
            reward.UpdateFilePath(filePath); // update before serialize always
            string jsonText = JsonUtility.ToJson(reward.Serialize(), true);

            if (File.Exists(filePath))
            {
                Debug.LogError($"File path already exist {filePath}");
                return false;
            }
            File.WriteAllText(filePath, jsonText);

            if (File.Exists(filePath))
            {
                Debug.Log($"File save with success {filePath}");
                Rewards.Add(reward);
                listDirty = true;
                return true;
            }
            else
            {
                Debug.LogError($"Unexpected error on save {filePath}");
                return false;
            }
        }

        /// <summary>Set list dirt to tell other script to reload their tasks</summary>
        /// <param name="value"></param>
        public void SetListDirty(bool value)
        {
            listDirty = value;
        }

        /// <summary>Update a reward file that already exist with it's new values.</summary>
        /// <param name="reward">Reward to overwrite in memory</param>
        /// <returns>true success</returns>
        public bool UpdateReward(Reward reward)
        {
            if (!reward.hasFilePath)
                reward.UpdateFilePath(Path.Combine(_rewardsFolderPath, reward.id + ".json"));

            if (!File.Exists(reward.filePath))
            {
                Debug.LogError($"Can't update task, file not found path: {reward.filePath}");
                return false;
            }

            string jsonText = JsonUtility.ToJson(reward.Serialize(), true);
            File.WriteAllText(reward.filePath, jsonText);

            if (File.Exists(reward.filePath))
            {
                Debug.Log($"File update with success {reward.id}");
                listDirty = true;
                return true;
            }
            else
            {
                Debug.LogError($"Unexpected error on save task {reward.id}, path: {reward.filePath}");
                return false;
            }
        }
    }
}

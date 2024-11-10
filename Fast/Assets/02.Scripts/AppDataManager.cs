using SaveFiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppDataManager : MonoBehaviour
{
    public static AppDataManager instance { get; private set; }

    /// <summary>
    /// List of current loaded tasks in the persistent memory.
    /// </summary>
    public List<Task> LoadedTasks { get; private set; }
    /// <summary>
    /// List of current loaded rewards in the persistent memory.
    /// </summary>
    public List<Reward> LoadedRewards { get; private set; }
    /// <summary>
    /// Class with all the user global data to save and load.
    /// </summary>
    public UserData userData { get; private set; }
    /// <summary>
    /// A suggeted category select option on create a new task.
    /// </summary>
    public TaskCategory suggestedCategory { get; private set; }

    /// <summary>
    /// Folder name where to find and save tasks files.
    /// </summary>
    private static string TASKS_FOLDER = "tasks";
    /// <summary>
    /// Folder name where to find and save tasks files.
    /// </summary>
    private static string REWARDS_FOLDER = "rewards";
    /// <summary>
    /// File name where to find and save user data file.
    /// </summary>
    private static string USER_FILE= "userData.json";
    /// <summary>
    /// Is set true on tasks persistent data change.
    /// </summary>
    public bool requestReloadTasks;
    /// <summary>
    /// Is set true on rewards persistent data change.
    /// </summary>
    public bool requestReloadRewards;
    /// <summary>
    /// Is set true on user data persistent data change.
    /// </summary>
    public bool requestReloadUserData;

    /// <summary>
    /// Action event on tasks loaded from the persistent memory.
    /// </summary>
    public UnityAction OnTaskLoaded;
    /// <summary>
    /// Action event on rewards loaded from the persistent memory.
    /// </summary>
    public UnityAction OnRewardsLoaded;
    /// <summary>
    /// Action event on user global data loaded from the persistent memory.
    /// </summary>
    public UnityAction OnUserDataLoaded;

    /// <summary>
    /// Action event on task saved in the persistent memory.
    /// </summary>
    public UnityAction OnTaskSaved;
    /// <summary>
    /// Action event on reward saved in the persistent memory.
    /// </summary>
    public UnityAction OnRewardsSaved;
    /// <summary>
    /// Action event on user data saved in the persistent memory.
    /// </summary>
    public UnityAction OnUserDataSaved;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        requestReloadTasks = true;
        requestReloadRewards = true;
        requestReloadUserData = true;
    }

    private void Update()
    {
        if (requestReloadTasks)
        {
            SearchAndLoadTasks();
            requestReloadTasks = false;
        }
        if (requestReloadRewards)
        {
            SearchAndLoadRewards();
            requestReloadRewards = false;
        }
        if (requestReloadUserData)
        { 
            SearchAndLoadUserData();
            requestReloadUserData = false;
        }
    }

    /// <summary>
    /// Search tasks in the persistent memory and load to LoadedTasks list.
    /// </summary>
    private void SearchAndLoadTasks()
    {
        List<string> fileNames = SaveFilesService.GetFilesName(TASKS_FOLDER);
        LoadedTasks = new List<Task>();
        foreach (string name in fileNames)
        {
            TaskSerialize data;
            if (SaveFilesService.TryToLoadDataFile(name, out data, TASKS_FOLDER))
            {
                LoadedTasks.Add(new Task(data));
            }
        }
        Debug.Log($"Tasks found and loaded: {LoadedTasks.Count}");
        OnTaskLoaded?.Invoke();
    }

    /// <summary>
    /// Search rewards in the persistent memory and load to LoadedRewards list.
    /// </summary>
    private void SearchAndLoadRewards()
    {
        List<string> fileNames = SaveFilesService.GetFilesName(REWARDS_FOLDER);
        LoadedRewards = new List<Reward>();
        foreach (string name in fileNames)
        {
            RewardSerialize data;
            if (SaveFilesService.TryToLoadDataFile(name, out data, REWARDS_FOLDER))
            {
                LoadedRewards.Add(new Reward(data));
            }
        }
        Debug.Log($"Rewards found and loaded: {LoadedRewards.Count}");
        OnRewardsLoaded?.Invoke();
    }

    /// <summary>
    /// Search and load user global data if it found in the persistent memory.
    /// </summary>
    private void SearchAndLoadUserData()
    {
        UserDataSerialize data;
        if (SaveFilesService.TryToLoadDataFile(USER_FILE, out data))
        {
            userData = new UserData(data);
            Debug.Log($"User data loaded with success !");
        }
        else
        {
            userData = new UserData();
            SaveUserData();
            Debug.Log($"User data can't be found, creating new data.");
        }
        OnUserDataLoaded?.Invoke();
    }

    /// <summary>
    /// Setup and save locally a new task category to use on create.
    /// </summary>
    /// <param name="category">new suggested category</param>
    public void SetSuggestedTaskCategory(TaskCategory category)
    {
        suggestedCategory = category;
    }

    /// <summary>
    /// Save the current user data in the persistent memory.
    /// </summary>
    /// <returns>true if was saved with success.</returns>
    public bool SaveUserData()
    {
        UserDataSerialize data = userData.Serialize();
        bool result = SaveFilesService.SaveDataFile<UserDataSerialize>(USER_FILE, data);
        if (result) 
            OnUserDataSaved?.Invoke();
        return result;
    }

    /// <summary>
    /// Save the current tasks in the persistent memory.
    /// </summary>
    /// <param name="task">Task to be saved</param>
    /// <returns>true if was saved with success.</returns>
    public bool SaveTask(Task task)
    {
        TaskSerialize data = task.Serialize();
        bool result = SaveFilesService.SaveDataFile<TaskSerialize>(task.generatedFileName, data, TASKS_FOLDER);
        if (result)
            OnTaskSaved?.Invoke();
        return result;
    }

    /// <summary>
    /// Save the current rewards in the persistent memory.
    /// </summary>
    /// <param name="reward">Reward to be saved</param>
    /// <returns>true if was saved with success.</returns>
    public bool SaveReward(Reward reward)
    {
        RewardSerialize data = reward.Serialize();
        bool result = SaveFilesService.SaveDataFile<RewardSerialize>(reward.generatedFileName, data, REWARDS_FOLDER);
        if (result)
            OnRewardsSaved?.Invoke();
        return result;
    }
}

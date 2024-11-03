using SaveFiles;
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
    public UserGlobal userGlobalData { get; private set; }
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
    /// True to request reload all the data.
    /// </summary>
    private bool _requestReloadData = true;

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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (_requestReloadData)
        {
            SearchAndLoadTasks();
            SearchAndLoadRewards();
            SearchAndLoadUserData();
            _requestReloadData = false;
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
            TaskData data;
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
            RewardData data;
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
        UserGlobalData data;
        if (SaveFilesService.TryToLoadDataFile(USER_FILE, out data))
        {
            userGlobalData = new UserGlobal(data);
            Debug.Log($"User data loaded with success !");
        }
        else
        {
            userGlobalData = new UserGlobal();
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
}

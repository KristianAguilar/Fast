using System.IO;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    /// <summary>
    /// File with all the users meta data for tasks 
    /// </summary>
    private string _appDataFile;

    /// <summary>
    /// Current user claim point not used for rewards yet
    /// </summary>
    public int currentPoints { get; private set; }

    /// <summary>
    /// Points already used to claim rewards
    /// </summary>
    public int claimedPoints { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        _appDataFile = Path.Combine(Application.persistentDataPath, "appData.json");
        LoadFile();
    }

    /// <summary>
    /// Register and save an addition in task points.
    /// </summary>
    /// <param name="points"></param>
    public void AddTaskPoints(int points)
    {
        currentPoints += points;
        SaveAppData();
    }

    /// <summary>
    /// Register and save an sustraction in rewards points.
    /// </summary>
    /// <param name="points"></param>
    public void SustrackRewardPoints(int points)
    {
        currentPoints -= points;
        SaveAppData();
    }


    /// <summary>
    /// Create and save the user data in a file
    /// </summary>
    private void SaveAppData()
    {
        AppDataSerialize appData = new AppDataSerialize()
        {
            userPoints = currentPoints,
        };
        string jsonText = JsonUtility.ToJson(appData, true);

        File.WriteAllText(_appDataFile, jsonText); 
        
        if (File.Exists(_appDataFile))
        {
            Debug.Log($"File app data save with success {_appDataFile}");
        }
        else
        {
            Debug.LogError($"Unexpected error on save {_appDataFile}");
        }
    }

    /// <summary>
    /// Search and load the file with the user data
    /// </summary>
    private void LoadFile()
    {
        if (File.Exists(_appDataFile))
        {
            string jsonText = File.ReadAllText(_appDataFile);
            AppDataSerialize appData = JsonUtility.FromJson<AppDataSerialize>(jsonText);
            currentPoints = appData.userPoints;
            claimedPoints = appData.claimedPoints;
        }
        else
        {
            SetupDefaultVariables();
        }
    }

    /// <summary>
    /// Setuo the default values for the user variables
    /// </summary>
    private void SetupDefaultVariables()
    {
        Debug.Log("Variables set with defaults values");
        currentPoints = 0;
        claimedPoints = 0;
    }
}

[System.Serializable]
public class AppDataSerialize
{
    public int userPoints;
    public int claimedPoints;
}
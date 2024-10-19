using System;

public class Task
{
    public string id { get; private set; }
    public string description { get; private set; }
    public TaskCategory category { get; private set; } 
    public TaskState state { get; private set; }
    public int points { get; private set; }
    public string filePath { get; private set; }
    public string limitDate { get; private set; }
    public bool hasFilePath => filePath != null && filePath != string.Empty;

    public Task(string description, TaskCategory category, int points, string limitDate = null)
    {
        id = Guid.NewGuid().ToString();
        state = TaskState.Pending;
        this.description = description;
        this.category = category;
        this.points = points;
        this.filePath = string.Empty;
        this.limitDate = limitDate;
    }

    public Task(TaskData data)
    {
        this.id = data.id;
        this.description = data.description;
        this.category = Enum.Parse<TaskCategory>(data.category);
        this.points = data.points;
        this.state = Enum.Parse<TaskState>(data.state);
        this.filePath = !string.IsNullOrEmpty(data.filePath) ? data.filePath : string.Empty;
        this.limitDate = data.limitDate;
    }

    public TaskData Serialize()
    {
        return new TaskData()
        {
            id = this.id,
            description = this.description,
            category = this.category.ToString(),
            points = this.points,
            state = this.state.ToString(),
            filePath = this.filePath != null? this.filePath : string.Empty,
            limitDate = this.limitDate
        };
    }

    public void UptateState(TaskState newState)
    {
        this.state = newState;
    }

    public void UpdateFilePath(string newPath)
    {
        this.filePath = newPath;
    }
}

[System.Serializable]
public class TaskData
{
    public string id;
    public string category;
    public string state;
    public string description;
    public int points;
    public string filePath;
    public string limitDate;
}

public enum TaskCategory
{
    None = 0,
    Mental = 1,
    Job = 2,
    Body = 3,
    Studies = 4,
    Relationship = 5,
    Project = 6,
    All = 99,
}

public enum TaskState
{
    Pending = 0,
    Ready = 1,
    All = 99,
}

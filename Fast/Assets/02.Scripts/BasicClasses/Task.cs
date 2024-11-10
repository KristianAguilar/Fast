using System;

public class Task
{
    public string id { get; private set; }
    public string description { get; private set; }
    public TaskCategory category { get; private set; } 
    public TaskState state { get; private set; }
    public int points { get; private set; }
    public string limitDate { get; private set; }
    /// <summary>
    /// Suggested file name to save the task in memory.
    /// </summary>
    public string generatedFileName => $"task_{id}.json";

    public Task(string description, TaskCategory category, int points, string limitDate = null)
    {
        id = Guid.NewGuid().ToString();
        state = TaskState.Pending;
        this.description = description;
        this.category = category;
        this.points = points;
        this.limitDate = limitDate;
    }

    public Task(TaskSerialize data)
    {
        this.id = data.id;
        this.description = data.description;
        this.category = Enum.Parse<TaskCategory>(data.category);
        this.points = data.points;
        this.state = Enum.Parse<TaskState>(data.state);
        this.limitDate = data.limitDate;
    }

    public TaskSerialize Serialize()
    {
        return new TaskSerialize()
        {
            id = this.id,
            description = this.description,
            category = this.category.ToString(),
            points = this.points,
            state = this.state.ToString(),
            limitDate = this.limitDate
        };
    }

    public void UptateState(TaskState newState)
    {
        this.state = newState;
    }
}

[System.Serializable]
public class TaskSerialize
{
    public string id;
    public string category;
    public string state;
    public string description;
    public int points;
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

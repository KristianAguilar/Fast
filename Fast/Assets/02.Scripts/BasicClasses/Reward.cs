using System;

public class Reward
{
    public string id { get; private set; }
    public RewardState state { get; private set; }
    public int cost { get; private set; }
    public string description { get; private set; }
    /// <summary>
    /// Suggested file name to save the task in memory.
    /// </summary>
    public string generatedFileName => $"reward_{id}.json";

    public Reward(string description, int cost)
    {
        this.id = Guid.NewGuid().ToString();
        this.state = RewardState.Created;
        this.cost = cost;
        this.description = description;
    }

    public Reward(RewardSerialize data)
    {
        this.id = data.id;
        this.state = Enum.Parse<RewardState>(data.state);
        this.cost = data.cost;
        this.description = data.description;
    }


    public RewardSerialize Serialize()
    {
        return new RewardSerialize()
        {
            id = this.id,
            state = this.state.ToString(),
            cost = this.cost,
            description = this.description,
        };
    }
    public void UptateState(RewardState newState)
    {
        this.state = newState;
    }
}

[System.Serializable]
public class RewardSerialize
{
    public string id;
    public string state;
    public int cost;
    public string description;
}

public enum RewardState
{
    Created = 0,
    Claim = 1,
    All = 99,
}

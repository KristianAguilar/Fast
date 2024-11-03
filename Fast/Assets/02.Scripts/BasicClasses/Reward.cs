using System;

public class Reward
{
    public string id { get; private set; }
    public RewardState state { get; private set; }
    public int cost { get; private set; }
    public string description { get; private set; }

    public Reward(string description, int cost)
    {
        this.id = Guid.NewGuid().ToString();
        this.state = RewardState.Created;
        this.cost = cost;
        this.description = description;
    }

    public Reward(RewardData data)
    {
        this.id = data.id;
        this.state = Enum.Parse<RewardState>(data.state);
        this.cost = data.cost;
        this.description = data.description;
    }


    public RewardData Serialize()
    {
        return new RewardData()
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
public class RewardData
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

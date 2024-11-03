
public class UserGlobal
{
    public int availablePoints { get; private set; }
    public int claimPoints { get; private set; }

    public UserGlobal()
    {
        availablePoints = 0;
        claimPoints = 0;
    }

    public UserGlobal(UserGlobalData data)
    {
        availablePoints = data.availablePoints;
        claimPoints = data.claimPoints;
    }

    public UserGlobalData Serialize()
    {
        return new UserGlobalData()
        {
            availablePoints = this.availablePoints,
            claimPoints = this.claimPoints
        };
    }
}

[System.Serializable]
public class UserGlobalData
{
    public int availablePoints;
    public int claimPoints;
}

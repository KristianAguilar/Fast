
public class UserData
{
    public int currentPoints { get; private set; }
    public int usedPoints { get; private set; }

    public UserData()
    {
        currentPoints = 0;
        usedPoints = 0;
    }

    public UserData(UserDataSerialize data)
    {
        currentPoints = data.currentPoints;
        usedPoints = data.usedPoints;
    }

    public UserDataSerialize Serialize()
    {
        return new UserDataSerialize()
        {
            currentPoints = this.currentPoints,
            usedPoints = this.usedPoints
        };
    }

    /// <summary>
    /// Add points to the current value, check valid value.
    /// </summary>
    /// <param name="addPoints">points to add.</param>
    /// <returns>true points added</returns>
    public bool TryToAddPoints(int addPoints)
    {
        if (addPoints <= 0)
            return false;

        currentPoints += addPoints;
        return true;
    }

    /// <summary>
    /// Try to claim point from current points if it's a valid amount.
    /// </summary>
    /// <param name="pointsToClaim">point to sustract</param>
    /// <returns>true valid claim</returns>
    public bool TryToClaimPoints(int pointsToClaim)
    {
        if (pointsToClaim > currentPoints)
            return false;
       
        currentPoints -= pointsToClaim;
        usedPoints += pointsToClaim;
        return true;
    }
}

[System.Serializable]
public class UserDataSerialize
{
    public int currentPoints;
    public int usedPoints;
}

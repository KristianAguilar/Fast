using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RewardList : MonoBehaviour
{
    /// <summary>
    /// Reward item prefab to instantiate in the list.
    /// </summary>
    [SerializeField] private GameObject _rewardItemPrefab;
    /// <summary>
    /// Transform to place the task item instances.
    /// </summary>
    [SerializeField] private Transform _content;
    /// <summary>
    /// Dropdown to change the reward list state.
    /// </summary>
    [SerializeField] private TMP_Dropdown _stateDropdown;
    /// <summary>
    /// Script to manage a selected task actions and see details
    /// </summary>

    //[SerializeField] private TMP_Text _acumulatedPoints;
    //[SerializeField] private DetailReward _detailsReward;

    /// <summary>
    /// List of current reward items show in the list
    /// </summary>
    private List<GameObject> _currentRewardList = new List<GameObject>();

    /// <summary>
    /// Current reward state to filter in the list.
    /// </summary>
    private RewardState _currentState = RewardState.Created;


    private void Start()
    {
        AppDataManager.instance.OnRewardsLoaded += LoadRewardsItems;
        AppDataManager.instance.OnRewardsSaved += LoadRewardsItems;
        if (AppDataManager.instance.LoadedRewards != null)
            LoadRewardsItems();
    }

    /// <summary>
    /// Reload all the rewards items in the list with new data.
    /// </summary>
    private void LoadRewardsItems()
    {
        CleanRewardList();

        List<Reward> rewards = new List<Reward>();
        rewards = AppDataManager.instance.LoadedRewards;

        if (_currentState != RewardState.All)
            rewards = rewards.Where((t) => t.state == _currentState).ToList();

        foreach (Reward reward in rewards)
        {
            GameObject rewardInstance = Instantiate(_rewardItemPrefab, _content);
            rewardInstance.GetComponent<RewardItemUI>().Setup(reward);
            _currentRewardList.Add(rewardInstance);
        }
    }

    /// <summary>
    /// Clean the current reward list instances
    /// </summary>
    private void CleanRewardList()
    {
        foreach (GameObject rewardInstance in _currentRewardList)
        {
            Destroy(rewardInstance);
        }
        _currentRewardList.Clear();
        _currentRewardList = new List<GameObject>();

        int instanceAmount = _content.childCount;
        for (int i = 0; i < instanceAmount; i++)
        {
            GameObject child = _content.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    /// <summary>
    /// Use the dropdown value to reload the reward with the new state.
    /// </summary>
    public void RequestNewState()
    {
        RewardState newState;
        int value = _stateDropdown.value;

        if (value == 0)
            newState = RewardState.All;
        else if (value == 1)
            newState = RewardState.Created;
        else
            newState = RewardState.Claim;

        if (newState != _currentState)
        {
            _currentState = newState;
            LoadRewardsItems();
        }
    }
}

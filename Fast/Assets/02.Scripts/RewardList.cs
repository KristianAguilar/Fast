using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RewardList : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private TMP_Text _acumulatedPoints;
    [SerializeField] private TMP_Dropdown _stateDropdown;
    /// <summary>
    /// Script to manage a selected task actions and see details
    /// </summary>
    [SerializeField] private DetailReward _detailsReward;

    /// <summary>
    /// List of current tasks instance in screen
    /// </summary>
    private List<GameObject> _rewardsInList = new List<GameObject>();

    private RewardState _currentState = RewardState.All;


    private void Start()
    {
        int instanceAmount = _content.childCount;
        for (int i = 0; i < instanceAmount; i++)
        {
            GameObject child = _content.GetChild(i).gameObject;
            Destroy(child);
        }

        _detailsReward.gameObject.SetActive(false);

        RewardService.instance.OnTouchReward += (Reward reward) =>
        {
            _detailsReward.gameObject.SetActive(true);
            _detailsReward.Setup(reward);
        };
    }

    private void Update()
    {
        if (RewardService.instance.listDirty)
        {
            LoadRewardList();
            RewardService.instance.SetListDirty(false);
            _acumulatedPoints.text = AppManager.instance.currentPoints.ToString();
        }
    }

    /// <summary>Use the current loaded tasks to show in a list</summary>
    public void LoadRewardList()
    {
        CleanRewardList();
        List<Reward> rewards = RewardService.instance.Rewards;

        if (_currentState != RewardState.All)
            rewards = rewards.Where((t) => t.state == _currentState).ToList();

        foreach (Reward reward in rewards)
        {
            GameObject rewardInstance = Instantiate(_itemPrefab, _content);
            rewardInstance.GetComponent<UIRewardItem>().Setup(reward);
            _rewardsInList.Add(rewardInstance);
        }
    }

    public void SetListState(string state)
    {
        _currentState = Enum.Parse<RewardState>(state);
        RewardService.instance.SetListDirty(true);
    }

    /// <summary>Clean the current task list instances</summary>
    private void CleanRewardList()
    {
        foreach (GameObject rewardInstance in _rewardsInList)
        {
            Destroy(rewardInstance);
        }
        _rewardsInList.Clear();
        _rewardsInList = new List<GameObject>();
    }


    public void UpdateRewards()
    {
        int value = _stateDropdown.value;

        RewardState state;

        if (value == 0)
            state = RewardState.All;
        else if (value == 1)
            state = RewardState.Created;
        else
            state = RewardState.Claim;

        _currentState = state;
        LoadRewardList();
    }
}

using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailReward : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private TMP_Text _costTxt;

    private Reward _currentReward;

    public void Setup(Reward reward)
    {
        _currentReward = reward;
        _descriptionTxt.text = reward.description;
        _costTxt.text = reward.cost.ToString();
        _confirmButton.interactable = reward.state == RewardState.Created;
    }

    public void MarkAsClaim()
    {
        if (AppManager.instance.currentPoints >= _currentReward.cost)
        {
            _currentReward.UptateState(RewardState.Claim);
            bool success = RewardService.instance.UpdateReward(_currentReward);
            if (success)
            {
                AppManager.instance.SustrackRewardPoints(_currentReward.cost);
                HidePanel();
            }
        }
        else
        {
            Debug.Log("Can't claim this reward.");
        }
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}

using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private TMP_Text _costTxt;
    [SerializeField] private Image _pointIcon;

    private Reward _currentReward;

    public void Setup(Reward reward)
    {
        _currentReward = reward;
        _descriptionTxt.text = reward.description;
        _costTxt.text = reward.cost.ToString();

        if (reward.state == RewardState.Claim)
        {
            _costTxt.color = Color.gray;
            _pointIcon.gameObject.SetActive(false);
        }
    }

    public void TouchReward()
    {
        RewardService.instance.OnTouchReward?.Invoke(_currentReward);
    }

}

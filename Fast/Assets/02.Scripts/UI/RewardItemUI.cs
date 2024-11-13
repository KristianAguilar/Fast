using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private TMP_Text _costTxt;
    [SerializeField] private Image _pointIcon;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _bgIcon;

    /// <summary>
    /// Panel with all the options
    /// </summary>
    [SerializeField] private GameObject _optionsPanel;
    /// <summary>
    /// Pivot for the button to mark the reward as complete
    /// </summary>
    [SerializeField] private GameObject _completeRewardPivot;
    /// <summary>
    /// Pivot for the button to allow repeat the reward back to pending.
    /// </summary>
    [SerializeField] private GameObject _restartRewardPivot;

    private Reward _currentReward;

    public void Setup(Reward reward)
    {
        _currentReward = reward;
        _descriptionTxt.text = reward.description;
        _costTxt.text = reward.cost.ToString();
        _restartRewardPivot.gameObject.SetActive(false);
        _optionsPanel.gameObject.SetActive(false);

        if (reward.state == RewardState.Claim)
        {
            _costTxt.color = Color.gray;
            _pointIcon.gameObject.SetActive(false);
            _icon.sprite = UIConfigService.Instance.rewardIconOpen;
            _bgIcon.color = Color.gray;
            _restartRewardPivot.gameObject.SetActive(true);
            _completeRewardPivot.gameObject.SetActive(false);
        }
        else
        {
            _icon.sprite = UIConfigService.Instance.rewardIconClose;
        }
    }

    /// <summary>
    /// Mark the reward as complete and reduce the points to the total.
    /// </summary>
    public void ClaimReward()
    {
        UserData userData = AppDataManager.instance.userData;
        if (userData.TryToClaimPoints(_currentReward.cost))
        {
            AppDataManager.instance.SaveUserData();
            _currentReward.UptateState(RewardState.Claim);
            AppDataManager.instance.SaveReward(_currentReward);
        }
        else
        {
            NotificationService.Instance.AriseSimpleNotification(new OneOptionConfig
            {
                message = $"Faltan {_currentReward.cost - AppDataManager.instance.userData.currentPoints} puntos para reclamar esta recompensa",

                buttonText = "Continuar",
                type = NotificationType.Informative
            });
        }
    }

    /// <summary>
    /// Restart the state to the reward back to created
    /// </summary>
    public void RestartReward()
    {
        _currentReward.UptateState(RewardState.Created);
        AppDataManager.instance.SaveReward(_currentReward);
    }
}

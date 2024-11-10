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
            _icon.sprite = UIConfigService.Instance.rewardIconOpen;
            _bgIcon.color = Color.gray;
        }
        else
        {
            _icon.sprite = UIConfigService.Instance.rewardIconClose;
        }
    }

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

}

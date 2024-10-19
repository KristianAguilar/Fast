using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateReward : MonoBehaviour
{
    [SerializeField] private TMP_InputField _descriptionInputField;
    [SerializeField] private TMP_InputField _costInputField;
    [SerializeField] private Image _iconPoints;


    private void OnEnable()
    {
        _descriptionInputField.text = string.Empty;
        _costInputField.text = string.Empty;
        _iconPoints.sprite = UIConfigService.Instance.pointsIcon;
    }

    public void RequestNewReward()
    {
        if (string.IsNullOrEmpty(_descriptionInputField.text) ||
            string.IsNullOrEmpty(_costInputField.text))
        {
            Debug.LogWarning("Try to create a new reward, but some fields are empty.");
            return;
        }
        Reward reward = new Reward(_descriptionInputField.text, int.Parse(_costInputField.text));
        bool result = RewardService.instance.SaveTask(reward);

        if (result)
            this.gameObject.SetActive(false);
    }
}

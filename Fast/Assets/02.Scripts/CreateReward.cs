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
        // Safe checks
        if (string.IsNullOrEmpty(_descriptionInputField.text) ||
            string.IsNullOrEmpty(_costInputField.text))
        {
            Debug.LogWarning("Try to create a new reward, but some fields are empty.");
            return;
        }
        int points = int.Parse(_costInputField.text);
        if (points <= 0)
        {
            Debug.LogWarning($"Not valid points value: {points}");
            return;
        }

        Reward reward = new Reward(_descriptionInputField.text, points);
        // Reward is added to the local memory before the persistent memory, to
        // be available on the OnSaveReward event send.
        AppDataManager.instance.LoadedRewards.Add(reward);
        // This method allow reload the reward list using the local data
        // (avoid unnecessary reload memory files)
        bool result = AppDataManager.instance.SaveReward(reward);

        if (result)
            this.gameObject.SetActive(false);
        else
            AppDataManager.instance.LoadedRewards.Remove(reward);
    }
}

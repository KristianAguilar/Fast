using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ShowPointsUI : MonoBehaviour
{
    private TMP_Text _pointsText;

    private void Start()
    {
        _pointsText = GetComponent<TMP_Text>();
        AppDataManager.instance.OnUserDataLoaded += UpdatePointsUI;
        AppDataManager.instance.OnUserDataSaved += UpdatePointsUI;

        // is null for the taskList screen, but not for rewardsList.
        // in taskList updatePoints is called for the OnUserDataLoaded event
        if (AppDataManager.instance.userData != null)
            UpdatePointsUI();
    }

    /// <summary>
    /// Update the points text UI with the current value in userData field
    /// </summary>
    private void UpdatePointsUI()
    {
        int points = AppDataManager.instance.userData.currentPoints;
        _pointsText.text = points.ToString();
    }
}

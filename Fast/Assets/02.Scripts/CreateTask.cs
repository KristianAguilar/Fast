using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTask : MonoBehaviour
{
    [SerializeField] private TMP_InputField _descriptionInputField;
    [SerializeField] private TMP_Dropdown _categoryDropdown;
    [SerializeField] private TMP_InputField _pointsInputField;
    [SerializeField] private Image _iconPoints;


    private void OnEnable()
    {
        _descriptionInputField.text = string.Empty;
        _categoryDropdown.value = 0;
        _pointsInputField.text = string.Empty;
        _iconPoints.sprite = UIConfigService.Instance.pointsIcon;
        SetSugestedCategory();
    }

    private void SetSugestedCategory()
    {
        TaskCategory category = AppDataManager.instance.suggestedCategory;

        if (category != TaskCategory.All)
            _categoryDropdown.value = (int)category;
    }

    public void RequestNewTask()
    {
        // Safe checks
        if (string.IsNullOrEmpty(_descriptionInputField.text) ||
            string.IsNullOrEmpty(_pointsInputField.text))
        {
            Debug.LogWarning("Try to create a new task, but some fields are empty.");
            return;
        }
        int points = int.Parse(_pointsInputField.text);
        TaskCategory category = (TaskCategory)_categoryDropdown.value;
        if (points <= 0)
        {
            Debug.LogWarning($"Not valid points value: {points}");
            return;
        }

        Task task = new Task(_descriptionInputField.text, category, points);
        // Task is added to the local memory before the persistent memory, to
        // be available on the OnSaveTask event send.
        AppDataManager.instance.LoadedTasks.Add(task);
        // This method allow reload the task list using the local data
        // (avoid unnecessary reload memory files)
        bool result = AppDataManager.instance.SaveTask(task);

        if (result)
            this.gameObject.SetActive(false);
        else
            AppDataManager.instance.LoadedTasks.Remove(task);
    }
}

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
        TaskCategory category = TaskService.Instance.taskCategory;

        if (category != TaskCategory.All)
            _categoryDropdown.value = (int)category;
    }

    public void RequestNewTask()
    {
        if (string.IsNullOrEmpty(_descriptionInputField.text) ||
            string.IsNullOrEmpty(_pointsInputField.text))
        {
            Debug.LogWarning("Try to create a new task, but some fields are empty.");
            return;
        }

        // this force the dropdown match the enum values
        TaskCategory category = (TaskCategory) _categoryDropdown.value;
        Task task = new Task(_descriptionInputField.text, category, int.Parse(_pointsInputField.text));
        bool result = TaskService.Instance.SaveTask(task);

        if (result)
            this.gameObject.SetActive(false);
    }

}

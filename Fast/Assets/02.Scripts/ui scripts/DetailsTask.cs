using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsTask : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private TMP_Text _pointsTxt;

    private Task _currentTask;

    public void Setup(Task task)
    {
        _currentTask = task;
        _descriptionTxt.text = task.description;
        _pointsTxt.text = task.points.ToString();
        _confirmButton.interactable = task.state == TaskState.Pending;

    }

    public void MarkAsComplete()
    {
        _currentTask.UptateState(TaskState.Ready);
        bool success = TaskService.Instance.UpdateTask(_currentTask);
        if (success)
        {
            AppManager.instance.AddTaskPoints(_currentTask.points);
            HidePanel();
        }
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}

using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private Image _categoryIconBg;
    [SerializeField] private Image _categoryIcon;
    [SerializeField] private TMP_Text _pointsTxt;
    [SerializeField] private Image _pointIcon;

    /// <summary>
    /// Panel with all the task options
    /// </summary>
    [SerializeField] private GameObject _optionsPanel;
    /// <summary>
    /// Pivot for the button to mark the task as complete
    /// </summary>
    [SerializeField] private GameObject _completeTaskPivot;
    /// <summary>
    /// Pivot for the button to allow repeat the task back to pending.
    /// </summary>
    [SerializeField] private GameObject _restartTaskPivot;


    private Task _currentTask;

    public void Setup(Task task)
    {
        _currentTask = task;
        _descriptionTxt.text = task.description;
        _pointsTxt.text = task.points.ToString();
        _restartTaskPivot.gameObject.SetActive(false);
        _optionsPanel.gameObject.SetActive(false);

        if (task.state == TaskState.Ready)
        {
            _pointsTxt.color = Color.gray;
            _pointIcon.gameObject.SetActive(false);
            _completeTaskPivot.gameObject.SetActive(false);
            _restartTaskPivot.gameObject.SetActive(true);
        }

        CategoryDesign categorySprite = UIConfigService.Instance.GetCategoryDesign(task.category);
        _categoryIcon.sprite = categorySprite.sprite;
        _categoryIconBg.color = categorySprite.color;
    }

    /// <summary>
    /// Mark the task as complete and add the points to the total.
    /// </summary>
    public void CompleteTask()
    {
        UserData userData = AppDataManager.instance.userData;

        if (userData.TryToAddPoints(_currentTask.points))
        {
            AppDataManager.instance.SaveUserData();

            _currentTask.UptateState(TaskState.Ready);
            AppDataManager.instance.SaveTask(_currentTask);
        }
    }

    /// <summary>
    /// Restart the state to pending to a ready task.
    /// </summary>
    public void RestartTask()
    {
        _currentTask.UptateState(TaskState.Pending);
        AppDataManager.instance.SaveTask(_currentTask);
    }

}

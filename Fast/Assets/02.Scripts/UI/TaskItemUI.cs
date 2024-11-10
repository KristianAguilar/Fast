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

    private Task _currentTask;

    public void Setup(Task task)
    {
        _currentTask = task;
        _descriptionTxt.text = task.description;
        _pointsTxt.text = task.points.ToString();

        if(task.state == TaskState.Ready)
        {
            _pointsTxt.color = Color.gray;
            _pointIcon.gameObject.SetActive(false);
        }

        CategoryDesign categorySprite = UIConfigService.Instance.GetCategoryDesign(task.category);
        _categoryIcon.sprite = categorySprite.sprite;
        _categoryIconBg.color = categorySprite.color;
    }

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

}

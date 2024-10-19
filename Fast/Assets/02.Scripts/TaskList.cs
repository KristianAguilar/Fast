using NotificationService;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private TMP_Text _acumulatedPoints;
    [SerializeField] private TMP_Dropdown _stateDropdown;
    /// <summary>
    /// Script to manage a selected task actions and see details
    /// </summary>
    [SerializeField] private DetailsTask _detailsTask;

    /// <summary>
    /// List of current tasks instance in screen
    /// </summary>
    private List<GameObject> _tasksInList = new List<GameObject>();

    private TaskState _currentState = TaskState.Pending;
    private TaskCategory _currentCategory = TaskCategory.All;

    private void Start()
    {
        int instanceAmount = _content.childCount;
        for (int i = 0; i < instanceAmount; i++)
        {
            GameObject child = _content.GetChild(i).gameObject;
            Destroy(child);
        }

        _detailsTask.gameObject.SetActive(false);

        TaskService.Instance.OnTouchTask += (Task task) =>
        {
            _detailsTask.gameObject.SetActive(true);
            _detailsTask.Setup(task);
        };
    }

    private void Update()
    {
        if (TaskService.Instance.listDirty)
        {
            LoadTasksList();
            TaskService.Instance.SetListDirty(false);
            _acumulatedPoints.text = AppManager.instance.currentPoints.ToString();
        }
    }

    /// <summary>Use the current loaded tasks to show in a list</summary>
    public void LoadTasksList()
    {
        CleanTaskList();
        List<Task> tasks = TaskService.Instance.Tasks;

        if (_currentState != TaskState.All)
            tasks = tasks.Where((t) => t.state == _currentState).ToList();

        if (_currentCategory != TaskCategory.All)
            tasks = tasks.Where((t) => t.category == _currentCategory).ToList();

        foreach (Task task in tasks)
        {
            GameObject taskInstance = Instantiate(_itemPrefab, _content);
            taskInstance.GetComponent<UITaskItem>().Setup(task);
            _tasksInList.Add(taskInstance);
        }
    }

    public void AddTaskOrReward()
    {
        NotificationService.NotificationService.Instance.AriseTwoAnswersNotification(new NotificationConfig
        {
            message = "",
            leftButtonText = "Crear nueva tarea",
            rightButtonText = "Crear nueva recompensa",
            onLeftButtonPress = () => { },
            onRightButtonPress = () => { },
        });
    }

    public void SetListCategory(string category)
    {
        _currentCategory = Enum.Parse<TaskCategory>(category);
        TaskService.Instance.SetListDirty(true);
        TaskService.Instance.SetListCategory(_currentCategory);
    }

    public void SetListState(string state)
    {
        _currentState = Enum.Parse<TaskState>(state);
        TaskService.Instance.SetListDirty(true);
    }


    /// <summary>Clean the current task list instances</summary>
    private void CleanTaskList()
    {
        foreach (GameObject taskInstance in _tasksInList)
        {
            Destroy(taskInstance);
        }
        _tasksInList.Clear();
        _tasksInList = new List<GameObject>();
    }


    public void UpdateTasks()
    {
        int value = _stateDropdown.value;

        TaskState state = TaskState.All;

        if (value == 0)
            state = TaskState.All;
        else if (value == 1)
            state = TaskState.Pending;
        else
            state = TaskState.Ready;

        _currentState = state;
        LoadTasksList();
    }

}
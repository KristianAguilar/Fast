using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    /// <summary>
    /// Task item prefab to instantiate in the list.
    /// </summary>
    [SerializeField] private GameObject _taskItemPrefab;
    /// <summary>
    /// Transform to place the task item instances.
    /// </summary>
    [SerializeField] private Transform _content;
    /// <summary>
    /// Dropdown to change the task list state.
    /// </summary>
    [SerializeField] private TMP_Dropdown _stateDropdown;

    /// <summary>
    /// List of current tasks items show in the list
    /// </summary>
    private List<GameObject> _currrentTaskList = new List<GameObject>();
    /// <summary>
    /// Current tasks state to filter in the list.
    /// </summary>
    private TaskState _currentState = TaskState.Pending;
    /// <summary>
    /// Current tasks category to filter in the list.
    /// </summary>
    private TaskCategory _currentCategory = TaskCategory.All;
    /// <summary>
    /// Task category suggested in the form on create a new task.
    /// </summary>
    public TaskCategory suggestedCategory => _currentCategory;

    private void Start()
    {
        AppDataManager.instance.OnTaskLoaded += LoadTasksItems;
        AppDataManager.instance.OnTaskSaved += LoadTasksItems;
    }

    /// <summary>
    /// Reload all the task items in the list with new data.
    /// </summary>
    public void LoadTasksItems()
    {
        CleanTaskList();
        List<Task> tasks = new List<Task>();
        tasks = AppDataManager.instance.LoadedTasks;

        if (_currentState != TaskState.All)
            tasks = tasks.Where((t) => t.state == _currentState).ToList();

        if (_currentCategory != TaskCategory.All)
            tasks = tasks.Where((t) => t.category == _currentCategory).ToList();

        foreach (Task task in tasks)
        {
            GameObject taskInstance = Instantiate(_taskItemPrefab, _content);
            taskInstance.GetComponent<TaskItemUI>().Setup(task);
            _currrentTaskList.Add(taskInstance);
        }
    }

    /// <summary>
    /// Clean the current task list items
    /// </summary>
    private void CleanTaskList()
    {
        foreach (GameObject taskItem in _currrentTaskList)
        {
            Destroy(taskItem);
        }
        _currrentTaskList.Clear();
        _currrentTaskList = new List<GameObject>();

        int instanceAmount = _content.childCount;
        for (int i = 0; i < instanceAmount; i++)
        {
            GameObject child = _content.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    /// <summary>
    /// Used to request filter the task list with a new category.
    /// </summary>
    /// <param name="category">new category for reload the list.</param>
    public void RequestNewCategory(string category)
    {
        TaskCategory taskCategory = Enum.Parse<TaskCategory>(category);

        if (taskCategory != _currentCategory)
        {
            _currentCategory = taskCategory;
            AppDataManager.instance.SetSuggestedTaskCategory(_currentCategory);
            LoadTasksItems();
        }
    }

    /// <summary>
    /// Use the dropdown value to reload the task with the new state.
    /// </summary>
    public void RequestNewState()
    {
        TaskState newState;
        int value = _stateDropdown.value;
        
        if (value == 0)
            newState = TaskState.All;
        else if (value == 1)
            newState = TaskState.Pending;
        else
            newState = TaskState.Ready;

        if (newState != _currentState)
        {
            _currentState = newState;
            LoadTasksItems();
        }
    }

}
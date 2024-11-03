using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    /// <summary>
    /// Service that allow to load, remove and create tasks
    /// </summary>
    public class TaskService : MonoBehaviour
    {
        public static TaskService Instance { get; private set; }
    
        /// <summary>
        /// All task load found in memory
        /// </summary>
        public List<Task> Tasks { get; private set; }

        /// <summary>
        /// File with all the tasks saved
        /// </summary>
        private string _taskFolder;

        /// <summary>
        /// True if it's necessary to update the tasks loaded
        /// </summary>
        private bool _reloadTasks;

        /// <summary>
        /// True if the list of tasks is not update
        /// </summary>
        public bool listDirty { get; private set; }
        
        /// <summary>
        /// Action on touch a Task in the task list
        /// </summary>
        public UnityAction<Task> OnTouchTask;
        
        /// <summary>
        /// Current category shown in task list 
        /// </summary>
        public TaskCategory taskCategory { get; private set; }


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);
            InitializeService();

        }

        /// <summary> Create folders and load tasks if exist. </summary>
        private void InitializeService()
        {
            _taskFolder = Path.Combine(Application.persistentDataPath, "tasks");
            if (!Directory.Exists(_taskFolder))
                Directory.CreateDirectory(_taskFolder);
            _reloadTasks = true;
            listDirty = true;
            LoadTasks();
            taskCategory = TaskCategory.All;
        }

        /// <summary> Search and load tasks in the memory</summary>
        private void LoadTasks()
        {
            if (!_reloadTasks) return;

            Tasks = new List<Task>();
            List<string> tasksFiles = Directory.EnumerateFiles(_taskFolder).ToList();

            if (tasksFiles.Count == 0) return;

            foreach (string filePath in tasksFiles)
            {
                if (File.Exists(filePath))
                {
                    string jsonText = File.ReadAllText(filePath);
                    TaskData taskData = JsonUtility.FromJson<TaskData>(jsonText);
                    Tasks.Add(new Task(taskData));
                }
            }
            _reloadTasks = false;
            Debug.Log($"Task loaded {Tasks.Count}");
        }

        /// <summary> Create a new file to the given task</summary>
        /// <param name="task">Task to be saved</param>
        /// <returns>true success on save, false otherwise</returns>
        public bool SaveTask(Task task)
        {
            string filePath = Path.Combine(_taskFolder, task.id + ".json");
            string jsonText = JsonUtility.ToJson(task.Serialize(), true);

            if (File.Exists(filePath))
            {
                Debug.LogError($"File path already exist {filePath}");
                return false;
            }
            File.WriteAllText(filePath, jsonText);

            if (File.Exists(filePath))
            {
                Debug.Log($"File save with success {filePath}");
                Tasks.Add(task);
                listDirty = true;
                return true;
            }
            else
            {
                Debug.LogError($"Unexpected error on save {filePath}");
                return false;
            }
        }

        /// <summary>Set list dirt to tell other script to reload their tasks</summary>
        /// <param name="value"></param>
        public void SetListDirty(bool value)
        {
            listDirty = value;
        }

        /// <summary>Update a task file that already exist with it's new values.</summary>
        /// <param name="task">Task to overwrite in memory</param>
        /// <returns>true success</returns>
        public bool UpdateTask(Task task)
        {
            return false;
            //if (!File.Exists(task.filePath))
            //{
            //    Debug.LogError($"Can't update task, file not found path: {task.filePath}");
            //    return false;
            //}

            //string jsonText = JsonUtility.ToJson(task.Serialize(), true);
            //File.WriteAllText(task.filePath, jsonText);

            //if (File.Exists(task.filePath))
            //{
            //    Debug.Log($"File update with success {task.id}");
            //    listDirty = true;
            //    return true;
            //}
            //else
            //{
            //    Debug.LogError($"Unexpected error on save task {task.id}, path: {task.filePath}");
            //    return false;
            //}
        }

        public void SetListCategory(TaskCategory category)
        {
            taskCategory = category;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskListUI : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private Text m_Text;

	[SerializeField]
	private List<TaskItem> m_TaskList = new List<TaskItem>();
	public List<TaskItem> TaskList
	{
		get
		{
			return m_TaskList;
		}

		set
		{
			m_TaskList = value;
		}
	}

	[SerializeField]
	private GameObject m_TaskSpritePrefab;
	public GameObject TaskSpritePrefab
	{
		get
		{
			return m_TaskSpritePrefab;
		}
	
		set
		{
			m_TaskSpritePrefab = value;
		}
	}

	private void Start()
    {
        m_Player.TaskListUpdatedEvent += OnTaskListUpdated;
    }

    private void OnDestroy()
    {
        if (m_Player == null)
            return;

        m_Player.TaskListUpdatedEvent -= OnTaskListUpdated;
    }

    private void OnTaskListUpdated()
    {
        List<Task> tasks = m_Player.Tasks;

        string str = "";
        foreach(Task task in tasks)
        {
            if (task.IsDone) str += "(Done)";

            str += task.TaskDefinition.Title + " - ";
        }

        m_Text.text = str;
    }

	public void UpdateTaskList(List<Task> inputList)
	{
		ClearTaskList();
		PopulateTaskList(inputList);

		Debug.Log("Updated task list");
	}

	void ClearTaskList()
	{
		for(int i = 0; i < m_TaskList.Count; i++)
		{
			Destroy(m_TaskList[i]._uiObject);
		}

		m_TaskList.Clear();
	}

	void PopulateTaskList(List<Task> inputList)
	{
		for(int i = 0; i < inputList.Count; i++)
		{
			GameObject newTaskItem = Instantiate(m_TaskSpritePrefab);

			// Set sprite
			newTaskItem.GetComponent<Image>().sprite = inputList[i].TaskDefinition.Sprite;

			// Set sprite as done
			if(inputList[i].IsDone)
			{

			}

			// Set transform
			newTaskItem.transform.SetParent(transform);

			// Add to tasklist
			m_TaskList.Add(new TaskItem(inputList[i], newTaskItem));
			
		}
	}
}

[System.Serializable]
public class TaskItem
{
	[Header("Properties")]
	public Task _task;
	public GameObject _uiObject;

	public TaskItem(Task task, GameObject uiObject)
	{
		_task = task;
		_uiObject = uiObject;
	}
}

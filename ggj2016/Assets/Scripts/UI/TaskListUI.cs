using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskListUI : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

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

	[SerializeField]
	private Transform m_Parent;
	public Transform Parent
	{
		get { return m_Parent; }
	}

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
		UpdateTaskList(m_Player.Tasks);
    }

	public void UpdateTaskList(List<Task> inputList)
	{
		ClearTaskList();
		PopulateTaskList(inputList);
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
			newTaskItem.GetComponent<TaskSprite>()._taskSprite.sprite = inputList[i].TaskDefinition.Sprite;
			if (inputList[i].IsDone)
				newTaskItem.GetComponent<TaskSprite>()._taskCheck.enabled = true;

			// Set transform
			newTaskItem.transform.SetParent(m_Parent);
            newTaskItem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

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

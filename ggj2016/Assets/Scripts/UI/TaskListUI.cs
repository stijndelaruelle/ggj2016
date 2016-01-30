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

	public void PopulateTaskList()
	{

	}
}

public class TaskItem
{
	[Header("Properties")]
	public bool _done;
	public TaskDefinition _taskDefinition;
	public GameManager _uiObject;
}

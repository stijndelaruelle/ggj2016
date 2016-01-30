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
}

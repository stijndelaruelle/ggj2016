using UnityEngine;
using System.Collections;

public class TaskObject : MonoBehaviour, InteractableObject
{
    [SerializeField]
    private TaskDefinition m_TaskDefinition;

    private Player m_CurrentPlayer;
    private Coroutine m_TaskRoutineHandle;

    private void Start()
    {
        GameManager.Instance.StartDayEvent += OnStartDay;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnStartDay;
    }

    public bool CanInteract(Player player)
    {
        return true;
    }

    public void Interact(Player player)
    {
        if (m_CurrentPlayer == null)
        {
            //Start the interaction
            m_CurrentPlayer = player;
            m_TaskRoutineHandle = StartCoroutine(TaskRoutine(player));
            return;
        }

        //Cancel the interaction
        m_CurrentPlayer = null;
        StopCoroutine(m_TaskRoutineHandle);
        m_TaskRoutineHandle = null;
    }

    private IEnumerator TaskRoutine(Player player)
    {
        float timer = m_TaskDefinition.TimeToComplete;

        while (timer > 0.0f)
        {
            //UPDATE VISUALS

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        player.UpdateTask(m_TaskDefinition);
    }

    private void OnStartDay()
    {
        m_CurrentPlayer = null;

        if (m_TaskRoutineHandle != null)
            StopCoroutine(m_TaskRoutineHandle);

        m_TaskRoutineHandle = null;
    }
}

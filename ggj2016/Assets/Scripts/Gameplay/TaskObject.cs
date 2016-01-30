using UnityEngine;
using System.Collections;

public class TaskObject : MonoBehaviour, InteractableObject
{
    [SerializeField]
    private TaskDefinition m_TaskDefinition;

    private Player m_CurrentPlayer;
    private Coroutine m_TaskRoutineHandle;

    public bool CanInteract(Player player)
    {
        return (m_CurrentPlayer == null || m_CurrentPlayer == player);
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

        if (m_CurrentPlayer == player)
        {
			// Hide icon
			m_CurrentPlayer._icon.Fail();

            //Cancel the interaction
            m_CurrentPlayer = null;
            StopCoroutine(m_TaskRoutineHandle);
            return;
        }

        Debug.Log("Somebody else is already interacting");
    }

    private IEnumerator TaskRoutine(Player player)
    {
        float timer = m_TaskDefinition.TimeToComplete;

        while (timer > 0.0f)
        {
			// UPDATE VISUALS
			float progress = (m_TaskDefinition.TimeToComplete - Mathf.FloorToInt(timer)) / m_TaskDefinition.TimeToComplete;
			//_icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

		// Hide icon
		//_icon.Win();

        player.UpdateTask(m_TaskDefinition);
    }

	// Icon visuals
	public Sprite ReturnSprite()
	{
		return m_TaskDefinition.Sprite;
	}
}

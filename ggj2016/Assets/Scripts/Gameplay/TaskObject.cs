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
        return true;
    }

    public void Interact(Player player)
    {
        if (m_CurrentPlayer == null)
        {
			//Start the interaction
			m_CurrentPlayer = player;
            m_TaskRoutineHandle = StartCoroutine(TaskRoutine(player));

			// Display the icon
			m_CurrentPlayer._icon.ShowSprite(m_TaskDefinition.Sprite);

			return;
        }

		// Hide icon
		m_CurrentPlayer._icon.Fail();

        //Cancel the interaction
        m_CurrentPlayer = null;
        StopCoroutine(m_TaskRoutineHandle);
    }

    private IEnumerator TaskRoutine(Player player)
    {
        float timer = m_TaskDefinition.TimeToComplete;

        while (timer > 0.0f)
        {
			// UPDATE VISUALS
			float progress = (m_TaskDefinition.TimeToComplete - timer) / m_TaskDefinition.TimeToComplete;
			m_CurrentPlayer._icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

		// Hide icon
		m_CurrentPlayer._icon.Win();

        player.UpdateTask(m_TaskDefinition);
    }
}

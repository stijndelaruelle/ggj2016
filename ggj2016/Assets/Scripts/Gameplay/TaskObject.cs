using UnityEngine;
using System.Collections;

public class TaskObject : MonoBehaviour, InteractableObject
{
    [SerializeField]
    private TaskDefinition m_TaskDefinition;

    private Player m_CurrentPlayer;
    private Coroutine m_TaskRoutineHandle;

	private Icon _icon;

    public bool CanInteract(Player player)
    {
        return true;
    }

    public void Interact(Player player)
    {
        if (m_CurrentPlayer == null)
        {
			// Display the icon
			InitializeIcon();

			//Start the interaction
			m_CurrentPlayer = player;
            m_TaskRoutineHandle = StartCoroutine(TaskRoutine(player));
            return;
        }

		// Hide icon
		_icon.Fail();

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
			float progress = (m_TaskDefinition.TimeToComplete - Mathf.FloorToInt(timer)) / m_TaskDefinition.TimeToComplete;
			_icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

		// Hide icon
		_icon.Win();

        player.UpdateTask(m_TaskDefinition);
    }

	// Icon visuals
	private void InitializeIcon()
	{
		if (_icon == null)
			_icon = GetComponentInChildren<Icon>();

		_icon.Initialize(m_TaskDefinition.Sprite);
	}
}

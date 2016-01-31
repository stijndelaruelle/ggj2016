using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskObject : MonoBehaviour, InteractableObject
{
    [SerializeField]
    private TaskDefinition m_TaskDefinition;

    [SerializeField]
    private bool m_AllowMultipleUsers = false;

    protected List<Player> m_CurrentPlayers;
    private Coroutine m_TaskRoutineHandle;

    private void Awake()
    {
        m_CurrentPlayers = new List<Player>();
    }

    public bool CanInteract(Player player)
    {
        return true;
    }

    public bool IsInteracting(Player player)
    {
        if (m_CurrentPlayers == null)
            return false;

        return (m_CurrentPlayers.Contains(player));
    }

    public virtual void Interact(Player player)
    {
        if (m_CurrentPlayers.Count == 0 || m_AllowMultipleUsers == true)
        {
            if (IsInteracting(player) == false)
            {
                //Start the interaction
                m_CurrentPlayers.Add(player);
                m_TaskRoutineHandle = StartCoroutine(TaskRoutine(player));

                // Display the icon
                player.Icon.ShowSprite(m_TaskDefinition.Sprite);

                // Play animation
                player.CharacterAnimation.Play(CharacterAnimation.AnimationType.GeneralTask);
            }

            return;
        }

        // Hide icon
        for (int i = 0; i < m_CurrentPlayers.Count; ++i)
        {
            m_CurrentPlayers[i].Icon.Fail(false);

            // Play animation
            m_CurrentPlayers[i].CharacterAnimation.Play(CharacterAnimation.AnimationType.Idle);
        }

        //Cancel the interaction
        EndInteraction(false);

        m_CurrentPlayers.Clear();
        StopCoroutine(m_TaskRoutineHandle);
    }

    protected virtual void EndInteraction(bool finished)
    {

    }

    private IEnumerator TaskRoutine(Player player)
    {
        float timer = m_TaskDefinition.TimeToComplete;

        while (timer > 0.0f)
        {
			// UPDATE VISUALS
			float progress = (m_TaskDefinition.TimeToComplete - timer) / m_TaskDefinition.TimeToComplete;
            player.Icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Hide icon
        player.Icon.Win();

        // Play animation
        player.CharacterAnimation.Play(CharacterAnimation.AnimationType.Idle);

		player.UpdateTask(m_TaskDefinition);

        EndInteraction(true);
        m_CurrentPlayers.Remove(player);
    }
}

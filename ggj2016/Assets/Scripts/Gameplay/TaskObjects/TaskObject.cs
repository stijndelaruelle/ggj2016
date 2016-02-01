using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskObject : MonoBehaviour, InteractableObject
{
    [SerializeField]
    protected TaskDefinition m_TaskDefinition;

    [SerializeField]
    private bool m_AllowMultipleUsers = false;

    protected List<Player> m_CurrentPlayers;

	[Header("Audio Clip")]
	public soAudio _audioDefault;

	private void Awake()
    {
        m_CurrentPlayers = new List<Player>();
    }

    public bool IsUnlocked()
    {
        return (GameManager.Instance.Day >= m_TaskDefinition.UnlockDay);
    }

    public virtual bool CanInteract(Player player)
    {
        if (m_CurrentPlayers.Count == 0 || m_AllowMultipleUsers == true)
        {
            if (IsInteracting(player) == false)
            {
                return IsUnlocked();
            }
        }

        return false;
    }

    public bool IsInteracting(Player player)
    {
        if (m_CurrentPlayers == null)
            return false;

        return (m_CurrentPlayers.Contains(player));
    }

    public virtual void Interact(Player player)
    {
        if (CanInteract(player))
        {
            // Display the icon
            player.Icon.ShowSprite(m_TaskDefinition.Sprite);

            // Play animation
            player.CharacterAnimation.Play(CharacterAnimation.AnimationType.GeneralTask);

			// Play the audio (if any)
			if(_audioDefault != null)
			{
				player.PlayerAudio.Play(_audioDefault);
			}

            //Start the interaction
            m_CurrentPlayers.Add(player);
            StartCoroutine(TaskRoutine(player));
        }
    }

    public void CancelInteraction(Player player)
    {
        // Play animation
        player.CharacterAnimation.Play(CharacterAnimation.AnimationType.Idle);

        // Stop audio player
        player.PlayerAudio.Stop();

        //Cancel the interaction
        EndInteraction(false);

        m_CurrentPlayers.Remove(player);
    }

    protected virtual void EndInteraction(bool finished)
    {

    }

    private IEnumerator TaskRoutine(Player player)
    {
        float timer = m_TaskDefinition.TimeToComplete;

        while (timer > 0.0f && player.Icon.IsUsingDefaultSprite() == false)
        {
            if (!m_CurrentPlayers.Contains(player))
            {
                timer = 0.0f;
            }

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

using UnityEngine;
using System.Collections;

public class Newspaper : TaskObject
{
    [SerializeField]
    private Animator m_Animator;

	[Header("Audio Clips")]
	public soAudio _audioNewspaper;

	[Header("Components")]
	private AudioController _audio;

	private void Start()
    {
        GameManager.Instance.StartDayEvent += OnDayStart;
		_audio = GetComponentInChildren<AudioController>();
	}

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnDayStart;
    }

    public override void Interact(Player player)
    {
        Read(true);
		player.PlayerAudio.Play(_audioNewspaper);
		base.Interact(player);
    }

    protected override void EndInteraction(bool finished)
    {
        Read(false);
    }

    private void Read(bool state)
    {
        m_Animator.SetBool("isReading", state);
    }

    private void OnDayStart()
    {
        Read(false);
    }
}

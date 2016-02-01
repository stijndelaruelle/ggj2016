using UnityEngine;
using System.Collections;

public class Shower : TaskObject
{
    [SerializeField]
    private Icon m_Icon;
    public Icon Icon
    {
        get { return m_Icon; }
        set { m_Icon = value; }
    }

    [SerializeField]
    private float m_TimeToFillBath;
    private bool m_IsFilled = false;

    [SerializeField]
    private Animator m_Animator;

    private Coroutine m_RoutineHandle;

	[Header("Audio Clips")]
	public soAudio _audioShower;

	[Header("Components")]
	private AudioController _audio;

	private void Start()
    {
        GameManager.Instance.StartDayEvent += OnDayStart;
        m_Icon.Initialize();
		_audio = GetComponentInChildren<AudioController>();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnDayStart;
    }

    public override bool CanInteract(Player player)
    {
        //We can interact as long as we're not filling the bath
        if (m_RoutineHandle != null)
            return false;

        return base.CanInteract(player);
    }

    public override void Interact(Player player)
    {
        if (IsInteracting(player))
            return;

        if (player == null)
        {
            base.Interact(player);
            return;
        }

        if (m_RoutineHandle != null)
            return;

        if (m_IsFilled)
        {
            m_Animator.SetBool("showerEnabled", true);
			// Play audio
			player.PlayerAudio.Play(_audioShower);

			base.Interact(player);
            return;
        }

        //Fill the bath if it hasn't already
        m_RoutineHandle = StartCoroutine(FillBathRoutine());
    }

    protected override void EndInteraction(bool finished)
    {
        //Stop shower
        m_Animator.SetBool("showerEnabled", false);
    }

    private IEnumerator FillBathRoutine()
    {
        float timer = m_TimeToFillBath;

        m_Icon.Show();

		while (timer > 0.0f)
        {
            // UPDATE VISUALS
            float progress = (m_TimeToFillBath - timer) / m_TimeToFillBath;
            m_Icon.UpdateProgress(progress);

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Hide icon
        m_Icon.Win();
		FillBath(true);
        m_RoutineHandle = null;
    }

    private void FillBath(bool value)
    {
        m_IsFilled = value;
        m_Animator.SetBool("isFilled", value);
    }

    private void OnDayStart()
    {
        if (m_RoutineHandle != null)
            StopCoroutine(m_RoutineHandle);

        m_RoutineHandle = null;
        FillBath(false);
    }
}

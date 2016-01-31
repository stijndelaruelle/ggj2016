using UnityEngine;
using System.Collections;

public class Television : TaskObject
{
    [SerializeField]
    private Animator m_Animator;

    private void Start()
    {
        GameManager.Instance.StartDayEvent += OnDayStart;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnDayStart;
    }

    public override void Interact(Player player)
    {
        int channelID = 0;
        if (player.PlayerType == PlayerType.Parent) { channelID = 1; }
        if (player.PlayerType == PlayerType.Child)  { channelID = 2; }

        SetChannel(channelID);
        base.Interact(player);
    }

    private void SetChannel(int channel)
    {
        m_Animator.SetInteger("channelID", channel);
    }

    private void OnDayStart()
    {
        SetChannel(0);
    }
}

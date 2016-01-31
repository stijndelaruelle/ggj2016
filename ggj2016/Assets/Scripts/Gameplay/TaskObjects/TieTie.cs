using UnityEngine;
using System.Collections;

public class TieTie : TaskObject
{
    [SerializeField]
    private Player m_Owner;
    private bool m_IsTied = false;

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

    public override bool CanInteract(Player player)
    {
        return (player.PlayerType == PlayerType.Parent &&
                player.Gender == GenderType.Female &&
                !m_IsTied);
    }

    protected override void EndInteraction(bool finished)
    {
        m_Owner.UpdateTask(m_TaskDefinition);
        m_IsTied = true;
    }

    private void OnDayStart()
    {
        m_IsTied = false;
    }
}

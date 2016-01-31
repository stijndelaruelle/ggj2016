using UnityEngine;
using System.Collections;

public class TieShoes : TaskObject
{
    [SerializeField]
    private Player m_Owner;
    private bool m_AreTied = false;

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
        bool success = (player.PlayerType == PlayerType.Parent && !m_AreTied);

        if (success)
            return base.CanInteract(player);
        else
            return false;
    }

    protected override void EndInteraction(bool finished)
    {
        m_Owner.UpdateTask(m_TaskDefinition);
        m_AreTied = true;
    }

    private void OnDayStart()
    {
        m_AreTied = false;
    }
}

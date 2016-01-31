using UnityEngine;
using System.Collections;

public class Newspaper : TaskObject
{
    [SerializeField]
    private Animator m_Animator;

    public override void Interact(Player player)
    {
        Read(true);
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
}

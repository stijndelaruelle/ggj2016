using UnityEngine;
using System.Collections;

public class Fridge : TaskObject
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private SpriteRenderer m_DoorSpriteRenderer;

    [SerializeField]
    private Sprite m_ClosedSprite;

    [SerializeField]
    private Sprite m_OpenSprite;

    private void Start()
    {
        Open(false);
    }

    public override void Interact(Player player)
    {
        Open(true);
        base.Interact(player);
    }

    protected override void EndInteraction(bool finished)
    {
        Open(false);
    }

    private void Open(bool value)
    {
        if (value)
        {
            m_SpriteRenderer.sprite = m_OpenSprite;
            m_DoorSpriteRenderer.enabled = true;
        } 
        else
        {
            m_SpriteRenderer.sprite = m_ClosedSprite;
            m_DoorSpriteRenderer.enabled = false;
        }  
    }
}

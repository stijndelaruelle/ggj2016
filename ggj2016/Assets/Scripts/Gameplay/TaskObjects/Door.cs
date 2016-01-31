using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private SpriteRenderer m_OpenSpriteRenderer;

    [SerializeField]
    private Sprite m_ClosedDoor;

    [SerializeField]
    private Sprite m_OpenDoor;

    private int m_NumberOfPlayersInDoor = 0;

    private void Start()
    {
        Open(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ++m_NumberOfPlayersInDoor;

        if (m_NumberOfPlayersInDoor > 0)
            Open(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        --m_NumberOfPlayersInDoor;

        if (m_NumberOfPlayersInDoor <= 0)
            Open(false);
    }

    private void Open(bool state)
    {
        if (state)
        {
            m_SpriteRenderer.sprite = m_OpenDoor;

            if (m_OpenSpriteRenderer != null)
                m_OpenSpriteRenderer.enabled = true;
        }
        else
        {
            m_SpriteRenderer.sprite = m_ClosedDoor;

            if (m_OpenSpriteRenderer != null)
                m_OpenSpriteRenderer.enabled = false;
        }
    }
}

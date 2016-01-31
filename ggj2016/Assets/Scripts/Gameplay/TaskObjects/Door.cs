using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private Sprite m_ClosedDoor;

    [SerializeField]
    private Sprite m_OpenDoor;

    private int m_NumberOfPlayersInDoor = 0;

    private void Start()
    {
        m_SpriteRenderer.sprite = m_ClosedDoor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ++m_NumberOfPlayersInDoor;

        if (m_NumberOfPlayersInDoor > 0)
            m_SpriteRenderer.sprite = m_OpenDoor;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        --m_NumberOfPlayersInDoor;

        if (m_NumberOfPlayersInDoor <= 0)
            m_SpriteRenderer.sprite = m_ClosedDoor;
    }
}

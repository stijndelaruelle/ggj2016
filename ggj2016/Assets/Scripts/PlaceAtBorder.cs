using UnityEngine;
using System.Collections;

public class PlaceAtBorder : MonoBehaviour
{
    public enum Direction
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3
    }

    [SerializeField]
    private Direction m_Direction;

    private void Start()
    {
        Vector3 viewPos = new Vector3();

        switch (m_Direction)
        {
            case Direction.Left:
                viewPos.x = 0.0f;
                viewPos.y = 0.5f;
                break;

            case Direction.Right:
                viewPos.x = 1.0f;
                viewPos.y = 0.5f;
                break;

            case Direction.Top:
                viewPos.x = 0.5f;
                viewPos.y = 0.0f;
                break;

            case Direction.Bottom:
                viewPos.x = 0.5f;
                viewPos.y = 1.0f;
                break;
        }

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;
    }
}

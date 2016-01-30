using UnityEngine;
using System.Collections;

public class ResetPosition : MonoBehaviour
{
    private Vector3 m_OriginalPosition;

    private void Start()
    {
        m_OriginalPosition = transform.position.Copy();
        GameManager.Instance.StartDayEvent += OnStartDay;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartDayEvent -= OnStartDay;
    }

    private void OnStartDay()
    {
        transform.position = m_OriginalPosition;
    }
}

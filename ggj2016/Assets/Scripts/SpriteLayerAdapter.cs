using UnityEngine;
using System.Collections;

public class SpriteLayerAdapter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

	private void Start()
    {
        AdaptLayer();
    }

    protected void AdaptLayer()
    {
        m_SpriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}

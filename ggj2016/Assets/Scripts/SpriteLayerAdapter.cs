using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpriteLayerAdapter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    
	private void Start()
    {
        if (m_SpriteRenderer == null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        AdaptLayer();
    }

    private void Update()
    {
        #if (UNITY_EDITOR)
            AdaptLayer();
        #endif
    }

    protected void AdaptLayer()
    {
        m_SpriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}

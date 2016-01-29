using UnityEngine;
using System.Collections;

public class DynamicSpriteLayerAdapter : SpriteLayerAdapter
{
    private void Update()
    {
        AdaptLayer();
    }
}

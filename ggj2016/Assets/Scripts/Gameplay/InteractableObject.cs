using UnityEngine;
using System.Collections;

public interface InteractableObject
{
    bool CanInteract(Player player);
    void Interact(Player player);
}

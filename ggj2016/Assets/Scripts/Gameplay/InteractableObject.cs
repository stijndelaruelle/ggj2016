using UnityEngine;
using System.Collections;

public interface InteractableObject
{
    bool IsUnlocked();
    bool CanInteract(Player player);
    bool IsInteracting(Player player);
    void Interact(Player player);
    void CancelInteraction(Player player);
}

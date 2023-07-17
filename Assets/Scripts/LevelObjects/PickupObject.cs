using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : InteractableObject
{
    private PlayerInteractor playerInteractor;
    private bool isPickedUp;

    public override void Awake()
    {
        base.Awake();
        playerInteractor = FindObjectOfType<PlayerInteractor>();
    }
    public override void Interact()
    {
        base.Interact();
        Invoke("PickUp", 0.1f);
    }

    private void PickUp()
    {
        //If the player is already carrying something, don't carry something new.
        if(playerInteractor.IsCarrying()) return;

        groundTilemap.InteractableObjectCoordinates.Remove(coordinates.PositionOnGrid);
        playerInteractor.SetObjectCarrying(gameObject);
        SetInteractPrompt(false);
    }
}

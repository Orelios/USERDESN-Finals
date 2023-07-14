using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearObjects : MonoBehaviour
{
    [SerializeField] private List<InteractableObject> nearObjects = new List<InteractableObject>();
    public List<InteractableObject> ListOfNearObjects { get => nearObjects; }
    [SerializeField] private InteractableObject targetInteractable;
    private PlayerCoordinates coordinates;
    private void Awake()
    {
        coordinates = GetComponent<PlayerCoordinates>();
    }
    private void Update()
    {
        //Set the near object that the player is looking at as the object to interact with
        foreach(InteractableObject nearObject in nearObjects)
        {
            if(nearObject.DirectionFromPlayer == coordinates.DirectionFacing)
            {
                targetInteractable = nearObject;
            }
        }

        if(targetInteractable != null && !nearObjects.Contains(targetInteractable))
        {
            targetInteractable = null;
        }

        foreach(InteractableObject nearObject in nearObjects)
        {
            nearObject.SetInteractPrompt(nearObject == targetInteractable ? true : false);
        }
    }

    private void OnInteract()
    {
        if(targetInteractable == null) return;
        targetInteractable.Interact();
    }
}

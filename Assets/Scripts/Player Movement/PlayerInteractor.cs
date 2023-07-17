using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameObject objectCarrying;
    [SerializeField] private Vector2 offset;
    private NearObjects nearObjects;
    private PlayerCoordinates coordinates;

    private void Awake()
    {
        nearObjects = GetComponent<NearObjects>();
        coordinates = GetComponent<PlayerCoordinates>();
    }

    public void SetObjectCarrying(GameObject objectToCarry)
    {
        objectCarrying = objectToCarry;

        InteractableObject interactableObject = objectToCarry.GetComponent<InteractableObject>();

        //Set as not interactable
        interactableObject.IsInteractable = false;

        //Remove the player from near objects
        nearObjects.ListOfNearObjects.Remove(interactableObject);

        //Set the gameobject as a child of the player
        objectToCarry.transform.SetParent(transform);

        //Set the transform of the object to the player's position plus the offset
        objectToCarry.transform.position = transform.position + (Vector3)offset;

        //Put the object in front
        objectToCarry.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public bool IsCarrying()
    {
        return objectCarrying != null;
    }

    private void OnInteract()
    {
        if(objectCarrying == null) return;
        Place();
    }

    private void Place()
    {
        //Do nothing if the player is carrying nothing.
        if(!objectCarrying) return;

        Vector3 targetPos = coordinates.GroundTilemap.GetCellCenterWorld(coordinates.GroundTilemap.WorldToCell(transform.position) + PlacePosition(coordinates.DirectionFacing));

        GroundTilemap groundTilemap = FindObjectOfType<GroundTilemap>();
        
        //Do nothing if the target position to place the object is not free.
        if(!groundTilemap.IsTargetTileFree(coordinates.GroundTilemap.WorldToCell(targetPos)) || groundTilemap.PushableObjectCoordinates.Contains((Vector2Int)coordinates.GroundTilemap.WorldToCell(targetPos)) || groundTilemap.InteractableObjectCoordinates.Contains((Vector2Int)coordinates.GroundTilemap.WorldToCell(targetPos))) return;

        //Set the transform of the carried object to the space in front of where the player is facing
        objectCarrying.transform.position = targetPos;
        objectCarrying.GetComponent<Coordinates>().SetCurrentPositionOnGrid();

        //Return the object to its initial sorting order
        objectCarrying.GetComponent<SpriteRenderer>().sortingOrder = 0;

        //Disown the object
        objectCarrying.transform.parent = null;

        InteractableObject interactableObject = objectCarrying.GetComponent<InteractableObject>();

        //Set the object to interactable again
        interactableObject.IsInteractable = true;

        //Add the object to near objects
        interactableObject.CheckPlayerPosition(coordinates.PositionOnGrid, coordinates.DirectionFacing, true);

        groundTilemap.InteractableObjectCoordinates.Add(objectCarrying.GetComponent<Coordinates>().PositionOnGrid);

        objectCarrying = null;
    }

    private Vector3Int PlacePosition(Direction playerFaceDirection)
    {
        switch(playerFaceDirection)
        {
            case Direction.North: return Vector3Int.up;
            case Direction.South: return Vector3Int.down;
            case Direction.East: return Vector3Int.right;
            case Direction.West: return Vector3Int.left;
            default: return Vector3Int.zero;
        }
    }
}

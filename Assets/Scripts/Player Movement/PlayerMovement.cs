using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private float movementSpeed;
    private Vector3 direction;
    private List<Vector3> targets = new List<Vector3>();
    private PlayerControls controls;
    private bool isMoving;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        controls.Actions.Move.started += _ => StartMoving();
    }

    private void Update()
    {
        //If there is a target, and the player has reached that target...
        if(targets.Count != 0 && HasReachedTarget())
        {
            //...remove that target.
            targets.RemoveAt(0);
            isMoving = false;
            //If the player is still pressing down on the same input...
            if(direction == (Vector3)controls.Actions.Move.ReadValue<Vector2>() && direction != Vector3.zero)
            {
                //...keep moving.
                StartMoving();
            }
            //Otherwise...
            else
            {
                //...stop moving.
                return;
            }
        }

        //If there is (still) a target to move to...
        if(targets.Count != 0) 
        {
            //...move to target.
            transform.position = Vector3.MoveTowards(transform.position, targets[0], movementSpeed * Time.deltaTime);
        }
    }

    private void StartMoving()
    {
        Vector3 currentInput = (Vector3)controls.Actions.Move.ReadValue<Vector2>();
        Vector3Int targetTile;

        //If the player has not yet reached the target and input is not the same as the previous input...
        if(targets.Count != 0 && !HasReachedTarget() && direction != currentInput) 
        {
            direction = currentInput;
            //...add the next move to the buffer.
            targetTile = groundTilemap.WorldToCell(targets[0] + currentInput);
            if(targets.Count > 1)
                targets[1] = groundTilemap.GetCellCenterLocal(targetTile);
            else
                targets.Add(groundTilemap.GetCellCenterLocal(targetTile));
        }
        //If the player has reached the target or is still moving in the same direction...
        else 
        {
            if(isMoving) return;
            direction = currentInput;
            targetTile = groundTilemap.WorldToCell(transform.position + direction);

            if(IsTargetTileFree(targetTile))
            {
                //...set the target point
                targets.Add(groundTilemap.GetCellCenterLocal(targetTile));
                isMoving = true;
            }
        }
    }

    private bool IsTargetTileFree(Vector3Int tilePosition)
    {
        return (groundTilemap.HasTile(tilePosition) /* && there are no obstacles*/) ? true : false;
    }

    private bool HasReachedTarget()
    {
        return (Vector3.Distance(transform.position, targets[0]) == 0) ? true : false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}

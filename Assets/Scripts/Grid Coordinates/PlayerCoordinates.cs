using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction 
{
    North,
    South,
    East,
    West
}

public class PlayerCoordinates : Coordinates
{
    [SerializeField] private Direction directionFacing;
    public Direction DirectionFacing { get => directionFacing; }
    public void FaceToTarget(Vector3 target)
    {
        if(target.x > transform.position.x) directionFacing = Direction.East;
        if(target.x < transform.position.x) directionFacing = Direction.West;
        if(target.y > transform.position.y) directionFacing = Direction.North;
        if(target.y < transform.position.y) directionFacing = Direction.South;
    }
}

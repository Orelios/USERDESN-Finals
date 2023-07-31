using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoordinates : Coordinates
{
    [SerializeField] private Direction directionFacing;
    [Header("Facing Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteLeft;
    private SpriteRenderer spriteRenderer;
    public Direction DirectionFacing { get => directionFacing; }
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void FaceToTarget(Vector3 target)
    {
        if(target.x > transform.position.x) directionFacing = Direction.East;
        if(target.x < transform.position.x) directionFacing = Direction.West;
        if(target.y > transform.position.y) directionFacing = Direction.North;
        if(target.y < transform.position.y) directionFacing = Direction.South;
        SetSpriteToFacing();
    }
    private void SetSpriteToFacing()
    {
        switch(directionFacing)
        {
            case Direction.North:
                spriteRenderer.sprite = spriteUp;
                break;
            case Direction.South:
                spriteRenderer.sprite = spriteDown;
                break;
            case Direction.East:
                spriteRenderer.sprite = spriteRight;
                break;
            default:
                spriteRenderer.sprite = spriteLeft;
                break;
        }
    }

    
}

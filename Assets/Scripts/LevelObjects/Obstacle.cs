using UnityEngine;
public class Obstacle : LevelObject
{
    PlayerMovement playerMovement;

    public override void Awake()
    {
        base.Awake();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        playerMovement.ObstacleCoordinates.Add(coordinates.PositionOnGrid);
    }
}

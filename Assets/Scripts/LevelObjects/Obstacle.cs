using UnityEngine;
public class Obstacle : LevelObject
{
    private void Start()
    {
        coordinates.GroundTilemap.gameObject.GetComponent<GroundTilemap>().ObstacleCoordinates.Add(coordinates.PositionOnGrid);
    }
}

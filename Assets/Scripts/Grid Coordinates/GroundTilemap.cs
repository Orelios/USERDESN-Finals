using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTilemap : MonoBehaviour
{
    private Tilemap groundTilemap;
    public Tilemap Tilemap { get => groundTilemap; }
    //Obstacles
    private List<Vector2Int> obstacleCoordinates = new List<Vector2Int>();
    public List<Vector2Int> ObstacleCoordinates { get => obstacleCoordinates; }

    private void Awake()
    {
        groundTilemap = GetComponent<Tilemap>();
    }

    public bool IsTargetTileFree(Vector3Int tilePosition)
    {
        //Check if tilePosition has an obstacle
        foreach(Vector2Int obstacleCoordinate in obstacleCoordinates)
        {
            //If there is an obstacle in the way...
            if(obstacleCoordinate == (Vector2Int)tilePosition)
            {
                //...target tile is not free.
                return false;   
            }
        }
        return (groundTilemap.HasTile(tilePosition));
    }
}

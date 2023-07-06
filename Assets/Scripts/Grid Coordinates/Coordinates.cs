using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction 
{
    North,
    South,
    East,
    West
}

public class Coordinates : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    public Tilemap GroundTilemap { get => groundTilemap; }
    [SerializeField] private Vector2Int positionOnGrid;
    public Vector2Int PositionOnGrid { get => positionOnGrid; }

    private void Awake()
    {
        groundTilemap = GameObject.Find("GroundTilemap").GetComponent<Tilemap>();
    }

    private void OnEnable() //Would be better to subscribe it to a Level Start event
    {
        SetCurrentPositionOnGrid();
        transform.position = groundTilemap.GetCellCenterLocal((Vector3Int)positionOnGrid); //Snap the object to the center of the cell.
    }

    //Subscribe or call this function whenever the object is initialized or changes position
    public void SetCurrentPositionOnGrid()
    {
        positionOnGrid = (Vector2Int)groundTilemap.WorldToCell(transform.position);
    }
}

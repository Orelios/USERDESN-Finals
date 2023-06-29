using UnityEngine;
using UnityEngine.Tilemaps;

public class Coordinates : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Vector2Int positionOnGrid;
    public Vector2Int PositionOnGrid { get => positionOnGrid; }

    private void OnEnable() //Would be better to subscribe it to a Level Start event
    {
        SetCurrentPositionOnGrid();
    }

    //Subscribe or call this function whenever the object is initialized or changes position
    public void SetCurrentPositionOnGrid()
    {
        positionOnGrid = (Vector2Int)groundTilemap.WorldToCell(transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap hexTilemap; // Public reference to Tilemap Component.
    private Dictionary<Vector3Int, HexTile> hexTiles =  new Dictionary<Vector3Int, HexTile>(); // Dictionary to store hextile objects with their positions as keys. This allows efficent lookup via positions.

    void Start()
    {
        InitialiseTiles();
    }

    
    void InitialiseTiles()
    {
        foreach (var pos in hexTilemap.cellBounds.allPositionsWithin) //Loop through all positions within cell bounds.
        {
            if (hexTilemap.HasTile(pos)) //Check for tile at current pos.
            {
                TileBase tile = hexTilemap.GetTile(pos); //Get tile at current pos.
                if (tile != null)
                {
                    hexTiles[pos] = new HexTile(pos); //If tile present create a new HexTile and add it to dictionary.
                }
            }
        }
    }


    public HexTile GetHexTile(Vector3Int position) //Get HexTile object at specific positon.
    {
        hexTiles.TryGetValue(position, out HexTile hexTile); //Try to get HexTile from dictionary and return it.
        return hexTile;
    }

    public bool IsPositionWithinBounds(Vector3Int position) //Check if position is within bounds of tilemap.
    {
        return hexTilemap.cellBounds.Contains(position); //Return true if position is within tilemaps cell bounds.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap hexTilemap; // Public reference to Tilemap Component.
    public Tile tileElevation0;
    public Tile tileElevation1;
    public Tile tileElevation2;
    public Tile tileElevation3;
    public Tile tileElevation4;
    
    private Dictionary<Vector3Int, HexTile> hexTiles =  new Dictionary<Vector3Int, HexTile>(); // Dictionary to store hextile objects with their positions as keys. This allows efficent lookup via positions.

    private Dictionary<TileBase, int> tileElevations; // Dictionary to associate tile assets with their elevations

    void Start()
    {
        tileElevations = new Dictionary<TileBase, int> // Creates an assigns new dictionary to store elevation values for each tile type
        {
            {tileElevation0, 0},
            {tileElevation1, 1},
            {tileElevation2, 2},
            {tileElevation3, 3},
            {tileElevation4, 4},

        };

        InitialiseTiles();
    }

    
    void InitialiseTiles()
    {
        foreach (var pos in hexTilemap.cellBounds.allPositionsWithin) //Loop through all positions within cell bounds.
        {
            if (hexTilemap.HasTile(pos)) //Check for tile at current pos.
            {
                TileBase tile = hexTilemap.GetTile(pos); //Get tile at current pos.
                if (tile != null && tileElevations.ContainsKey(tile)) // Ensures tile isnt null and is a key in tileElevations
                {
                    int elevation = tileElevations[tile]; // Looks up elevation value associated with that tile
                    hexTiles[pos] = new HexTile(pos, elevation); //If tile present create a new HexTile and add it to dictionary.
                }
                else
                {
                    hexTiles[pos] = new HexTile(pos, 0);  //Default 0 for unknown tiles
                    Debug.Log("Missing Elevation"); //Logs unknown elevation
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
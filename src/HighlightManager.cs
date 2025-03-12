using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap highlightTilemap;
    public Tile highlightTile;
    private TilemapManager TilemapManager;
    private ObjectiveManager objectiveManager;
    private List<Vector3Int> highlightedTiles = new List<Vector3Int>();

    void Start()
    {
        //Find Managers Within Scene
        TilemapManager = FindObjectOfType<TilemapManager>();
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

        // Highlight Adjacent Tiles Around a Central Tile
    public void HighlightAdjacent(Vector3Int center)
    {
        ClearHighlights(); // Remove prior highlights
        //Convert center positions to world coordinates
        Vector3 centerWorld = groundTilemap.CellToWorld(center);
        HexTile centerTile = TilemapManager.GetHexTile(center);
        int centerElevation = centerTile != null ? centerTile.Elevation : 0;
        
        
        Vector3[] worldOffsets = new Vector3[] // Defines relative positions of flat top hex numbers within world space. Exact values based on Unity's tilemap system.
        {
            new Vector3(0, 0.866f, 0),      // Top
            new Vector3(-0.75f, 0.433f, 0), // Top-Left (Left)
            new Vector3(0.75f, 0.433f, 0),  // Top-Right (Right)
            new Vector3(0, -0.866f, 0),     // Bottom
            new Vector3(-0.75f, -0.433f, 0), // Bottom-Left
            new Vector3(0.75f, -0.433f, 0)  // Bottom-Right
        };

        Debug.Log($"Rover center: {center}, ground world: {centerWorld}");
        foreach (var offset in worldOffsets) // Loop through each neighbour
        {
            Vector3 targetWorld = centerWorld + offset; // Calculate its world position
            Vector3Int tilePosition = groundTilemap.WorldToCell(targetWorld); // Convert this world position to tilemap
            if (groundTilemap.HasTile(tilePosition)) // If a tile exists at this tilemap position
            {
                HexTile targetTile =TilemapManager.GetHexTile(tilePosition); // Retrieve HexTile for this Tile
                if (targetTile != null)
                { 
                    int elevationDiff = Mathf.Abs(targetTile.Elevation - centerElevation); // Absolute difference between centre tile and neighbour tiles elevation (absolute calculation ensures its correct for positive and negative)

                    bool isWinTile = objectiveManager.isWinTile(tilePosition); // Check if neighbour tile is win tile.

                    if (isWinTile || elevationDiff <=1) // If neighbour has the correct elevation difference or is a win tile it can be highlighted allowing a move,
                    {
                        Vector3 groundWorld = groundTilemap.CellToWorld(tilePosition); // Grid to world
                        Debug.Log($"Ground tile: {tilePosition} at world {groundWorld}"); // Log Neighbours Grid and World Coordinates for debugging
                        highlightTilemap.SetTile(tilePosition, highlightTile); // Sets highlight tile on highlight tilemap on the same grid position
                        highlightedTiles.Add(tilePosition); // Add this position to highlightedTiles list.
                        Vector3 highlightWorld = highlightTilemap.CellToWorld(tilePosition); // Converts highlighted tiles position to world coordinates. (purely to log them)
                        Debug.Log($"Highlighted: {tilePosition} at world {highlightWorld}"); // Log highlighted tiles Grid and World coordinates.
                    }
                }
            }
        }
    }

    public void ClearHighlights() //Removes all highlighted tiles
    {
        foreach (var tilePosition in highlightedTiles) //Iterate through each position on highlightedTiles list
        {
            highlightTilemap.SetTile(tilePosition, null); //Set tile at that position to null.
        }
        highlightedTiles.Clear(); // Remove all entries from highlightedTiles list.
    }

    public bool IsHighlighted(Vector3Int position) // Check if position is highlighted.
    {
        return highlightedTiles.Contains(position); // Checks if position exists in highlightedTiles list.
    }
}
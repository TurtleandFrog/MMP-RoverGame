using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Rover : MonoBehaviour
{
    public Vector3Int Position { get; set; } //Rovers current tilemap position
    public int moveRange = 3; // How many tile a rover can move per "turn"
    public int maxBattery = 100; // Rovers max battery capacity
    private int currentBattery; // Rovers current battery capacity
    private HighlightManager highlightManager;
    private ObjectiveManager objectiveManager;
    private TilemapManager TilemapManager;
    private List<Vector3Int> path = new List<Vector3Int>(); // List to store planned path
    private int remainingMoves; // Moves left in this "turn"
    public Text batteryText; // Reference to batteryText
    public GameObject gameOverScreen; // Reference to gameOverScreen

    void Start() 
    {
        highlightManager = FindObjectOfType<HighlightManager>();
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        TilemapManager = FindObjectOfType<TilemapManager>();
        Position = highlightManager.groundTilemap.WorldToCell(transform.position); // Convert rover world position to tilemap cell position
        Vector3 worldPos = highlightManager.groundTilemap.CellToWorld(Position); // Convert grid position to world coordinates for debugging
        Debug.Log($"Rover at {Position}, world {worldPos}");
        remainingMoves = moveRange; // Sets remaining moves to full moveRange
        highlightManager.HighlightAdjacent(Position); // Show reachable tiles from starting position.
        currentBattery = maxBattery; // Set current battery to max battery
        DisplayStatus(); // Show initial battery on UI
    }

    public void AddMove(Vector3Int targetPosition)
    {
        if (remainingMoves > 0 && highlightManager.IsHighlighted(targetPosition)) // Check if there are moves remaining and target is highlighted
        {
            HexTile currentTile = TilemapManager.GetHexTile(Position); // Retrieve HexTile for rovers current position
            HexTile targetTile = TilemapManager.GetHexTile(targetPosition); // Retrieve HexTile for rovers target position
            int currentElevation = currentTile != null ? targetTile.Elevation : 0; // Gets elevation for currentTile, if null set to 0
            int targetElevation = targetTile != null ? targetTile.Elevation : 0; // Gets elevation for targetTile, if null set to 0 
            int elevationDiff = Mathf.Abs(targetElevation - currentElevation); // Calculate absolute difference between the two elevations
    

            if (elevationDiff <= 1 && targetElevation != -1) // Check if move is valid (-1 will be used as a blocker)
            {
                path.Add(targetPosition); // Add target position to movement path
                Position = targetPosition; // Set rovers current position to target position
                remainingMoves--; // Reduces the number of moves left by 1
                currentBattery -= 5; // Reduces the battery amount by 5 per move
                if (remainingMoves > 0) //Check if more moves are left
                {
                    highlightManager.HighlightAdjacent(Position); // Highlight new neighbour tiles
                }
                else
                {
                    highlightManager.ClearHighlights(); // Otherwise clear all highlights.
                }
            }
                DisplayStatus(); // Update UI
        }
    }

    public void UndoMove() //Revert last move in path
    {
        if (path.Count > 0) // Check there are moves
        {
            Vector3Int lastMove = path[path.Count -1]; //Retrieve last position
            path.RemoveAt(path.Count - 1); // Remove last move from path
            Position = path.Count > 0 ? path[path.Count - 1] : highlightManager.groundTilemap.WorldToCell(transform.position); // Set position to previous path entry (or initial position)
            remainingMoves++; // Add one to remaining moves
            currentBattery += 5; // Add 5 back to battery
            highlightManager.HighlightAdjacent(Position); // Update highlighted positions to show the undone move
            DisplayStatus(); // Update UI
        }
    }
    public void ExecutePath() // Execute planned movement path
    {
        StartCoroutine(MoveThroughPath()); 
        remainingMoves = moveRange; // Restore full move range for next "turn"
    }

    private IEnumerator MoveThroughPath() // CoRoutine to move through path. Moves through each position with a delay.
    {
        foreach (var targetPosition in path) // Loop through each position in path
        {
            transform.position = highlightManager.groundTilemap.CellToWorld(targetPosition); // Update rover world position to match tilemap positions world coordinates
            Position = targetPosition; // Update rover's grid position to current target
            yield return new WaitForSeconds(0.1f); // Pause for 0.1 seconds (makes it look animated, temporary before animation)

                objectiveManager.CheckForWin(Position); // Check if rover position is a win tile
                if (!objectiveManager.isWinTile(Position)) // If it isnt a win tile
                {
                    CheckForGameOver(); // Check if the rover battery is depleted to end the game
                }
          }
            path.Clear(); // Clear the path
            highlightManager.HighlightAdjacent(Position); // Highlight new adjacent tiles of rover position
            DisplayStatus(); // Update UI
        }

        private void CheckForGameOver() // Check if battery is depleted for GameOver (This needs moving to ObjectiveManager)
        {
            if ( currentBattery <= 0 && !objectiveManager.isWinTile(Position)) // If current battery is 0 or less and rover isnt on a win tile.
            {
                Time.timeScale = 0; // Pause Game
                gameOverScreen.SetActive(true); // Activate game over panel
            }
        }

        private void DisplayStatus() // Update UI
        {
            if(batteryText != null) // Check batteryText is assigned
            {
                batteryText.text = $"Battery: {currentBattery}%"; // Set text component to show current battery percentage.
            }
        }
    }
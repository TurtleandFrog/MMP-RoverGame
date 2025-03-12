using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectiveManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase winTile;
    public GameObject winScreen;


    public bool isWinTile(Vector3Int position) // Check if tile at position is win tile.
    {
        TileBase tileAtPosition = tilemap.GetTile(position); //Checks tilemap position for tilebase.
        return tileAtPosition == winTile; // Compares found tile with winTile, returning true if they're the same.
    }

    public void CheckForWin(Vector3Int roverPosition) // Check if rover is on a win tile.
    {
        if (isWinTile(roverPosition)) // Check if roverPosition is a win tile.
        {
        Time.timeScale = 0; // Pause the game
        winScreen.SetActive(true); // Activate the win screen
        }
    }
}

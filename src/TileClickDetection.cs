using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileClickDetection : MonoBehaviour
{

    public Tilemap groundTilemap; //Tilemap component reference.
    public Rover rover; //Rover gameobject reference.
    private HighlightManager highlightManager;

    void Start()
    {
        highlightManager = FindObjectOfType<HighlightManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Check for left mouse click.
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get world position of mouse click.
            mouseWorldPos.z = 0; //Set z to 0 to match plane of game scene.
            Debug.Log("Mouse World Position: " + mouseWorldPos); //Log mouse position.

            Vector3Int clickedCell = groundTilemap.WorldToCell(mouseWorldPos); //Convert mouse world position to tilemap cell position.
            Debug.Log("Clicked Cell: " + clickedCell); //Log clicked cell position.
    
        
            if (groundTilemap.HasTile(clickedCell) && highlightManager.IsHighlighted(clickedCell))  //Check if there is a tile at clicked cell position.
            {
                Debug.Log("Tile Exists At: " + clickedCell); // Log tile exists.
                rover.AddMove(clickedCell); //Move rover to clicked cell.
            }
            else
            {
                Debug.Log("No Tile At: " + clickedCell); // Log no tile.
            }
        }
        
    if (Input.GetKeyDown(KeyCode.Space))
    {
        rover.ExecutePath();
    }

    if (Input.GetKeyDown(KeyCode.Z))
    {
        rover.UndoMove();
    }   

    }
}

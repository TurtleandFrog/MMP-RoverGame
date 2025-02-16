using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{

    public Vector3Int Position //Get and Set rover position in tilemap coordinates.
    {
        get;
        set;
    }
    private TilemapManager tilemapManager; //Reference to TilemapManager Script.
    private DirectMovement directMovement; // Reference to DirectMovement Script.

    void Start()
    {
        tilemapManager = FindObjectOfType<TilemapManager>(); // Find and assign TilemapManager.
        directMovement = new DirectMovement(); // Find and assign DirectMovement.
        Position = tilemapManager.hexTilemap.WorldToCell(transform.position); // Convert Rover's world position to tilemap coordinates. Assign these coordinates to Position.
    }

    public void MoveTo(Vector3Int targetPosition) //Move Rover to Target Position.
    {
        if (tilemapManager.GetHexTile(targetPosition) != null) //Check Target Position is a valid tile.
        {
            List<Vector3Int> path = directMovement.FindPath(Position, targetPosition); //Find path from Current Position to Target Position.
            if (path.Count > 0) //Check valid path exists.
            {
                StartCoroutine(FollowPath(path)); //Start Coroutine to FollowPath.
            }
        }
    }

    private IEnumerator FollowPath(List<Vector3Int> path) //Coroutine to follow path.
    {
        foreach (Vector3Int pos in path) // Loop through each path position.
        {
            transform.position = tilemapManager.hexTilemap.CellToWorld(pos); //Convert tilemap position to world coordinates. Move rover to that position.
            Position = pos; //Update Position
            yield return new WaitForSeconds(0.2f); //Wait 0.2 seconds before next movement.
        }
    }

}

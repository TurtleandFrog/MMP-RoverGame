using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectMovement
{
  public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal) //Findpath method from start position to goal position.
  {
    List<Vector3Int> path = new List<Vector3Int>(); //Create list to store the path.
    Vector3Int current = start; // Set current position as start position.


    while (current != goal) //Loop until current position = goal position.
    {
        Vector3Int direction = new Vector3Int(
            Mathf.Clamp(goal.x - current.x, -1, 1), // Calculates difference between goal and current x coordinate. Mathf.Clamp ensures the difference is between -1 and 1, therefore the movement appears in "steps".
            Mathf.Clamp(goal.y - current.y, -1, 1),
            Mathf.Clamp(goal.z - current.z, -1, 1)
        );
    
        current += direction; //Update current position by adding direction vector.
        path.Add(current); //Add updated current position to path.
    } 
    return path; //Return calculated path.
  }

}
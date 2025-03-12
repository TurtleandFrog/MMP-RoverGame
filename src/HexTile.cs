using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    public Vector3Int Position { get; set; }
    public int Elevation { get; set; }    

    public HexTile(Vector3Int position, int elevation) //Constructor for HexTile class.
    {
        Position = position; // Set Position to the provided position.
        Elevation = elevation; // Set Elevation to the provided elevation.
    }
}
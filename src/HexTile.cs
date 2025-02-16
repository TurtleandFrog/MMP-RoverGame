using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    public Vector3Int Position //Get and set HexTile position in Tilemap.
    {
        get;
        set;
    }

    public HexTile(Vector3Int position) //Constructor for HexTile class.
    {
        Position = position; // Set Position to the provided position.
                            // Add elevation here.


    }
}

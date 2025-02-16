using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGenerator : MonoBehaviour
{
    public Transform sun; //Reference to sun to centre orbits.
    public int numOrbits; // Variable for number of orbits. 
    public float[] orbitRadius; //Array for orbit radii.
    public Material orbitMaterial; // Reference to material to render orbits.

    void Start()
    {
        for (int i = 0; i < numOrbits; i++) //Loops through each orbit
        {
            GenerateOrbit(i); //Calls GenerateOrbit for that orbit.
        }
    }

    void GenerateOrbit(int index)
    {
        GameObject orbit = new GameObject("Orbit " + index); //Create and name new orbit gameobject.
        orbit.transform.position = sun.position; //Set new orbit position to suns position.

        LineRenderer lineRenderer = orbit.AddComponent<LineRenderer>(); //Add LineRenderer component to orbit game object.
        lineRenderer.widthMultiplier = 0.05f; // Set width of new orbit line.
        lineRenderer.material = orbitMaterial; // Add material to new orbit line.

        int segments = 100; //Define number of segments. 100 segments provides a smooth circle whilst balancing performance.
        lineRenderer.positionCount = segments + 1; //Set number of positions. Segments + 1 ensures the last point connects to the first point.
        lineRenderer.useWorldSpace = false; // Use local space coordinates over world space coordinates.

        float angle = 0f; //Initalise angle variable to 0.
        for (int i =0; i < (segments + 1); i++) // Loop through each segment.
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * orbitRadius[index]; //Calculate X position using sine of the angle and the radius.
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * orbitRadius[index]; //Calculate Y position using cosine of the angle and the radius.

            lineRenderer.SetPosition(i, new Vector3(x, y, 0)); //Set the new position in the LineRenderer.
            angle += 360f / segments; //Increment the angle by step size. Step size =360/segments.
        }
    }
}

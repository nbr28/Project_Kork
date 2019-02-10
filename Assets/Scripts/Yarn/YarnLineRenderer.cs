using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnLineRenderer : MonoBehaviour {

    public Color c1 = Color.yellow; // Gradient color 1
    public Color c2 = Color.red;    // Gradient color 2

    private Vector3 a = new Vector3(0f, 0f, 0f); // Endpoint 1
    private Vector3 b = new Vector3(0f, 0f, 0f); // Endpoint 2 (actual order is arbitrary)

    private int yarnID;
    private int fromID;
    private int toID;

    public Vector3 A
    {
        get
        {
            return a;
        }

        set
        {
            a = value;
        }
    }

    public Vector3 B
    {
        get
        {
            return b;
        }

        set
        {
            b = value;
        }
    }

    public int YarnID
    {
        get
        {
            return yarnID;
        }

        set
        {
            yarnID = value;
        }
    }

    public int FromID
    {
        get
        {
            return fromID;
        }

        set
        {
            fromID = value;
        }
    }

    public int ToID
    {
        get
        {
            return toID;
        }

        set
        {
            toID = value;
        }
    }

    void Awake()
    {
        createLineRenderer();
    }

    void Update()
    {
        refreshYarnLines();
    }

    // Creates and sets up the LineRenderer component
    public void createLineRenderer()
    {
        // Instantiate a new LineRenderer object and add it as a component to this GameObject
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the line graphic's material
        lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        // Set the line's width (thiccness?)
        lineRenderer.widthMultiplier = 0.2f;

        // Define a simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 0.5f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );

        // Set the gradient as the LineRenderer's color
        lineRenderer.colorGradient = gradient;
    }

    // Reconstructs the line based on the current state of the object, used to update appearance
    // Called at Update() to ensure the graphics reflect the most current state of the line
    public void refreshYarnLines()
    {
        // Get the lineRenderer component we added during initialization
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // Create an array of Vector3 points using the endpoint properties
        Vector3[] linePoints = { a, b };

        // Set the number of line vertices there are based on the length of linePoints array
        lineRenderer.positionCount = linePoints.Length;

        // Set the Vector3 points in linePoints array as the line vertices
        lineRenderer.SetPositions(linePoints);
    }

    // Given a snippet's ID, returns true/false if the snippet is either of the yarn line's endpoints
    public bool isAttachedSnippet(int snippetID)
    {
        return (snippetID == fromID || snippetID == toID);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour {

    //Structure of associations
    GameObject[] tagged;

    public string groupname = "";
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 20;


    // Use this for initialization
    [Obsolete("Using Yarn line renderer now")]
    void Start () {
        //Debug.Log("Spawned Linemaker");
        tagged = GameObject.FindGameObjectsWithTag(groupname);

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );

        lineRenderer.colorGradient = gradient;
    }
	
	// Update is called once per frame
	void Update () {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        List<Vector3> temp = new List<Vector3>();

        foreach (GameObject t in tagged)
        {
            if (t.activeSelf)
            {
                temp.Add(t.transform.position);
            }
        }

        Vector3[] positionsOfPoints = temp.ToArray();

        lineRenderer.positionCount = positionsOfPoints.Length;
        //lineRenderer.SetVertexCount(positionsOfPoints.Length);

        lineRenderer.SetPositions(positionsOfPoints);
    }
}

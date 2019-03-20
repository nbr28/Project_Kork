using UnityEngine;

public class dragAndDrop : MonoBehaviour
{
    private SnippetState state;
    private Vector3 dist;
    private float posX;
    private float posY;
    private float posZ;
    private bool dragging = false;

    public bool Dragging
    {
        get
        {
            return dragging;
        }

        set
        {
            dragging = value;
        }
    }

    void OnMouseDown()
    {
        if (Dragging)
        {
            dist = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - dist.x;
            posY = Input.mousePosition.y - dist.y;
            posZ = Input.mousePosition.z - dist.z;
        }     
    }

    void OnMouseDrag()
    {
        if (Dragging)
        {
            Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, Input.mousePosition.z - posZ);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            transform.position = worldPos;

            state = GetComponentInChildren<SnippetState>();
            state.x = transform.position.x;
            state.y = transform.position.y;
            state.z = transform.position.z;
        }
    }
}

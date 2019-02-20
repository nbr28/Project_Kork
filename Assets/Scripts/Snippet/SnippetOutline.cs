using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnippetOutline : MonoBehaviour
{

    private Outline outline;

    void Start()
    {
        outline = this.transform.Find("Outline Box").GetComponent<Outline>();
        outline.enabled = false;
    }

    public void setToggle(bool state)
    {
        outline.enabled = state;
    }

    public void toggle()
    {
        outline.enabled = !outline.enabled;
    }

    public bool isToggled()
    {
        return outline.enabled;
    }

    public void setOutlineColor(int r, int g, int b, int a)
    {
        outline.OutlineColor = new Color(r, g, b, a);
    }
}

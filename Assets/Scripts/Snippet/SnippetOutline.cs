using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnippetOutline : MonoBehaviour
{

    private QuickOutline outline;

    void Start()
    {
        outline = this.transform.Find("Outline Box").GetComponent<QuickOutline>();
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

    public void setOutlineColor(float r, float g, float b, float a)
    {
        outline.OutlineColor = new Color(r/255, g/255, b/255, a/255);
    }
}

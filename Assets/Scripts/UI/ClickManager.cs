﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public UIManager uiManager;
    public Button showSnippetContentButton; //There should be a better solution to this, but we'll deal with it later

    private int clickMode; // 0 = normal selection, 1 = yarn selection

    private Transform lastHit;
    private Transform objectHit;

    private void Awake()
    {
        clickMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (clickMode == 0)
        {
            normalClickSelect();
        }
        else if (clickMode == 1)
        {
            yarnClickSelect();
        }
    }

    public void setClickMode(int mode)
    {
        clickMode = mode;
    }

    public int getClickMode()
    {
        return clickMode;
    }

    private void normalClickSelect()
    {
        // Check if the mouse was clicked over a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Generate a ray that travels outward from the mouse click point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the generated ray hit something, that means it was clicked
            if (Physics.Raycast(ray, out hit))
            {
                //Get the hit object
                objectHit = hit.transform;

                //Assign last hit object if not assigned yet
                if (!lastHit)
                {
                    lastHit = objectHit;
                }
                else if (lastHit != objectHit)
                {
                    //If we hit a different object than the last, remove hightlight on last selected snippet
                    if (lastHit.GetComponent<SnippetState>())
                    {
                        lastHit.GetComponent<SnippetOutline>().setToggle(false);
                    }

                    //Assign new last hit object
                    lastHit = objectHit;
                }

                //If the current hit object is a Snippet
                if (objectHit.GetComponent<SnippetState>())
                {
                    objectHit.GetComponent<SnippetOutline>().setOutlineColor(255, 246, 0, 255);
                    objectHit.GetComponent<SnippetOutline>().setToggle(true);
                }

                //If user clicks
                if (Input.GetMouseButtonDown(0))
                {
                    //If the clicked object is a Snippet
                    if (objectHit.GetComponent<SnippetState>())
                    {
                        uiManager.loadSnippetDetails(objectHit.GetComponent<SnippetState>());
                        uiManager.showSnippetDetails();
                    }
                    else
                    {
                        uiManager.clearSnippetDetails();
                        uiManager.closeSnippetDetails();
                    }
                }
            }
            else
            {
                objectHit = null;

                //If we hit nothing, remove highlight on last selected snippet
                if (lastHit && lastHit.GetComponent<SnippetState>())
                {
                    lastHit.GetComponent<SnippetOutline>().setToggle(false);
                }

                //If user clicks nothing
                if (Input.GetMouseButtonDown(0))
                {
                    uiManager.clearSnippetDetails();
                    uiManager.closeSnippetDetails();
                }
            }

            updateDebugUI();
        }
    }

    private void yarnClickSelect()
    {
        // Check if the mouse was clicked over a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Generate a ray that travels outward from the mouse click point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the generated ray hit something, that means it was clicked
            if (Physics.Raycast(ray, out hit))
            {
                //Get the hit object
                objectHit = hit.transform;

                //Assign last hit object if not assigned yet
                if (!lastHit)
                {
                    lastHit = objectHit;
                }
                else if (lastHit != objectHit)
                {
                    //If we hit a different object than the last, remove hightlight on last selected snippet
                    if (lastHit.GetComponent<SnippetState>())
                    {
                        lastHit.GetComponent<SnippetOutline>().setToggle(false);
                    }

                    //Assign new last hit object
                    lastHit = objectHit;
                }

                //If the current hit object is a Snippet
                if (objectHit.GetComponent<SnippetState>())
                {
                    objectHit.GetComponent<SnippetOutline>().setOutlineColor(255, 0, 0, 255);
                    objectHit.GetComponent<SnippetOutline>().setToggle(true);
                }

                //If user clicks
                if (Input.GetMouseButtonDown(0))
                {
                    //If the clicked object is a Snippet
                    if (objectHit.GetComponent<SnippetState>())
                    {
                        if (uiManager.yarnManager.yarnEditor.getFrom() == -1)
                        {
                            uiManager.yarnManager.yarnEditor.setFrom(objectHit.GetComponent<SnippetState>().id);
                        }
                        else
                        {
                            uiManager.yarnManager.yarnEditor.setTo(objectHit.GetComponent<SnippetState>().id);
                        }
                    }
                }
            }
            else
            {
                objectHit = null;

                //If we hit nothing, remove highlight on last selected snippet
                if (lastHit && lastHit.GetComponent<SnippetState>())
                {
                    lastHit.GetComponent<SnippetOutline>().setToggle(false);
                }

                //If user clicks nothing
                if (Input.GetMouseButtonDown(0))
                {
                    uiManager.clearSnippetDetails();
                    uiManager.closeSnippetDetails();
                }
            }

            updateDebugUI();
        }
    }

    private void updateDebugUI()
    {
        if (lastHit)
        {
            uiManager.setDbgLastHitText(lastHit.name);
        }
        else
        {
            uiManager.setDbgLastHitText("None");
        }

        if (objectHit)
        {
            uiManager.setDbgObjectHitText(objectHit.name);
        }
        else
        {
            uiManager.setDbgObjectHitText("None");
        }

    }
}

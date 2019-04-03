using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class YarnLabelRowBehavior : MonoBehaviour
{
    public TMP_InputField yarnNameField;
    public Button contextButton;

    private YarnManager yarnManager;
    private int yarnID;

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

    public YarnManager YarnManager
    {
        get
        {
            return yarnManager;
        }

        set
        {
            yarnManager = value;
        }
    }

    void Start()
    {
        yarnNameField.readOnly = true;
        yarnID = -1;
    }

    public void contextAction()
    {
        if (yarnNameField.readOnly)
        {
            yarnNameField.readOnly = false;
            contextButton.GetComponentInChildren<Text>().text = "Save Change";
        }
        else
        {
            yarnNameField.readOnly = true;
            // Post the yarn name edit
        }

    }
}

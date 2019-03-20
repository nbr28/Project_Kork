using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Assets.Scripts.DataBase;

public class UIManager : MonoBehaviour
{
    //External Managers
    public SnippetManager snippetManager;
    public YarnManager yarnManager;

    //General UI
    public TMP_InputField tagSearchField;
    public TMP_Dropdown yarnSelectionDropdown;
    public Button applyFiltersButton;

    //Snippet UI
    public EasyTween dimmerPanel;
    public EasyTween snippetDetailsPanel;
    public TMP_InputField snippetTitleText;
    public TMP_InputField snippetContentText;
    public TMP_Dropdown snippetYarnDropdown;
    public TMP_Dropdown snippetTagDropdown;
    public Button closeSnippetButton;
    public Button editSnippetButton;

    //Editing Yarn and Tags
    public Button addYarnButton;
    public Button removeYarnButton;
    public Button addTagButton;
    public Button removeTagButton;

    //These are quite temporary (maybe)
    public Button yarnSelectModeButton;
    public Button quitYarnSelectModeButton;
    public Button saveYarnButton;
    public TMP_InputField yarnNameField;

    //Debug UI
    public GameObject dbgPanel;
    public TextMeshProUGUI dbgLastHitText;
    public TextMeshProUGUI dbgObjectHitText;
    public TextMeshProUGUI dbgYarnSelFromText;
    public TextMeshProUGUI dbgYarnSelToText;
    public TextMeshProUGUI dbgYarnTargetIDText;

    //When Snippet UI is up this will be the last
    private SnippetState LastClickedSnippet { get; set; }

    private bool snippetEditEnabled;

    /// <summary>
    /// Yarn Handler to get yarn names
    /// </summary>
    private YarnHandler _YarnHandler;
    /// <summary>
    /// Yarn line handler used to find what yarn lines a snippet is associated with
    /// </summary>
    private YarnLineHandler _YarnLineHandler;

    private List<int> yarnIds;

    void Awake()
    {
        this._YarnHandler = new YarnHandler();
        this._YarnLineHandler = new YarnLineHandler();
        //dbgPanel.SetActive(false);
        dimmerPanel.gameObject.SetActive(false);
        snippetDetailsPanel.gameObject.SetActive(false);
        disableSnippetEditing();
        snippetEditEnabled = false;

        yarnNameField.gameObject.SetActive(false);
        saveYarnButton.gameObject.SetActive(false);
    }

    void Update()
    {
        setDbgYarnFromText(yarnManager.yarnEditor.getFrom().ToString());
        setDbgYarnToText(yarnManager.yarnEditor.getTo().ToString());
        setDbgYarnTargetText(yarnManager.yarnEditor.getID().ToString());

        if (
            yarnManager.yarnEditor.getFrom() != -1 &&
            yarnManager.yarnEditor.getTo() != -1
            )
        {
            yarnNameField.gameObject.SetActive(true);
        }
        else
        {
            yarnNameField.text = "";
            yarnNameField.gameObject.SetActive(false);
        }

        if (yarnNameField.text != "")
        {
            saveYarnButton.gameObject.SetActive(true);
        }
        else
        {
            saveYarnButton.gameObject.SetActive(false);
        }
    }

    public void setSnippetTitleText(string title)
    {
        snippetTitleText.text = title;
    }

    public void setSnippetContentText(string title)
    {
        snippetContentText.text = title;
    }

    public void loadSnippetDetails(SnippetState clickedSnippet)
    {
        this.LastClickedSnippet = clickedSnippet;
        clearSnippetDetails();

        if (clickedSnippet)
        {
            setSnippetTitleText(clickedSnippet.title);
            setSnippetTagDropdown(clickedSnippet.tags);
            Debug.Log(clickedSnippet.tags.Count);
            setSnippetYarnDropdown(clickedSnippet.id);
            setSnippetContentText(clickedSnippet.contents);
        }
    }

    public void showSnippetDetails()
    {
        if (!snippetDetailsPanel.IsObjectOpened())
        {
            dimmerPanel.OpenCloseObjectAnimation();
            snippetDetailsPanel.OpenCloseObjectAnimation();
        }
    }

    public void closeSnippetDetails()
    {
        if (snippetDetailsPanel.IsObjectOpened())
        {
            dimmerPanel.OpenCloseObjectAnimation();
            snippetDetailsPanel.OpenCloseObjectAnimation();
            applyFilters();
        }
    }

    /// <summary>
    /// Displays the yarns that a snippet is associated with
    /// </summary>
    /// <param name="snippetID">The snippet id of the current snippet being displayed</param>
    public void setSnippetYarnDropdown(int snippetID)
    {

        YarnLine[] yarnLines = this._YarnLineHandler.GetRequestYarnsBySnippetId(snippetID);
        List<string> yarnList = new List<string>();
        yarnIds = new List<int>(5);

        foreach (YarnLine yarnLine in yarnLines)
        {
            if (!yarnIds.Contains(yarnLine.Yarn_Id))
            {
                yarnIds.Add(yarnLine.Yarn_Id);
                yarnList.Add(this._YarnHandler.GetRequestSingleYarnById(yarnLine.Yarn_Id).Yarn_Name);
            }
        }

        snippetYarnDropdown.AddOptions(yarnList);

        //// Go through the List of yarn line objects in the YarnManager
        //foreach (GameObject y in yarnManager.yarnLineObjectList)
        //{
        //    // Get the YarnLineRenderer component of each yarn line object
        //    YarnLineRenderer ylr = y.GetComponent<YarnLineRenderer>();

        //    // See if the provided snippet id is attached to the yarn line
        //    if ( ylr.isAttachedSnippet(snippetID) &&
        //            !yarnList.Contains(snippetID.ToString()) )
        //    {
        //        // If it is attached, we add it to the list of yarns
        //        yarnList.Add(snippetID.ToString());
        //        Debug.Log(yarnList);
        //    }
        //}

        //snippetYarnDropdown.AddOptions(yarnList);
    }

    public void setSnippetTagDropdown(List<string> tagList)
    {
        snippetTagDropdown.AddOptions(tagList);
    }

    public void clearSnippetYarnDropdownList()
    {
        snippetYarnDropdown.ClearOptions();
    }

    public void clearSnippetTagDropdownList()
    {
        snippetTagDropdown.ClearOptions();
    }

    public void addToExistingYarn()
    {
        if (yarnIds != null)
        {
            yarnManager.yarnEditor.setID(yarnIds[snippetYarnDropdown.value]);
            yarnManager.yarnEditor.setFrom(LastClickedSnippet.id);
            yarnManager.yarnEditor.lockFrom = true;
        }

        closeSnippetDetails();
        yarnManager.yarnEditor.enableYarnSelection();
    }

    public void addNewYarn()
    {

    }

    public void clearSnippetDetails()
    {
        setSnippetTitleText("");
        setSnippetContentText("");
        clearSnippetYarnDropdownList();
        clearSnippetTagDropdownList();
    }

    public void disableSnippetEditing()
    {
        snippetContentText.readOnly = true;
        snippetTitleText.readOnly = true;
        //addSnippetTagButton
        //addSnippetYarnButton
        editSnippetButton.GetComponentInChildren<Text>().text = "Edit Snippet";
    }

    public void enableSnippetEditing()
    {
        snippetContentText.readOnly = false;
        snippetTitleText.readOnly = false;
        //addSnippetTagButton
        //addSnippetYarnButton
        editSnippetButton.GetComponentInChildren<Text>().text = "Stop Editing";
    }

    public void toggleSnippetEditing()
    {
        if (snippetEditEnabled)
        {
            disableSnippetEditing();
            saveLocalSnippetChanges();
            snippetEditEnabled = false;
        }
        else
        {
            enableSnippetEditing();
            snippetEditEnabled = true;
        }
    }

    public void saveLocalSnippetChanges()
    {
        //Set the text to the modified text via the last clicked snippet 
        //TODO: better handling of LastClickedSnippet
        if (LastClickedSnippet != null)
        {
            this.LastClickedSnippet.contents = snippetContentText.text;
            this.LastClickedSnippet.title = snippetTitleText.text;

            // update the UI for snippets: header, contents
            this.snippetManager.snippetObjectDict[this.LastClickedSnippet.id].transform.Find("Header_Text").GetComponent<TMP_Text>().text = this.LastClickedSnippet.title;
            this.snippetManager.snippetObjectDict[this.LastClickedSnippet.id].transform.Find("UI_Text").GetComponent<TMP_Text>().text = this.LastClickedSnippet.contents;
        }
    }

    public void saveSnippetsToDB()
    {
        foreach (KeyValuePair<int, GameObject> snippetObj in snippetManager.snippetObjectDict)
        {
            SnippetState snippetObjState = snippetObj.Value.GetComponent<SnippetState>();
            snippetObjState.Save();
        }
    }

    public void setYarnSelectionDropDown(List<string> yarnList)
    {
        yarnSelectionDropdown.AddOptions(yarnList);
    }

    public void applyFilters()
    {
        string tagQuery = tagSearchField.text;
        /*
        int yarnQuery = -1;
        if (yarnSearchField.text != "")
        {
            yarnQuery = int.Parse(yarnSearchField.text);
        }
        */
        int yarnQuery = yarnSelectionDropdown.value;
        snippetManager.filterByTagAndYarn(tagQuery, yarnQuery);
    }

    public void setDbgLastHitText(string text)
    {
        if (dbgPanel.activeSelf)
        {
            dbgLastHitText.SetText("lastHit: " + text);
        }
    }

    public void setDbgObjectHitText(string text)
    {
        if (dbgPanel.activeSelf)
        {
            dbgObjectHitText.SetText("objectHit: " + text);
        }
    }

    public void setDbgYarnFromText(string text)
    {
        if (dbgPanel.activeSelf)
        {
            dbgYarnSelFromText.SetText("yarnSelFrom: " + text);
        }
    }

    public void setDbgYarnToText(string text)
    {
        if (dbgPanel.activeSelf)
        {
            dbgYarnSelToText.SetText("yarnSelTo: " + text);
        }
    }

    public void setDbgYarnTargetText(string text)
    {
        if (dbgPanel.activeSelf)
        {
            dbgYarnTargetIDText.SetText("yarnTargetID: " + text);
        }
    }

}

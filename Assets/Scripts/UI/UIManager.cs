using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Assets.Scripts.DataBase;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    // 
    //External Managers
    public SnippetManager snippetManager;
    public YarnManager yarnManager;
    public ClickManager clickManager;

    //General UI
    public TMP_InputField tagSearchField;
    public TMP_Dropdown yarnSelectionDropdown;
    public Button applyFiltersButton;
    public Button toggleDNDButton;

    //Panels
    public RectTransform searchPanel;
    public RectTransform contentPanel;
    public Button toggleSearchButton;
    public Button toggleContentButton;

    //Panel Selections
    public Button yarnSelectModeButton;
    public Button quitYarnSelectModeButton;
    public Button saveYarnButton;
    public TMP_InputField yarnNameField;
    public TMP_Dropdown yarnSelectionDropdownAdd;
    public Button newSnippetButton;

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
    public Button editYarnLabelsButton;
    public Button removeYarnButton;
    public Button addTagButton;
    public Button removeTagButton;

    //Debug UI
    public GameObject dbgPanel;
    public TextMeshProUGUI dbgLastHitText;
    public TextMeshProUGUI dbgObjectHitText;
    public TextMeshProUGUI dbgYarnSelFromText;
    public TextMeshProUGUI dbgYarnSelToText;
    public TextMeshProUGUI dbgYarnTargetIDText;

    //Toggle Flags for UI panels
    private bool searchTog;
    private bool contentTog;

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

        searchTog = false;
        contentTog = false;

        dimmerPanel.gameObject.SetActive(false);
        snippetDetailsPanel.gameObject.SetActive(false);
        disableSnippetEditing();
        snippetEditEnabled = false;

        yarnSelectionDropdownAdd.gameObject.SetActive(false);
        yarnNameField.gameObject.SetActive(false);
        saveYarnButton.gameObject.SetActive(false);
    }

    void Update()
    {
        setDbgYarnFromText(yarnManager.yarnEditor.getFrom().ToString());
        setDbgYarnToText(yarnManager.yarnEditor.getTo().ToString());
        setDbgYarnTargetText(yarnManager.yarnEditor.getID().ToString());

        if (yarnManager.yarnEditor.Mode == YarnEditor.mode.ADD)
        {
            if (
                yarnManager.yarnEditor.getFrom() != -1 &&
                yarnManager.yarnEditor.getTo() != -1
                )
            {
                yarnSelectionDropdownAdd.gameObject.SetActive(true);
                yarnNameField.gameObject.SetActive(true);
            }
            else
            {
                yarnSelectionDropdownAdd.gameObject.SetActive(false);
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
        else if (yarnManager.yarnEditor.Mode == YarnEditor.mode.DELETE)
        {
            yarnSelectionDropdownAdd.gameObject.SetActive(false);
            yarnNameField.text = "";
            yarnNameField.gameObject.SetActive(false);

            if (yarnManager.yarnEditor.getTo() != -1)
            {
                saveYarnButton.gameObject.SetActive(true);
            }
            else
            {
                saveYarnButton.gameObject.SetActive(false);
            }
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
            yarnManager.yarnEditor.Mode = YarnEditor.mode.ADD;
        }

        closeSnippetDetails();
        enableYarnSelection();
    }

    public void removeFromExistingYarn()
    {
        if (yarnIds != null)
        {
            yarnManager.yarnEditor.setID(yarnIds[snippetYarnDropdown.value]);
            yarnManager.yarnEditor.setFrom(LastClickedSnippet.id);
            yarnManager.yarnEditor.lockFrom = true;
            yarnManager.yarnEditor.Mode = YarnEditor.mode.DELETE;
        }

        closeSnippetDetails();
        enableYarnSelection();
    }

    public void removeTagFromSnippet()
    {
        snippetManager.getReferenceToSnippet(LastClickedSnippet.id).RemoveTag(snippetTagDropdown.value);
        loadSnippetDetails(LastClickedSnippet);
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

    public void deleteLastClickedSnippet()
    {
        closeSnippetDetails();

        List<YarnLine> attachedYarnLines = new List<YarnLine>(_YarnLineHandler.GetRequestYarnsBySnippetId(LastClickedSnippet.id));

        //Remove the yarn lines attached to snippet 
        foreach (YarnLine yl in attachedYarnLines)
        {
            yarnManager.YarnList.Remove(yl);
        }

        //Redraw all yarnlines
        yarnManager.redraw();

        //Remove actual snippet itself
        snippetManager.deleteSnippet(LastClickedSnippet);
        Destroy(snippetManager.snippetObjectDict[LastClickedSnippet.id]);
    }

    public void createNewSnippet()
    {
        snippetManager.createBlankSnippet();
    }

    public void enableYarnSelection()
    {
        clickManager.setClickMode(1);
    }

    public void quitYarnSelect()
    {
        yarnSelectionDropdownAdd.gameObject.SetActive(false);
        yarnNameField.text = "";
        yarnNameField.gameObject.SetActive(false);

        int from = yarnManager.yarnEditor.getFrom();
        int to = yarnManager.yarnEditor.getTo();

        if (snippetManager.snippetObjectDict.ContainsKey(from))
        {
            snippetManager.snippetObjectDict[from].GetComponent<SnippetOutline>().setToggle(false);
        }

        if (snippetManager.snippetObjectDict.ContainsKey(to))
        {
            snippetManager.snippetObjectDict[to].GetComponent<SnippetOutline>().setToggle(false);
        }

        yarnManager.yarnEditor.disableYarnSelection();
        clickManager.setClickMode(0);
    }

    public void fillYarnNameFieldFromDropdown()
    {
        yarnNameField.text = yarnSelectionDropdownAdd.options[yarnSelectionDropdownAdd.value].text;
    }

    public void applyYarnChanges()
    {
        int from = yarnManager.yarnEditor.getFrom();
        int to = yarnManager.yarnEditor.getTo();

        if (snippetManager.snippetObjectDict.ContainsKey(from))
        {
            snippetManager.snippetObjectDict[from].GetComponent<SnippetOutline>().setToggle(false);
        }

        if (snippetManager.snippetObjectDict.ContainsKey(to))
        {
            snippetManager.snippetObjectDict[to].GetComponent<SnippetOutline>().setToggle(false);
        }

        yarnManager.yarnAction();
        quitYarnSelect();
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
        yarnSelectionDropdown.ClearOptions();
        yarnSelectionDropdownAdd.ClearOptions();
        yarnSelectionDropdown.AddOptions(yarnList);
        yarnSelectionDropdownAdd.AddOptions(yarnList);
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

    public void clearYarnFilter()
    {
        string tagQuery = tagSearchField.text;
        snippetManager.filterByTagAndYarn(tagQuery, -1);
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

    public void toggleSearchPanel()
    {
        if (!searchTog)
        {
            searchPanel.DOAnchorPosX(-7.0f, 0.25f);
            searchTog = true;

            contentPanel.gameObject.SetActive(false);
            toggleDNDButton.gameObject.SetActive(false);
        }
        else
        {
            searchPanel.DOAnchorPosX(280.0f, 0.25f);
            searchTog = false;

            contentPanel.gameObject.SetActive(true);
            toggleDNDButton.gameObject.SetActive(true);
        }
    }

    public void openContentPanel()
    {
        if (!contentTog)
        {
            contentPanel.DOAnchorPosX(-7.0f, 0.25f);
            contentTog = true;

            searchPanel.gameObject.SetActive(false);
            toggleDNDButton.gameObject.SetActive(false);
        }
    }

    public void toggleContentPanel()
    {
        if (!contentTog)
        {
            contentPanel.DOAnchorPosX(-7.0f, 0.25f);
            contentTog = true;

            searchPanel.gameObject.SetActive(false);
            toggleDNDButton.gameObject.SetActive(false);
        }
        else
        {
            contentPanel.DOAnchorPosX(280.0f, 0.25f);
            contentTog = false;

            searchPanel.gameObject.SetActive(true);
            toggleDNDButton.gameObject.SetActive(true);
        }
    }
}

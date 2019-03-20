using Assets.Scripts.DataBase;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class YarnManager : MonoBehaviour, ISaveLoadInterface
{
    private List<YarnLine> yarnList;

    public SnippetManager snippetManager;   // Reference to SnippetManager (be sure to set through Unity)
    public UIManager uiManager;
    public YarnEditor yarnEditor;
    public List<GameObject> yarnLineObjectList;         // List of references to YarnLineRenderer objects

    // List of YarnLine objects, which will be populated with db query results upon iniitlization
    public List<YarnLine> YarnList//self building list of yarn lines
    {

        get
        {
            if(yarnList ==null)
            {
                YarnLineHandler yarnLineHandler = new YarnLineHandler();
                yarnList = new List<YarnLine>(yarnLineHandler.GetRequestAllYarnLines());
            }
         
            return yarnList;
        }
        set
        {
            yarnList = value;
        }
    }

    //dictonary of all the yarn lines used to build menues
    public Dictionary<int, string> allYarnLines
    {
        get
        {
            Dictionary<int, string>  tempDic = new Dictionary<int,string>();
            YarnHandler yarnHandler = new YarnHandler();
            foreach (Yarn yarn in yarnHandler.GetRequestAllYarn())
            {
                tempDic.Add(yarn.Yarn_Id, yarn.Yarn_Name);
            }
            return tempDic;
        }
    }

    // Use this for initialization
    void Start()
    {
        init();
    }

    // Instantiates a list of DrawLines objects, which are the renderers for the actual yarn line graphics
    void createYarnLines()
    {
        // For each yarnLine, we attempt to create a DrawLines object to render the graphic
        foreach (YarnLine yarnLine in YarnList)
        {
            // Store the endpoint IDs from current yarnLine
            int fromID = yarnLine.Snippet_Id_From;
            int toID = yarnLine.Snippet_Id_To;

            // Check if both of the endpoints exist in the snippet object dictionary in SnippetManager
            if (snippetManager.snippetObjectDict.ContainsKey(fromID) &&
                    snippetManager.snippetObjectDict.ContainsKey(toID))
            {
                // Create a new GameObject to hold the DrawLines as component
                GameObject lineObject = new GameObject();
                lineObject.AddComponent<YarnLineRenderer>();

                // Get the YarnLineRenderer component we just added
                YarnLineRenderer instYLR = lineObject.GetComponent<YarnLineRenderer>();

                if (instYLR)
                {
                    // Set the state of YarnLineRenderer
                    instYLR.YarnID = yarnLine.Yarn_Id;
                    instYLR.FromID = fromID;
                    instYLR.ToID = toID;

                    // Set the endpoints of the YarnLineRenderer
                    instYLR.A = snippetManager.snippetObjectDict[fromID].transform.position;
                    instYLR.B = snippetManager.snippetObjectDict[toID].transform.position;

                    // Give the object a name for identification
                    lineObject.name = "YarnLine ID: " + instYLR.YarnID;

                    // Add reference for newly instantiated object to list
                    yarnLineObjectList.Add(lineObject);
                }
            }
        }
    }

    void init()
    {
        createYarnLines();

        YarnHandler yarnHandler = new YarnHandler();

        //// Populate the unique name and unique id lists
        //foreach (GameObject y in yarnLineObjectList)
        //{
        //    // Get the renderer component
        //    YarnLineRenderer ylr = y.GetComponent<YarnLineRenderer>();
        //    int currentYarnID = ylr.YarnID;

        //    // Add yarn id to list, if it isn't already in
        //    if (!UniqueYarnIDList.Contains(ylr.YarnID))
        //    {
        //        UniqueYarnIDList.Add(ylr.YarnID);
        //        UniqueYarnNameList.Add(yarnHandler.GetRequestSingleYarnById(ylr.YarnID).Yarn_Name);//request the yarn names from the db
        //    }

        //}
        string[] temp = new string[allYarnLines.Count];
        allYarnLines.Values.CopyTo(temp, 0);
        uiManager.setYarnSelectionDropDown(new List<string>(temp));
    }

    // Given a yarn ID, returns a list of all snippet IDs involved
    public List<int> getAttachedSnippetsByYarnID(int yarnID)
    {
        List<int> ret = new List<int>();

        // Look through array of yarn line objects
        foreach (GameObject y in yarnLineObjectList)
        {
            // Get the renderer component
            YarnLineRenderer ylr = y.GetComponent<YarnLineRenderer>();

            // Check if the yarn ID matches specified ID
            if (ylr.YarnID == yarnID)
            {
                // Add both endpoint snippet IDs to list, if they aren't already added
                if (!ret.Contains(ylr.FromID))
                {
                    ret.Add(ylr.FromID);
                }

                if (!ret.Contains(ylr.ToID))
                {
                    ret.Add(ylr.ToID);
                }
            }

        }

        return ret;
    }

    // Automatically hides yarns if either of the attached snippets are inactive, and shows yarns if both snippets are active
    public void adaptiveHide()
    {
        foreach (GameObject y in yarnLineObjectList)
        {
            YarnLineRenderer ylr = y.GetComponent<YarnLineRenderer>();

            if (!snippetManager.snippetObjectDict[ylr.FromID].activeSelf ||
                    !snippetManager.snippetObjectDict[ylr.ToID].activeSelf)
            {
                y.SetActive(false);
            }
            else
            {
                y.SetActive(true);
            }
        }
    }

    public void hideAll()
    {
        foreach (GameObject y in yarnLineObjectList)
        {
            y.SetActive(false);
        }
    }

    public void showAll()
    {
        foreach (GameObject y in yarnLineObjectList)
        {
            y.SetActive(true);
        }
    }

    // Shows only the yarns associated with the provided ID
    public void filterByID(int query = -1)
    {
        if (query == -1)
        {
            showAll();
        }
        else
        {
            foreach (GameObject y in yarnLineObjectList)
            {
                YarnLineRenderer ylr = y.GetComponent<YarnLineRenderer>();

                if (ylr && ylr.YarnID == query)
                {
                    y.SetActive(true);
                }
                else
                {
                    y.SetActive(false);
                }
            }
        }
    }

    public void saveYarn()
    {
        YarnHandler yarnHandler = new YarnHandler();
        Yarn newYarn = new Yarn();
        newYarn.Yarn_Name = uiManager.yarnNameField.text;

        //If we're adding a new yarn
        if (yarnEditor.getID() == -1)
        {
            newYarn = yarnHandler.Post(newYarn);
        }
        //If we're adding to an existing yarn group
        else
        {
            newYarn.Yarn_Id = yarnEditor.getID();
        }

        YarnLineHandler yarnLineHandler = new YarnLineHandler();
        yarnLineHandler.Post(new YarnLine(yarnEditor.getTo(), yarnEditor.getFrom(), newYarn.Yarn_Id));

        init();
    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateObject()
    {
        throw new System.NotImplementedException();
    }

    public void Delete()
    {
        throw new System.NotImplementedException();
    }
}

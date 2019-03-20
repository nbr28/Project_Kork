using Assets.Scripts.DataBase;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class YarnManager : MonoBehaviour, ISaveLoadInterface {

    public SnippetManager snippetManager;   // Reference to SnippetManager (be sure to set through Unity)
    public UIManager uiManager;

    public YarnEditor yarnEditor;

    public List<YarnLineRenderer> yarnLineObjectList;         // List of references to YarnLineRenderer objects
    private List<YarnLine> yarnList;                    // List of YarnLine objects, which will be populated with db query results
    //TODO: @Jerry from @Natan why do we have both a list of yarn lines and another list of each component?
    private List<int> uniqueYarnIDList;                 // List of unique yarn IDs
    private List<string> uniqueYarnNameList;            // List of unique yarn names

    public List<int> UniqueYarnIDList
    {
        get
        {
            return uniqueYarnIDList;
        }

        private set
        {
            uniqueYarnIDList = value;
        }
    }

    public List<string> UniqueYarnNameList
    {
        get
        {
            return uniqueYarnNameList;
        }

        private set
        {
            uniqueYarnNameList = value;
        }
    }

    //private Dictionary<int, string> uniqueYarnIDList;   // Dictionary of unique yarns, with their ids as the key and names as the value

    void Awake()
    {
        loadYarnLineData(); // yarnList will be populated with YarnLine objects from DB
    }

    // Use this for initialization
    void Start () {
        init();
    }

    void Update()
    {
        for (int i = 0; i < yarnLineObjectList.Count; i++)
        {
            yarnLineObjectList[i].A = snippetManager.snippetObjectDict[yarnList[i].Snippet_Id_From].gameObject.transform.position;
            yarnLineObjectList[i].B = snippetManager.snippetObjectDict[yarnList[i].Snippet_Id_To].gameObject.transform.position;
        }
    }

    // Utilizes YarnLineHandler class to perform a get call to the database and retrieve a list of YarnLine objects
    void loadYarnLineData()
    {
        YarnLineHandler yarnLineHandler = new YarnLineHandler();

        this.yarnList = new List<YarnLine>(yarnLineHandler.GetRequestAllYarnLines());
    }

    // Instantiates a list of DrawLines objects, which are the renderers for the actual yarn line graphics
    void createYarnLines()
    {
        // For each yarnLine, we attempt to create a DrawLines object to render the graphic
        foreach (YarnLine yarnLine in yarnList)
        {
            // Store the endpoint IDs from current yarnLine
            int fromID = yarnLine.Snippet_Id_From;
            int toID = yarnLine.Snippet_Id_To;

            // Check if both of the endpoints exist in the snippet object dictionary in SnippetManager
            if ( snippetManager.snippetObjectDict.ContainsKey(fromID) &&
                    snippetManager.snippetObjectDict.ContainsKey(toID) )
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
                    yarnLineObjectList.Add(instYLR);
                }
            }
        }
    }

    void init()
    {
        createYarnLines();

        UniqueYarnIDList = new List<int>();
        UniqueYarnNameList = new List<string>();

        UniqueYarnIDList.Add(-1);
        UniqueYarnNameList.Add("---");

        YarnHandler yarnHandler = new YarnHandler();

        // Populate the unique name and unique id lists
        foreach (YarnLineRenderer ylr in yarnLineObjectList)
        {
            // Get the renderer component
            int currentYarnID = ylr.YarnID;

            // Add yarn id to list, if it isn't already in
            if (!UniqueYarnIDList.Contains(ylr.YarnID))
            {
                UniqueYarnIDList.Add(ylr.YarnID);
                UniqueYarnNameList.Add(yarnHandler.GetRequestSingleYarnById(ylr.YarnID).Yarn_Name);//request the yarn names from the db
            }

        }

        uiManager.setYarnSelectionDropDown(UniqueYarnNameList);
    }

    // Given a yarn ID, returns a list of all snippet IDs involved
    public List<int> getAttachedSnippetsByYarnID(int yarnID)
    {
        List<int> ret = new List<int>();

        // Look through array of yarn line objects
        foreach (YarnLineRenderer ylr in yarnLineObjectList)
        {
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
        foreach (YarnLineRenderer ylr in yarnLineObjectList)
        {
            if ( !snippetManager.snippetObjectDict[ylr.FromID].activeSelf ||
                    !snippetManager.snippetObjectDict[ylr.ToID].activeSelf)
            {
                ylr.gameObject.SetActive(false);
            }
            else
            {
                ylr.gameObject.SetActive(true);
            }
        }
    }

    public void hideAll()
    {
        foreach (YarnLineRenderer ylr in yarnLineObjectList)
        {
            ylr.gameObject.SetActive(false);
        }
    }

    public void showAll()
    {
        foreach (YarnLineRenderer ylr in yarnLineObjectList)
        {
            ylr.gameObject.SetActive(true);
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
            foreach (YarnLineRenderer ylr in yarnLineObjectList)
            {
                if (ylr && ylr.YarnID == query)
                {
                    ylr.gameObject.SetActive(true);
                }
                else
                {
                    ylr.gameObject.SetActive(false);
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

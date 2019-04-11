using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using Assets.Scripts.DataBase;
using System;
using System.Linq;

public class SnippetManager : MonoBehaviour
{

    //Set the prefab in Editor!!
    public GameObject snippetPrefab;
    public YarnManager yarnManager;

    public Dictionary<int, GameObject> snippetObjectDict;   // Dictionary of snippet GameObjects with its SnippetState ID as the key   

    private string currentTagFilter;
    private int currentYarnFilter;

    //private List<SnippetState> snippetList = new List<SnippetState>();                      //Array of snippets (necessary interface for de-serialization from JSON)

    private void Awake()
    {
        createSnippets();
        currentTagFilter = "";
        currentYarnFilter = -1;
    }

    private void createSnippets()
    {
        SnippetHandler snippetHandler = new SnippetHandler();
        TagHandler tagHandler = new TagHandler(this);
        AssocationHandler assocationHandler = new AssocationHandler();
        Assets.Scripts.DataBase.Snippet[] snippetArr = snippetHandler.GetAllSnippets();

        snippetObjectDict = new Dictionary<int, GameObject>();

        foreach (Assets.Scripts.DataBase.Snippet snippet in snippetArr)
        {
            GameObject instantiatedSnippet = Instantiate(snippetPrefab) as GameObject;
            //instantiatedSnippet.tag = "group" + Random.Range(1, 2); //Temporary hack measure, randomly assign snippet to one of two groups

            SnippetState instSnippetState = instantiatedSnippet.GetComponent<SnippetState>();
            instSnippetState.loadState( tagHandler.GetTagBySnippetId(snippet.Snippet_Id),
                                        snippet,
                                        assocationHandler.GetAssociationViewForSnippet(snippet.Snippet_Id));

            instantiatedSnippet.name = instSnippetState.title;  //names each snippet by its title

            snippetObjectDict.Add(instSnippetState.id, instantiatedSnippet);
        }

    }

    public SnippetState getReferenceToSnippet(int snippet_Id)
    {
        return this.snippetObjectDict[snippet_Id].GetComponent<SnippetState>();
    }


    public void showAllSnippets()
    {
        foreach (KeyValuePair<int, GameObject> snippetObject in snippetObjectDict)
        {
            snippetObject.Value.SetActive(true);
        }

        yarnManager.showAll();
    }

    public void hideAllSnippets()
    {
        foreach (KeyValuePair<int, GameObject> snippetObject in snippetObjectDict)
        {
            snippetObject.Value.SetActive(false);
        }

        yarnManager.hideAll();
    }

    public void filterByTagAndYarn(string tagQuery, int yarnQuery)
    {
        int yarnQueryID = -1;
        //Get the actual yarnID from the dropdown seletion value
        if (yarnQuery >= 0)
        {
            yarnQueryID = yarnManager.allYarnLines.Keys.ElementAt<int>(yarnQuery);
        }
         

        //If both filters exist
        if (tagQuery != "" && yarnQueryID >= 0)
        {
            Debug.Log("Searching tag: " + tagQuery + "and yarn: " + yarnQueryID);

            //First hide all snippets
            hideAllSnippets();

            //Apply both filters:

            //Get all the snippetIDs attached to the given yarnID
            List<int> snippetsToShow = yarnManager.getAttachedSnippetsByYarnID(yarnQueryID);

            //Search the list of snippet objects 
            foreach (KeyValuePair<int, GameObject> snippetObject in snippetObjectDict)
            {
                //Get the SnippetState component
                SnippetState snippetState = snippetObject.Value.GetComponent<SnippetState>();

                //If the snippet matches both filters, show it
                if (snippetState && snippetState.tags.Contains(tagQuery) && snippetsToShow.Contains(snippetState.id))
                {
                    snippetObject.Value.SetActive(true);
                }
            }

            yarnManager.adaptiveHide();
        }
        //If only tag query
        else if (tagQuery != "" && yarnQueryID < 0)
        {
            Debug.Log("Searching tag: " + tagQuery);

            //First hide all snippets
            hideAllSnippets();

            //Search the list of snippet objects 
            foreach (KeyValuePair<int, GameObject> snippetObject in snippetObjectDict)
            {
                //Get the SnippetState component
                SnippetState snippetState = snippetObject.Value.GetComponent<SnippetState>();

                //If the snippet contains the tag, show it
                if (snippetState && snippetState.tags.Contains(tagQuery))
                {
                    snippetObject.Value.SetActive(true);
                }
            }

            yarnManager.adaptiveHide();
        }
        //If only yarn query
        else if (tagQuery == "" && yarnQueryID >= 0)
        {
            Debug.Log("Searching yarn: " + yarnQueryID);

            //First hide all snippets
            hideAllSnippets();

            //Get all the snippetIDs attached to the given yarnID
            List<int> snippetsToShow = yarnManager.getAttachedSnippetsByYarnID(yarnQueryID);

            //Search the list of snippet objects 
            foreach (KeyValuePair<int, GameObject> snippetObject in snippetObjectDict)
            {
                //Get the SnippetState component
                SnippetState snippetState = snippetObject.Value.GetComponent<SnippetState>();

                //If the snippet is in the list, show it
                if (snippetState && snippetsToShow.Contains(snippetState.id))
                {
                    snippetObject.Value.SetActive(true);
                }
            }

            yarnManager.adaptiveHide();
        }
        //The filters are empty, so show everything
        else
        {
            showAllSnippets();
            yarnManager.showAll();
        }
        //Set the current filter
        currentTagFilter = tagQuery;
        currentYarnFilter = yarnQueryID;
    }

    public void deleteSnippet(SnippetState target)
    {
        SnippetHandler snippetHandler = new SnippetHandler();
        snippetHandler.Delete(target.GetBaseInterFace());
    }

    public void createBlankSnippet()
    {
        TagHandler tagHandler = new TagHandler(this);
        AssocationHandler assocationHandler = new AssocationHandler();

        Snippet blankSnippet = SnippetState.CreateBlankSnippet(1);

        GameObject instantiatedSnippet = Instantiate(snippetPrefab) as GameObject;

        SnippetState instSnippetState = instantiatedSnippet.GetComponent<SnippetState>();
        instSnippetState.loadState(tagHandler.GetTagBySnippetId(blankSnippet.Snippet_Id),
                                    blankSnippet,
                                    assocationHandler.GetAssociationViewForSnippet(blankSnippet.Snippet_Id));

        instantiatedSnippet.name = instSnippetState.title;  //names each snippet by its title

        snippetObjectDict.Add(instSnippetState.id, instantiatedSnippet);
    }

    //DEPRECATED!!
    //Loads JSON data from DB file into application
    //Returns whether or not data was successfully loaded
    [Obsolete("Please use create snippets to load the snippet data from the db")]
     private bool loadSnippetData()
    {
        //Get the full path of the data file from within StreamingAssets folder
        //string filepath = Path.Combine(Application.dataPath + "/Scripts/Data", snippetDataFile);

        ////Check if file exists first before loading data
        //if (File.Exists(filepath))
        //{
        //    //Store the JSON data as string literal
        //    string dataAsJSON = File.ReadAllText(filepath);

        //    //Deserialize the JSON data into a temporary GameObject
        //    snippetList = JsonHelper.getJsonArray<Snippet>(dataAsJSON);
        //}

        return true;
    }
}
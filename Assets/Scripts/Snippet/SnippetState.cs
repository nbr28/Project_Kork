using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AssociationsName;
using Assets.Scripts.DataBase;
using System;
using Association = AssociationsName.Association;
using Assets.Scripts.DataBase.Interfaces;

public class SnippetState : MonoBehaviour, ISaveLoadInterface, IBaseConverter<Snippet>
{

    //These strings are used to load data from the associations table. If you are adding a default string to the db please add one here 
    private static string m_X_associationName = "x";
    private static string m_Y_associationName = "y";
    private static string m_Z_associationName = "z";
    private static string m_Connection_Data_AssociationName = "Connection_Data";
    private static string m_title_AssociationName = "title";
    private static string m_content_AssociationName = "content";
    private static string m_titleBarColor_AssociationName = "titleBarColor";

    public RectTransform titleBar;
    public RectTransform contentPane;
    public RectTransform tagBar;

    public int id;
    public string title;
    public string contents;
    public string Path_To_Data;
    public List<string> tags= new List<string>();
    public float x, y, z;
    public string titleBarColor;
    public Dictionary<Association, string> assocations = new Dictionary<Association, string>();




    /// <summary>
    /// Used when loaded from db to trigger rendering snippet
    /// </summary>
    public void loadState(Tag[] tags, Assets.Scripts.DataBase.Snippet snippet, AssociationView[] associationViews)
    {

        //Add the snippet Information
        this.Path_To_Data = snippet.Path_To_Data;
        this.id = snippet.Snippet_Id;

        //Add all the tags
        foreach (Tag tag in tags)
            this.tags.Add(tag.Tag_Name);

        foreach (AssociationView associationView in associationViews)
        {
            //default loaded associations
            if (associationView.Association_Name == m_X_associationName)
                this.x = float.Parse(associationView.Connection_Data);
            else if (associationView.Association_Name == m_Y_associationName)
                this.y = float.Parse(associationView.Connection_Data);
            else if (associationView.Association_Name == m_Z_associationName)
                this.z = float.Parse(associationView.Connection_Data);
            else if (associationView.Association_Name == m_content_AssociationName)
                this.contents = associationView.Connection_Data;
            else if (associationView.Association_Name == m_titleBarColor_AssociationName)
                this.titleBarColor = associationView.Connection_Data;
            else if (associationView.Association_Name == m_title_AssociationName)
                this.title = associationView.Connection_Data;
            else
            {
                //If they are not default loaded add them to the object as regular associations
                this.assocations.Add(new Association(associationView.Association_Name, associationView.Association_Data_Type,this.id), associationView.Connection_Data);
            }
        }



        transform.position = new Vector3(x*2, y*2, Math.Abs(z*2));
        titleBar.GetComponent<TextMeshPro>().SetText(title);
        contentPane.GetComponent<TextMeshPro>().SetText(contents);

        string allTags = "";
        foreach (string tag in this.tags)
        {
            allTags += (" #" + tag);
        }
        allTags.Trim();
        tagBar.GetComponent<TextMeshPro>().SetText(allTags);
    }

    public void fadeOut()
    {
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = 0.3f;
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    /// <summary>
    /// Does not save tags
    /// TODO: Add tag Saving 
    /// 
    /// </summary>
    public void Save()
    {
        ConnectionHandler connectionHandler = new ConnectionHandler(this);//create saver
        //foreach(KeyValuePair<Association,string> association in assocations)
        //{
        //    Connection tempConnection = new Connection
        //    {
        //        Assoication_Id = association.Key.Association_Id,
        //        Snippet_Id = this.id,
        //        data = association.Value
        //    };

        //    connectionHandler.PostConnection(tempConnection);//save 
        //}

        Connection conTitle = new Connection
        {
            Association_Id = 78,
            Snippet_Id=this.id,
            data=this.title
        };

        Connection conContent = new Connection
        {
            Association_Id = 79,
            Snippet_Id = this.id,
            data = this.contents
        };

        connectionHandler.PutConnection(conTitle);
        connectionHandler.PutConnection(conContent);

    }

    public void Load()
    {
        throw new NotImplementedException();
    }

    public void UpdateObject()
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public Snippet GetBaseInterFace()
    {
        return new Snippet(this.id,this.Path_To_Data);
    }
}

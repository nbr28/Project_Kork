using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AssociationsName;
using Assets.Scripts.DataBase;
using System;
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



    public string title
    {
        get
        {
            Debug.Log(m_title_AssociationName);
            return assocations[new AssociationState(m_title_AssociationName, DataType.String)];
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_title_AssociationName, DataType.String)))
            {
                assocations[new AssociationState(m_title_AssociationName, DataType.String)] = value;
            }
            else
            {
                assocations.Add(new AssociationState(m_title_AssociationName, DataType.String),value);
            }
        }
    }

    public string contents
    {
        get
        {
            return assocations[new AssociationState(m_content_AssociationName, DataType.String)];
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_content_AssociationName, DataType.String)))
            {
                assocations[new AssociationState(m_content_AssociationName, DataType.String)] = value;
            }
            else
            {
                assocations.Add(new AssociationState(m_content_AssociationName, DataType.String), value);
            }
        }
    }

 

    public float x
    {
        get
        {
            return float.Parse(assocations[new AssociationState(m_X_associationName, DataType.Float)]);
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_X_associationName, DataType.Float)))
            {
                assocations[new AssociationState(m_X_associationName, DataType.Float)] = value.ToString();
            }
            else
            {
                assocations.Add(new AssociationState(m_X_associationName, DataType.Float), value.ToString());
            }
        }
    }

    public float y
    {
        get
        {
            return float.Parse(assocations[new AssociationState(m_Y_associationName, DataType.Float)]);
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_Y_associationName, DataType.Float)))
            {
                assocations[new AssociationState(m_Y_associationName, DataType.Float)] = value.ToString();
            }
            else
            {
                assocations.Add(new AssociationState(m_Y_associationName, DataType.Float), value.ToString());
            }
        }
    }

    public float z
    {
        get
        {
            return float.Parse(assocations[new AssociationState(m_Z_associationName, DataType.Float)]);
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_Z_associationName, DataType.Float)))
            {
                assocations[new AssociationState(m_Z_associationName, DataType.Float)] = value.ToString();
            }
            else
            {
                assocations.Add(new AssociationState(m_Z_associationName, DataType.Float), value.ToString());
            }
        }
    }


    public string titleBarColor
    {
        get
        {
            return assocations[new AssociationState(m_titleBarColor_AssociationName, DataType.String)];
        }
        set
        {
            if (assocations.ContainsKey(new AssociationState(m_titleBarColor_AssociationName, DataType.String)))
            {
                assocations[new AssociationState(m_titleBarColor_AssociationName, DataType.String)] = value;
            }
            else
            {
                assocations.Add(new AssociationState(m_titleBarColor_AssociationName, DataType.String), value);
            }
        }
    }

    public int id;
    public string Path_To_Data;

    public List<string> tags = new List<string>();
    public Dictionary<AssociationState, string> assocations = new Dictionary<AssociationState, string>();




    /// <summary>
    /// Used when loaded from db to trigger rendering snippet
    /// </summary>
    public void loadState(Tag[] tags, Snippet snippet, AssociationView[] associationViews)
    {

        //Add the snippet Information
        this.Path_To_Data = snippet.Path_To_Data;
        this.id = snippet.Snippet_Id;

        //Add all the tags
        foreach (Tag tag in tags)
            this.tags.Add(tag.Tag_Name);

        foreach (AssociationView associationView in associationViews)
        {
            this.assocations.Add(associationView.GetAssociationKeyPair().Key,associationView.GetAssociationKeyPair().Value);
        }



        transform.position = new Vector3(x, y, z);
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
    /// Saves the entr
    /// </summary>
    public void Save()
    {
        ConnectionHandler connectionHandler = new ConnectionHandler();
        foreach(KeyValuePair<AssociationState, string> connetion in assocations)
        {
            connectionHandler.Put(new Connection(this.id, connetion.Key.Association_Id, connetion.Value));
        }
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
        return new Snippet(this.id, this.Path_To_Data);
    }

    public static Snippet CreateBlankSnippet(int board_id)
    {
        SnippetHandler snippetHandler = new SnippetHandler();

        //GUID is a unique code that is a placeholder for now
        Snippet snippet = snippetHandler.Post(new Snippet(-1, board_id, Guid.NewGuid().ToString()));//get the snippets id


        //create all the required fields
        Connection x = new Connection(snippet.Snippet_Id, 80, 0.ToString());
        Connection y = new Connection(snippet.Snippet_Id, 81, 0.ToString());
        Connection z = new Connection(snippet.Snippet_Id, 82, 0.ToString());

        Connection titleBar = new Connection(snippet.Snippet_Id, 83, "#0000ff");
        Connection content = new Connection(snippet.Snippet_Id, 79, "Fill out text");
        Connection title = new Connection(snippet.Snippet_Id, 78, "Placeholder Title");

        ConnectionHandler connectionHandler = new ConnectionHandler();

        connectionHandler.Post(x);
        connectionHandler.Post(y);
        connectionHandler.Post(z);
        connectionHandler.Post(titleBar);
        connectionHandler.Post(content);
        connectionHandler.Post(title);

        return snippet;
    }
}

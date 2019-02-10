﻿using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to serialize and deserialize data from DB.
/// Each class (except URLConfig) represents a table from DB.
/// </summary>
namespace Assets.Scripts.DataBase
{
    public static class URLConfig
    {
        public const string BASEURL = "https://porject-kork.herokuapp.com/api/";
       // public const string BASEURL = "http://localhost:3000/api/";

    }
    
    [System.Serializable]
    public class Tag
    {
        public int Snippet_Id;
        public string Tag_Name;
    }

    [System.Serializable]
    public class Snippet
    {
        public int Snippet_Id;
        public string Path_To_Data;
    }

    [System.Serializable]
    public class Connection
    {
        public int Snippet_Id;
        public int Association_Id;
        public string data;
    }

    [System.Serializable]
    public class Association
    {
        public int Association_Id;
        public AssociationsName.Association.DataType Association_Data_Type;
        public string Association_Name;

        public override string ToString()
        {
            return Association_Name + ", " + Association_Data_Type + ", " + Association_Id;
        }
    }

    [System.Serializable]
    public class Yarn
    {
        public string Yarn_Name;
        public int Yarn_Id;

        public override string ToString()
        {
            return Yarn_Name + ", " + Yarn_Id ;
        }
    }


    [System.Serializable]
    public class YarnLine
    {
        public int Snippet_Id_To;
        public int Snippet_Id_From;
        public int Yarn_Id=-1;
    }


    [System.Serializable]
    public class AssociationView
    {
        public int Association_Id;
        public AssociationsName.Association.DataType Association_Data_Type;
        public string Association_Name;
        public int Snippet_Id;
        public string Connection_Data;
    }
}
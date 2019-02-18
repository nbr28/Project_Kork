using Assets.Scripts.DataBase.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Used to serialize and deserialize data from DB.
/// Each class (except URLConfig) represents a table from DB.
/// </summary>
namespace Assets.Scripts.DataBase
{

    public static class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    public static class URLConfig
    {
        public const string BASEURL = "https://porject-kork.herokuapp.com/api/";
        // public const string BASEURL = "http://localhost:3000/api/";
        public static int TempBoardId = 1;
    }

    [System.Serializable]
    public class Board
    {
        public int Board_Id;
        public string Board_Name;
    }
    
    [System.Serializable]
    public class Tag
    {
        public int Snippet_Id;
        public string Tag_Name;
    }

    [System.Serializable]
    public class Snippet : ICreateFormData
    {
        public int Snippet_Id;
        public string Path_To_Data;
        public int Board_Id;

        public Snippet(int snippet_Id, string path_To_Data)
        {
            Snippet_Id = snippet_Id;
            Path_To_Data = path_To_Data;
            Board_Id =URLConfig.TempBoardId;
        }

        public WWWForm CreateForm()
        {
            WWWForm form = new WWWForm();
            form.AddField("Snippet_Id", this.Snippet_Id);
            form.AddField("Path_To_Data", this.Path_To_Data);
            form.AddField("Board_Id", this.Board_Id);
            return form;
        }

        List<IMultipartFormSection> ICreateFormData.CreateForm()
        {

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Snippet_Id",this.Snippet_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Path_To_Data", this.Path_To_Data));
            formData.Add(new MultipartFormDataSection("Board_Id", this.Board_Id.ToString()));
            return formData;
        }
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
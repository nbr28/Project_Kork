﻿using Assets.Scripts.DataBase.Interfaces;
using AssociationsName;
using System;
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
        //public const string BASEURL = "http://localhost:3000/api/";
        public static int TempBoardId = 1;
    }

    [System.Serializable]
    public class Board : ICreateFormData
    {
        public int Board_Id=-1;
        public string Board_Name;

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Board_Id", this.Board_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Board_Name", this.Board_Name));
            return formData;
        }
    }
    
    [System.Serializable]
    public class Tag : ICreateFormData
    {
        public int Snippet_Id=-1;
        public string Tag_Name;

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Snippet_Id", this.Snippet_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Tag_Name", this.Tag_Name));
            return formData;
        }
    }

    [System.Serializable]
    public class Snippet : ICreateFormData
    {
        public int Snippet_Id=-1;
        public string Path_To_Data;
        public int Board_Id=-1;

        public Snippet(int snippet_Id, string path_To_Data)
        {
            Snippet_Id = snippet_Id;
            Path_To_Data = path_To_Data;
            Board_Id =URLConfig.TempBoardId;
        }

        [Obsolete]
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
    public class Connection : ICreateFormData
    {
        public int Snippet_Id=-1;
        public int Association_Id=-1;
        public string Connection_Data;

        public Connection(int snippet_Id, int association_Id, string data)
        {
            Snippet_Id = snippet_Id;
            Association_Id = association_Id;
            this.Connection_Data = data;
        }

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Snippet_Id", this.Snippet_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Association_Id", this.Association_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Connection_Data", this.Connection_Data));
            return formData;
        }
    }

    [System.Serializable]
    public class Association : ICreateFormData
    {
        public int Association_Id=-1;
        public AssociationsName.DataType Association_Data_Type;
        public string Association_Name;

        public Association(int association_Id, DataType association_Data_Type, string association_Name)
        {
            Association_Id = association_Id;
            Association_Data_Type = association_Data_Type;
            Association_Name = association_Name;
        }

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Association_Id", this.Association_Id.ToString()));
            formData.Add(new MultipartFormDataSection("Association_Data_Type", ((int)this.Association_Data_Type).ToString()));
            formData.Add(new MultipartFormDataSection("Association_Name", this.Association_Name));
            return formData;
        }

        public override string ToString()
        {
            return Association_Name + ", " + Association_Data_Type + ", " + Association_Id;
        }
    }

    [System.Serializable]
    public class Yarn : ICreateFormData
    {
        public string Yarn_Name;
        public int Yarn_Id=-1;

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Yarn_Name", this.Yarn_Name));
            formData.Add(new MultipartFormDataSection("Yarn_Id", this.Yarn_Id.ToString()));
            return formData;
        }

        public override string ToString()
        {
            return Yarn_Name + ", " + Yarn_Id ;
        }
    }


    [System.Serializable]
    public class YarnLine : ICreateFormData
    {
        public int Snippet_Id_To=-1;
        public int Snippet_Id_From=-1;
        public int Yarn_Id=-1;

        public YarnLine() { }

        public YarnLine(int snippet_Id_To, int snippet_Id_From, int yarn_Id)
        {
            Snippet_Id_To = snippet_Id_To;
            Snippet_Id_From = snippet_Id_From;
            Yarn_Id = yarn_Id;
        }

        public List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("Snippet_Id_To", this.Snippet_Id_To.ToString()));
            formData.Add(new MultipartFormDataSection("Snippet_Id_From", this.Snippet_Id_From.ToString()));
            formData.Add(new MultipartFormDataSection("Yarn_Id", this.Yarn_Id.ToString()));
            return formData;
        }

        public override bool Equals(object obj)
        {
            var line = obj as YarnLine;
            return line != null &&
                  ( Snippet_Id_To == line.Snippet_Id_To && Snippet_Id_From == line.Snippet_Id_From ||
                    Snippet_Id_To == line.Snippet_Id_From && Snippet_Id_From == line.Snippet_Id_To)
                   &&
                   Yarn_Id == line.Yarn_Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -1258304170;
            hashCode = hashCode * -1521134295 + Snippet_Id_To.GetHashCode()+ Snippet_Id_From.GetHashCode();
            hashCode = hashCode * -1521134295 + Yarn_Id.GetHashCode();
            return hashCode;
        }
    }

    public class YarnLineUpdate : YarnLine
    {
        public int Old_Snippet_Id_To = -1;
        public int Old_Snippet_Id_From = -1;

        public YarnLineUpdate(int old_Snippet_Id_To, int old_Snippet_Id_From, int snippet_Id_To, int snippet_Id_From, int yarn_Id) : base(snippet_Id_To, snippet_Id_From, yarn_Id)
        {
            Old_Snippet_Id_To = old_Snippet_Id_To;
            Old_Snippet_Id_From = old_Snippet_Id_From;
        }

        public new List<IMultipartFormSection> CreateForm()
        {
            List<IMultipartFormSection> formData = base.CreateForm();
            formData.Add(new MultipartFormDataSection("Old_Snippet_Id_To", this.Old_Snippet_Id_To.ToString()));
            formData.Add(new MultipartFormDataSection("Old_Snippet_Id_From", this.Old_Snippet_Id_From.ToString()));
            return formData;
        }
    }


    [System.Serializable]
    public class AssociationView : ICreateFormData
    {
        public int Association_Id=-1;
        public AssociationsName.DataType Association_Data_Type;
        public string Association_Name;
        public int Snippet_Id=-1;
        public string Connection_Data;

        public List<IMultipartFormSection> CreateForm()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<AssociationState, string> GetAssociationKeyPair()
        {
           return new KeyValuePair<AssociationState, string>(new AssociationState(this.Association_Name, this.Association_Data_Type, this.Association_Id),this.Connection_Data);
             
        }
    }
}
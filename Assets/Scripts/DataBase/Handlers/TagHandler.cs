using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Proyecto26;
using UnityEngine.Networking;
using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.DataBase
{
    public class TagHandler : GenericCRUD<Tag>
    {
        private RequestHelper currentRequest;
        private MonoBehaviour mono;
        private Tag[] tags;
        private Tag tag;
        private const string baseTable = "tag";

        /// <summary>
        /// Constructor for TagHandler class.
        /// </summary>
        /// <param name="mono">a script with monobehavior that will be attached to this script</param>
        public TagHandler(MonoBehaviour mono)
        {
            this.mono = mono;
        }


        /// <summary>
        /// Gets all tags in the database
        /// </summary>
        /// <returns> All tags in the database</returns>
        public Tag[] GetAllTags()
        {
            return JsonHelper.getJsonArray<Tag>(base.Get(URLConfig.BASEURL + baseTable + 's'));
        }



        /// <summary>
        /// Getter for single tag with given snippet id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <returns>single tag</returns>
        public Tag[] GetTagBySnippetId(int Snippet_Id)
        {
            return JsonHelper.getJsonArray<Tag>(base.Get(URLConfig.BASEURL + baseTable + "/SnippetId/" + Snippet_Id));
        }


        /// <summary>
        /// Getter for tag with given tag name.
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <returns>All the snippets that have that tag</returns>
        public Tag[] GetTagByTagName(string tagName)
        {
            return JsonHelper.getJsonArray<Tag>(base.Get(URLConfig.BASEURL + baseTable + "/TagName?Tag_Name=" + tagName));
        }

        /// <summary>
        /// Posts a Tag to the DB
        /// </summary>
        /// <param name="tag">Tag to be posted</param>
        /// <returns>TODO: Currently posts true always!</returns>
        public Tag Post(ICreateFormData obj)
        {
            return JsonUtility.FromJson<Tag>(base.Post(URLConfig.BASEURL + baseTable, obj));
        }

        /// <summary>
        /// Wrapper class used to add an extra field to a object
        /// </summary>
        private class Temp : ICreateFormData
        {
            public List<IMultipartFormSection> aList = new List<IMultipartFormSection>();
            public List<IMultipartFormSection> CreateForm()
            {
                return aList;
            }
        }


        public int Delete(ICreateFormData obj, bool removeAll = false)
        {
            if (removeAll)
            {
                Temp temp= new Temp();
                temp.aList = obj.CreateForm();
                temp.aList.Add(new MultipartFormDataSection("Remove_All", true.ToString()));
                return base.Delete(URLConfig.BASEURL + baseTable, temp);
            }
            else
            {
                return base.Delete(URLConfig.BASEURL + baseTable, obj);
            }

        }


    }
}
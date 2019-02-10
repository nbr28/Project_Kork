using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Proyecto26;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class TagHandler
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
        /// Getter for all tags.
        /// </summary>
        /// <returns>list of tags</returns>
        public Tag[] GetAllTags()
        {
            mono.StartCoroutine(GetRequestGetAllTags());
            return tags;
        }

        /// <summary>
        /// Getter for single tag with given snippet id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <returns>single tag</returns>
        public Tag[] GetTagBySnippetId(int snippetId)
        {
            mono.StartCoroutine(GetRequestGetTagBySnippetId(snippetId));
            return tags;
        }

        /// <summary>
        /// Getter for single tag with given tag name.
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <returns>single tag</returns>
        public Tag[] GetTagByTagName(String tagName)
        {
            mono.StartCoroutine(GetRequestGetTagByTagName(tagName));
            return tags;
        }

        /// <summary>
        /// Posts a Tag to the DB
        /// </summary>
        /// <param name="tag">Tag to be posted</param>
        /// <returns>TODO: Currently posts true always!</returns>
        public bool PostTag(Tag tag)
        {
            PostRequestPostTag(tag);
            return true;
        }

        public bool DeleteTag(Tag tag)
        {
            PostRequestPostTag(tag);
            return true;
        }

        /// <summary>
        /// GET request for all tags.
        /// </summary>
        /// <param name="uri">url address that will be used for API call</param>
        private IEnumerator GetRequestGetAllTags(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone) ;
            tags = JsonHelper.getJsonArray<Tag>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request for tag with given snippet id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="uri">url address that will be used for API call</param>
        private IEnumerator GetRequestGetTagBySnippetId(int snippetId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/SnippetId/" + snippetId);
            while (!request.isDone) ;
            tags = JsonHelper.getJsonArray<Tag>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request for tag with given tag name.
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <param name="uri">url address that will be used for API call</param>
        private IEnumerator GetRequestGetTagByTagName(String tagName, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/TagName?Tag_Name=" + tagName);
            while (!request.isDone);
            tags = JsonHelper.getJsonArray<Tag>(request.text);
            yield return request;
        }


        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="tag">Tag to be sent</param>
        /// <param name="uri"></param>
        private IEnumerator PostRequestPostTag(Tag tag, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Tag_Name",tag.Tag_Name);
            form.AddField("Snippet_Id", tag.Snippet_Id);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                while (!webRequest.isDone) ;
                yield return webRequest;
            }
        }

        /// <summary>
        /// delete request
        /// </summary>
        /// <param name="tag">Tag to be sent</param>
        /// <param name="uri"></param>
        private IEnumerator DeleteRequestPostTag(Tag tag, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Tag_Name", tag.Tag_Name);
            form.AddField("Snippet_Id", tag.Snippet_Id);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                webRequest.method = "DELETE";//set to delete
                while (!webRequest.isDone) ;
                yield return webRequest;
            }
        }



    }
}
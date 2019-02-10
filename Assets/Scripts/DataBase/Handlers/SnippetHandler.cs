using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class SnippetHandler
    {
        private MonoBehaviour mono;
        private Snippet[] snippets;
        private Snippet snippet;
        private const string baseTable = "snippet";

        /// <summary>
        /// Constructor for SinpetHandler class.
        /// </summary>
        /// <param name="mono">a script with monobehavior that will be attached to this script</param>
        public SnippetHandler(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        /// <summary>
        /// Getter for all snippets.
        /// </summary>
        /// <returns>list of snippets</returns>
        public Snippet[] GetAllSnippets()
        {
            mono.StartCoroutine(GetRequestGetAllSnippets());
            return snippets;
        }

        /// <summary>
        /// Getter for single snippet with given id.
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns>single snippet</returns>
        public Snippet GetSnippet(int snippetId)
        {
            mono.StartCoroutine(GetRequestGetSnippet(snippetId));
            return snippet;
        }

        /// <summary>
        /// GET request for all snippets.
        /// </summary>
        /// <param name="uri">url address that will be used for API call</param>
        private IEnumerator GetRequestGetAllSnippets(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone);
            snippets = JsonHelper.getJsonArray<Snippet>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request for snippet with given id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="uri">url address that will be used for API call</param>
        private IEnumerator GetRequestGetSnippet(int snippetId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/" + snippetId);
            while (!request.isDone) ;
            snippet = JsonHelper.getJsonArray<Snippet>(request.text)[0];
            yield return request;
        }

       
    }
}

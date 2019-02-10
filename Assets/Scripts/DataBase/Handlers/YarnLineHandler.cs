using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class YarnLineHandler
    {
        private RequestHelper currentRequest;
        private MonoBehaviour mono;
        private YarnLine[] yarnLines;
        private YarnLine yarnLine;
        private const string baseTable = "YarnLine";

        /// <summary>
        /// Constructor for the YarnLineHandler class
        /// </summary>
        /// <param name="mono">a script with monobehavior that will be attached to this script</param>
        public YarnLineHandler(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        /// <summary>
        /// Gets all the yarn lines in the table
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestAllYarnLines()
        {
            mono.StartCoroutine(GetAllYarnLine());
            return yarnLines;
        }

        /// <summary>
        /// Get all yarn lines of a single yarn off its id
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestSingleYarnById(int yarnId)
        {
            mono.StartCoroutine(GetYarnLineById(yarnId));
            return yarnLines;
        }

        /// <summary>
        /// Gets a all yarn lines for a single snippet 
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestYarnsBySnippetId(int snippetId)
        {
            mono.StartCoroutine(GetYarnLineById(snippetId));
            return yarnLines;
        }


        /// <summary>
        /// Posts a yarn line
        /// </summary>
        /// <param name="yarn"></param>
        public void PostYarn(YarnLine yarn)
        {
            mono.StartCoroutine(_PostYarnLine(yarn));
        }

        /// <summary>
        /// POST request to create a new yarn line
        /// </summary>
        /// <param name="yarnline">yarn line</param>
        /// <param name="uri">API url</param>
        private IEnumerator _PostYarnLine(YarnLine yarnline, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Snippet_Id_To", yarnline.Snippet_Id_To);
            form.AddField("Yarn_Id", yarnline.Yarn_Id);
            form.AddField("Snippet_Id_From", yarnline.Snippet_Id_From);
            WWW request = new WWW(uri, form);
            while (!request.isDone) ;
            yield return request;


        }

        /// <summary>
        /// GET request to retrieve all yarn lines
        /// </summary>
        /// <param name="uri">API url</param>
        private IEnumerator GetAllYarnLine(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone) ;
            yarnLines = JsonHelper.getJsonArray<YarnLine>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request to retrieve yarn line by its id
        /// </summary>
        /// <param name="yarnLineId">id of yarn line</param>
        /// <param name="uri">API url</param>
        private IEnumerator GetYarnLineById(int yarnLineId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/snippetId/" + yarnLineId);
            while (!request.isDone) ;
            yarnLines = JsonHelper.getJsonArray<YarnLine>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request to retrieve yarn line by snippet id
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="uri">API url</param>
        private IEnumerator GetYarnLineBySnippetId(int snippetId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/snippet/" + snippetId);
            while (!request.isDone) ;
            yarnLines = JsonHelper.getJsonArray<YarnLine>(request.text);
            yield return request;
        }
    }
}

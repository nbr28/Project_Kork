using Proyecto26;
using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class YarnHandler
    {
        private RequestHelper currentRequest;
        private MonoBehaviour mono;
        private Yarn[] yarns;
        private Yarn yarn;
        private const string baseTable = "Yarn";

        /// <summary>
        /// Constructor for the YarnHandler class
        /// </summary>
        /// <param name="mono">a script with monobehavior that will be attached to this script</param>
        public YarnHandler(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        /// <summary>
        /// Gets all the yarns in the table
        /// </summary>
        /// <returns>list of Yarns</returns>
        public Yarn[] GetRequestAllYarn()
        {
            mono.StartCoroutine(GetAllYarn());
            return yarns;
        }

        /// <summary>
        /// Gets a single yarn off its id
        /// </summary>
        /// <param name="yarnId">id of requested yarn</param>
        /// <returns>single yarn</returns>
        public Yarn GetRequestSingleYarnById(int yarnId)
        {
            mono.StartCoroutine(GetYarnById(yarnId));
            return yarn;
        }

        /// <summary>
        /// Posts a yarn
        /// </summary>
        /// <param name="yarn">Yarn object that does not have an ID (this function will ignore the id if it is present) </param>
        /// <returns>The created yarn object with its Id or an id of  -1 if unsuccessful</returns>
        public Yarn PostYarn(Yarn yarn)
        {
            mono.StartCoroutine(_PostYarn(yarn));
            return this.yarn;
        }

        /// <summary>
        /// Deletes the given yarn from the database. Note this will cascade and delete the references in the YarnLine table associated with this yarn id
        /// </summary>
        /// <param name="yarn">The yarn to be deleted</param>
        public void DeleteYarn(Yarn yarn)
        {
            mono.StartCoroutine(DeleteYarnById(yarn.Yarn_Id));
        }

        /// <summary>
        /// POST request to create a new yarn
        /// </summary>
        /// <param name="yarn">yarn to be posted</param>
        /// <param name="uri">API url</param>
        private IEnumerator _PostYarn(Yarn yarn, string uri = URLConfig.BASEURL + baseTable)
        {
            var x = new { Yarn_Id=-1};

            WWWForm form = new WWWForm();
            form.AddField("Yarn_Name", yarn.Yarn_Name);
            form.AddField("Yarn_Id", yarn.Yarn_Id);
            WWW request = new WWW(uri, form);
            while (!request.isDone) ;
            Debug.Log(request.text);
            this.yarn = JsonUtility.FromJson <Yarn>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request to retrieve all yarns
        /// </summary>
        /// <param name="uri">API url</param>
        private IEnumerator GetAllYarn(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone) ;
            yarns = JsonHelper.getJsonArray<Yarn>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request to retrieve yarn by id
        /// </summary>
        /// <param name="yarnId">yarn id</param>
        /// <param name="uri">API url</param>
        private IEnumerator GetYarnById(int yarnId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/" + yarnId);
            while (!request.isDone) ;
            yarn = JsonUtility.FromJson<Yarn>(request.text);
            yield return request;
        }


        /// <summary>
        /// DELETE request to delete yarn by id
        /// </summary>
        /// <param name="yarnId">yarn id</param>
        /// <param name="uri">API url</param>
        private IEnumerator DeleteYarnById(int yarnId, string uri = URLConfig.BASEURL + baseTable)
        {
            UnityWebRequest request = UnityWebRequest.Delete(uri + "/" + yarnId);
            yield return request.SendWebRequest();

            //TODO: Error Handling 
            if (request.isNetworkError)
            {
                Debug.Log("Error While Sending: " + request.error);
            }
        }
    }
}

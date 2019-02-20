using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using Proyecto26;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class YarnHandler: GenericCRUD<Yarn>
    {
        [Obsolete]
        private RequestHelper currentRequest;
        private MonoBehaviour mono;
        [Obsolete]
        private Yarn[] yarns;
        [Obsolete]
        private Yarn yarn;
        private const string baseTable = "Yarn";



        /// <summary>
        /// Gets all the yarns in the table
        /// </summary>
        /// <returns>list of Yarns</returns>
        public Yarn[] GetRequestAllYarn()
        {
            return JsonHelper.getJsonArray<Yarn>(base.Get(URLConfig.BASEURL + baseTable + "s"));
        }

        /// <summary>
        /// Gets a single yarn off its id
        /// </summary>
        /// <param name="yarnId">id of requested yarn</param>
        /// <returns>single yarn</returns>
        public Yarn GetRequestSingleYarnById(int yarnId)
        {
            return JsonUtility.FromJson<Yarn>(base.Get(URLConfig.BASEURL + baseTable + "/" + yarnId));
        }


        public Yarn Put(IBaseConverter<Yarn> formData)
        {
            return this.Put(formData.GetBaseInterFace());
        }

        public Yarn Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<Yarn>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }

        public Yarn Post(IBaseConverter<Yarn> formData)
        {
            return this.Post(formData.GetBaseInterFace());
        }

        public Yarn Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<Yarn>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }

        /// <summary>
        /// Deletes the given yarn from the database. Note this will cascade and delete the references in the YarnLine table associated with this yarn id
        /// </summary>
        /// <param name="yarn">The yarn to be deleted</param>
        public override int Delete(string url, ICreateFormData obj)
        {
            return base.Delete(URLConfig.BASEURL + baseTable, obj);
        }

        /// <summary>
        /// POST request to create a new yarn
        /// </summary>
        /// <param name="yarn">yarn to be posted</param>
        /// <param name="uri">API url</param>
        [Obsolete]
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
        [Obsolete]
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
        [Obsolete]
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
        [Obsolete]
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

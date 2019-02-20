using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using Proyecto26;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DataBase
{
    public class YarnLineHandler : GenericCRUD<YarnLine>
    {
        [Obsolete]
        private RequestHelper currentRequest;
        [Obsolete]
        private YarnLine[] yarnLines;
        [Obsolete]
        private YarnLine yarnLine;
        private const string baseTable = "YarnLine";



        /// <summary>
        /// Gets all the yarn lines in the table
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestAllYarnLines()
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + "s"));
        }

        /// <summary>
        /// Get all yarn lines of a single yarn off its id
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestSingleYarnById(int yarnId)
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + yarnId));
        }

        /// <summary>
        /// Gets a all yarn lines for a single snippet 
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestYarnsBySnippetId(int snippetId)
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + "/snippetId/" + snippetId));
        }


        public YarnLine Post(IBaseConverter<YarnLine> formData)
        {
            return this.Post(formData.GetBaseInterFace());
        }


        public YarnLine Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<YarnLine>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }


        public YarnLineUpdate Put(IBaseConverter<YarnLineUpdate> formData)
        {
            return this.Put(formData.GetBaseInterFace());
        }

        public YarnLineUpdate Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<YarnLineUpdate>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }


        /// <summary>
        /// POST request to create a new yarn line
        /// </summary>
        /// <param name="yarnline">yarn line</param>
        /// <param name="uri">API url</param>
        [Obsolete]
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
        [Obsolete]
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
        [Obsolete]
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
        [Obsolete]
        private IEnumerator GetYarnLineBySnippetId(int snippetId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/snippet/" + snippetId);
            while (!request.isDone) ;
            yarnLines = JsonHelper.getJsonArray<YarnLine>(request.text);
            yield return request;
        }
    }
}

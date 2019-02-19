using Models;
using Proyecto26;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using System;

namespace Assets.Scripts.DataBase
{
    public class AssocationHandler : GenericCRUD<Association>
    {
        [Obsolete]
        private RequestHelper currentRequest;
        [Obsolete]
        private MonoBehaviour mono;
        [Obsolete]
        private Association[] associations;
        [Obsolete]
        private Association association;
        [Obsolete]
        private AssociationView[] associationViews;

        private const string baseTable = "Association";

        /// <summary>
        /// Getter for all associations.
        /// </summary>
        /// <returns>list of associations</returns>
        public Association[] GetAllAssocations()
        {
            return JsonHelper.getJsonArray<Association>(base.Get(URLConfig.BASEURL + baseTable + "s"));
        }

        /// <summary>
        /// Getter for single association with given id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <returns>single association</returns>
        public Association GetAssociation(int associationId)
        {
            return JsonUtility.FromJson<Association>(base.Get(URLConfig.BASEURL + baseTable + "/" + associationId));
        }


        /// <summary>
        /// Getter for single association with given id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <returns>single association</returns>
        public AssociationView[] GetAssociationViewForSnippet(int snippetId)
        {
            return JsonHelper.getJsonArray<AssociationView>(base.Get(URLConfig.BASEURL + "AssociationView/" + snippetId));
        }

        public Association Post(IBaseConverter<Association> formData)
        {
            throw new NotImplementedException();
            //return this.Post(formData.GetBaseInterFace());
        }

        public Association Post(ICreateFormData formData)
        {
            throw new NotImplementedException();
            return JsonUtility.FromJson<Association>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }


        public Association Put(IBaseConverter<Association> formData)
        {
            throw new NotImplementedException();
            //return this.Put(formData.GetBaseInterFace());
        }

        public Association Put(ICreateFormData obj)
        {
            throw new NotImplementedException();
            return JsonUtility.FromJson<Association>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }








        /// <summary>
        /// GET request for all associations.
        /// </summary>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetAllAssociations(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone) ;
            associations = JsonHelper.getJsonArray<Association>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request for association with given id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetAssociation(int associationId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/" + associationId);
            while (!request.isDone) ;
            association = JsonHelper.getJsonArray<Association>(request.text)[0];
            yield return request;
        }

        [Obsolete]
        private IEnumerator GetAssociationViewsForASnippet(int snippetId)
        {
            WWW request = new WWW(URLConfig.BASEURL + "AssociationView/" + snippetId);
            while (!request.isDone) ;
            associationViews = JsonHelper.getJsonArray<AssociationView>(request.text);
            yield return request;
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="tag">Tag to be sent</param>
        /// <param name="uri"></param>
        [Obsolete]
        private IEnumerator PostRequestAssociation(Association association, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Association_Id", association.Association_Id);
            form.AddField("Association_Name", association.Association_Name);
            form.AddField("Association_Data_Type", (int) association.Association_Data_Type);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                while (!webRequest.isDone) ;
                yield return webRequest;
            }
        }


        /// <summary>
        /// Put request by the ID of the association ONLY
        /// </summary>
        /// <param name="tag">Association to be sent</param>
        /// <param name="uri"></param>
        [Obsolete]
        private IEnumerator PutRequestAssociation(Association association, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Association_Id", association.Association_Id);
            form.AddField("Association_Name", association.Association_Name);
            form.AddField("Association_Data_Type", (int)association.Association_Data_Type);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                webRequest.method = "PUT";//sets method to put not post
                while (!webRequest.isDone) ;
                yield return webRequest;
            }
        }


    }
}

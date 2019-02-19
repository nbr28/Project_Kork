
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class AssocationHandler
    {

        private MonoBehaviour mono;
        private Association[] associations;
        private Association association;
        private AssociationView[] associationViews;
        private const string baseTable = "Association";

        /// <summary>
        /// Constructor for AssociationHandler class.
        /// </summary>
        /// <param name="mono">a script with monobehavior that will be attached to this script</param>
        public AssocationHandler(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        /// <summary>
        /// Getter for all associations.
        /// </summary>
        /// <returns>list of associations</returns>
        public Association[] GetAllAssocations()
        {
            mono.StartCoroutine(GetRequestGetAllAssociations());
            return associations;
        }

        /// <summary>
        /// Getter for single association with given id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <returns>single association</returns>
        public Association GetAssociation(int associationId)
        {
            mono.StartCoroutine(GetRequestGetAssociation(associationId));
            return association;
        }



        /// <summary>
        /// Getter for single association with given id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <returns>single association</returns>
        public AssociationView[] GetAssociationViewForSnippet(int snippetId)
        {
            mono.StartCoroutine(GetAssociationViewsForASnippet(snippetId));
            return this.associationViews;
        }

        /// <summary>
        /// Posts an Association 
        /// </summary>
        /// <param name="association"></param>
        public void PostAssociation(Association association)
        {
            mono.StartCoroutine(PostRequestAssociation(association));
        }

        /// <summary>
        /// PUT for a id of an given association
        /// </summary>
        /// <param name="association"></param>
        public void PutAssociation(Association association)
        {
            mono.StartCoroutine(PutRequestAssociation(association));
        }




        /// <summary>
        /// GET request for all associations.
        /// </summary>
        /// <param name="uri">url address that will be used for API call</param>
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
        private IEnumerator GetRequestGetAssociation(int associationId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/" + associationId);
            while (!request.isDone) ;
            association = JsonHelper.getJsonArray<Association>(request.text)[0];
            yield return request;
        }

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

using Assets.Scripts.DataBase.Handlers;
using Proyecto26;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class ConnectionHandler : GenericCRUD<Connection>
    {

        private const string baseTable = "Connection";

        /// <summary>
        /// Getter for all connections.
        /// </summary>
        /// <returns>list of connections</returns>
        public Connection[] GetAllConnections()
        {

           // mono.StartCoroutine(GetRequestGetAllConnections());
            return JsonHelper.getJsonArray<Connection>(base.Get(URLConfig.BASEURL + baseTable + "s"));
        }

        /// <summary>
        /// Getter for single connection with given snippet id and association id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="associationId">association id</param>
        /// <returns>single connection</returns>
        public Connection GetConnection(int snippetId, int associationId)
        {
            //mono.StartCoroutine(GetRequestGetConnection(snippetId, associationId));
            return JsonUtility.FromJson<Connection>(base.Get(URLConfig.BASEURL + baseTable + "/single?Snippet_Id=" + snippetId + "&Association_Id=" + associationId));

        }

        /// <summary>
        /// Getter for single connection with given snippet id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <returns>single connection</returns>
        public Connection GetConnectionBySnippetId(int snippetId)
        {

            //mono.StartCoroutine(GetRequestGetConnectionBySnippetId(snippetId));
            return JsonUtility.FromJson<Connection>(base.Get(URLConfig.BASEURL + baseTable + "/snippet/" + snippetId));
        }

        /// <summary>
        /// Getter for single connection with given association id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <returns>single connection</returns>
        public Connection GetConnectionByAssociationId(int associationId)
        {
            //mono.StartCoroutine(GetRequestGetConnectionByAssociationId(associationId));
            return JsonUtility.FromJson<Connection>(base.Get(URLConfig.BASEURL + baseTable + "/association/" + associationId));
        }


        /// <summary>
        /// TODO: Return success status
        /// </summary>
        /// <param name="connection"></param>
        public Connection PostConnection(Connection connection)
        {
            //mono.StartCoroutine(PostRequestConnection(connection));
            return JsonUtility.FromJson<Connection>(base.Post(URLConfig.BASEURL + baseTable, connection));
        }

        /// <summary>
        /// TODO: Return success status
        /// </summary>
        /// <param name="connection"></param>
        public Connection PutConnection(Connection connection)
        {
            return JsonUtility.FromJson<Connection>(base.Put(URLConfig.BASEURL + baseTable, connection));
        }


        /// <summary>
        /// GET request for all connections.
        /// </summary>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetAllConnections(string uri = URLConfig.BASEURL + baseTable + "s")
        {
            WWW request = new WWW(uri);
            while (!request.isDone) ;
            connections = JsonHelper.getJsonArray<Connection>(request.text);
            yield return request;
        }

        /// <summary>
        /// GET request for connection with given snippet id and association id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="associationId">association id</param>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetConnection(int snippetId, int associationId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/single?Snippet_Id=" + snippetId + "&Association_Id=" + associationId);
            while (!request.isDone) ;
            connection = JsonHelper.getJsonArray<Connection>(request.text)[0];
            yield return request;
        }

        /// <summary>
        /// GET request for connection with given snippet id.
        /// </summary>
        /// <param name="snippetId">snippet id</param>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetConnectionBySnippetId(int snippetId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/snippet/" + snippetId);
            while (!request.isDone) ;
            connection = JsonHelper.getJsonArray<Connection>(request.text)[0];
            yield return request;
        }

        /// <summary>
        /// GET request for connection with given association id.
        /// </summary>
        /// <param name="associationId">association id</param>
        /// <param name="uri">url address that will be used for API call</param>
        [Obsolete]
        private IEnumerator GetRequestGetConnectionByAssociationId(int associationId, string uri = URLConfig.BASEURL + baseTable)
        {
            WWW request = new WWW(uri + "/association/" + associationId);
            while (!request.isDone) ;
            connection = JsonHelper.getJsonArray<Connection>(request.text)[0];
            yield return request;
        }


        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="tag">Tag to be sent</param>
        /// <param name="uri"></param>
        [Obsolete]
        private IEnumerator PostRequestConnection(Connection connection, string uri = URLConfig.BASEURL + baseTable)
        {
            WWWForm form = new WWWForm();
            form.AddField("Snippet_Id", connection.Snippet_Id);
            form.AddField("Assoication_Id", connection.Association_Id);
            form.AddField("Connection_Data", connection.data);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                while (!webRequest.isDone) ;
                yield return webRequest;
            }
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="tag">Tag to be sent</param>
        /// <param name="uri"></param>
        [Obsolete]
        private void PutRequestConnection(Connection connection, string uri = URLConfig.BASEURL + baseTable)
        {
            //WWWForm form = new WWWForm();
            //form.AddField("Snippet_Id", connection.Snippet_Id);
            //form.AddField("Assoication_Id", connection.Assoication_Id);
            //form.AddField("Connection_Data", connection.data);
            //using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            //{
            //    //webRequest.method = "PUT";
            //    while (!webRequest.isDone||!webRequest.isNetworkError) ;
            //    yield return webRequest;
            //}


            RestClient.Put<Connection>(uri, new Connection
            {
                Snippet_Id = connection.Snippet_Id,
                Association_Id = connection.Association_Id,
                data = connection.data
            }, (err, res, body) =>
            {
                if (err != null)
                {
                    // EditorUtility.DisplayDialog("Error", err.Message, "Ok");
                }
                else
                {
                    //EditorUtility.DisplayDialog("Success", JsonUtility.ToJson(body, true), "Ok");
                }
            });
        }


    }
}

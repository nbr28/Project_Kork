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
        public Connection Get(int snippetId, int associationId)
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
        public Connection Post(Connection connection)
        {
            //mono.StartCoroutine(PostRequestConnection(connection));
            return JsonUtility.FromJson<Connection>(base.Post(URLConfig.BASEURL + baseTable, connection));
        }

        /// <summary>
        /// TODO: Return success status
        /// </summary>
        /// <param name="connection"></param>
        public Connection Put(Connection connection)
        {
            return JsonUtility.FromJson<Connection>(base.Put(URLConfig.BASEURL + baseTable, connection));
        }
    }
}

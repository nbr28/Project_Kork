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

            return this.Post(formData.GetBaseInterFace());
        }

        public Association Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<Association>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }


        public Association Put(IBaseConverter<Association> formData)
        {
         
            return this.Put(formData.GetBaseInterFace());
        }

        public Association Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<Association>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }
    }
}

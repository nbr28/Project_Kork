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
    }
}

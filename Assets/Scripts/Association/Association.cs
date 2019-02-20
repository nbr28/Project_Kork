using Assets.Scripts.DataBase;
using Assets.Scripts.DataBase.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssociationsName
{
    //Types of data currently accepted as associations
    public enum DataType { String, Integer, Float }

    public class AssociationState:IBaseConverter<Association>
    {
        public int Association_Id { get; set; }


        // name of the association
        protected string name;

        // data type of the association
        protected DataType type;



        /// <summary>
        /// Association constructor
        /// </summary>
        /// <param name="name">association name</param>
        /// <param name="type">association data type</param>
        public AssociationState(string name, DataType type, int id =-1)
        {
            this.name = name;
            this.type = type;
            this.Association_Id = id;
        }

        public AssociationState(Association association)
        {
            this.name = association.Association_Name;
            this.type = association.Association_Data_Type;
            this.Association_Id = association.Association_Id;
        }


        public override bool Equals(object obj)
        {
            var association = obj as AssociationState;
            return association != null &&
                   name == association.name &&
                   type == association.type;
        }


        public override int GetHashCode()
        {
            var hashCode = -1534309985;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            return hashCode;
        }

        public Association GetBaseInterFace()
        {
            return new Association(this.Association_Id, type, name);
        }
    }
}
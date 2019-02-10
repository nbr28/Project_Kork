using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssociationsName
{
    public class Association
    {
        public int Association_Id { get; set; }
        //Types of data currently accepted as associations
        public enum DataType { String, Integer, Float }

        // name of the association
        protected string name;

        // data type of the association
        protected DataType type;

        /// <summary>
        /// Association constructor
        /// </summary>
        /// <param name="name">association name</param>
        /// <param name="type">association data type</param>
        public Association(string name, DataType type, int id)
        {
            this.name = name;
            this.type = type;
            this.Association_Id = id;
        }

        public override bool Equals(object obj)
        {
            var association = obj as Association;
            return association != null &&
                   Association_Id == association.Association_Id &&
                   name == association.name &&
                   type == association.type;
        }

        public override int GetHashCode()
        {
            var hashCode = -1534309985;
            hashCode = hashCode * -1521134295 + Association_Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            return hashCode;
        }
    }
}
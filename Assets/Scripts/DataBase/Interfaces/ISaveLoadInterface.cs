using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataBase
{
    /// <summary>
    /// Interface to allow instantiated objects to preform CRUD on the database
    /// </summary>
    interface ISaveLoadInterface
    {
        /// <summary>
        /// Saves a given object to the database
        /// </summary>
        void Save();
        /// <summary>
        /// Loads a given object to the database
        /// </summary>
        void Load();
        /// <summary>
        /// Updates a given object to the database
        /// </summary>
        void UpdateObject();
        /// <summary>
        /// Deletes a given object to the database
        /// </summary>
        void Delete();
    }
}

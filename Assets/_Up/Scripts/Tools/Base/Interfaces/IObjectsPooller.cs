using System.Collections.Generic;

namespace Assets._Scripts.Tools.Base.Interfaces
{
    public interface IObjectsPooller<T>
    {
        List<T> AllObjects { get; set; }
        List<T> NotInUseObjects { get; set; }

        /// <summary>
        /// Add object to AllObjects
        /// </summary>
        /// <param name="object"></param>
        void AddObject(T @object);

        /// <summary>
        /// Remove object from AllObjects and InUseObjects
        /// </summary>
        /// <param name="object"></param>
        void RemoveObject(T @object);

        /// <summary>
        /// Add FreeObject to InUseObjects and returt
        /// </summary>
        /// <returns>FreeObject</returns>
        T GetFreeObject();

        /// <summary>
        /// Remove object from InUseObjects
        /// </summary>
        /// <param name="object"></param>
        void ReturnObject(T @object);
    }
}
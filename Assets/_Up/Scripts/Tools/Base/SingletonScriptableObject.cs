using System.Linq;
using UnityEngine;

namespace Assets._Scripts.Tools.Base
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.LoadAll<T>("Data").FirstOrDefault();

                    if (!_instance)
                    {
                        GameDebug.LogErrorFormat("Could not load scriptable object of type \"{0}\"", typeof(T).ToString());
                    }
                }
                return _instance;
            }
        }
    }
}

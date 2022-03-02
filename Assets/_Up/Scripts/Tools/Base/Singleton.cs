namespace Assets._Scripts.Tools.Base
{
    public class Singleton<T> where T : new()
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }

        public static void Delete()
        {
            GameDebug.Log("Deleting Singleton with type: " + typeof(T));
            instance = default(T);
        }
    }
}

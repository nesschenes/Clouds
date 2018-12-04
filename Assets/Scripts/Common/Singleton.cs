using UnityEngine;

namespace Clouds
{
    public class Singleton<T> where T : new()
    {
        private static T mInstance = default(T);
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new T();

                return mInstance;
            }
        }
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T mInstance;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = (T)FindObjectOfType(typeof(T));

                return mInstance;
            }
        }

        public static bool IsExist { get { return (mInstance != null); } }

        protected virtual void Awake()
        {
            mInstance = (T)((System.Object)this);
        }

        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }

    public class UndeadMonoSingleton<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);
        }
    }
}
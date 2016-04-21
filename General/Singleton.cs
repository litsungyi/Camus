using UnityEngine;

namespace Camus.General
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
		private static readonly string NodeName = "(Singletons)";
        private static object instanceLock = new object();
        private static T instance = default( T );
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if ( applicationIsQuitting )
                {
                    Debug.LogWarning( "[Singleton] Instance '" + typeof( T ) + "' already destroyed on application quit." );
                    return default( T );
                }

                lock ( instanceLock )
                {
                    if ( null != instance )
                    {
                        return instance;
                    }

                    instance = ( T ) FindObjectOfType( typeof( T ) );
                    if ( FindObjectsOfType( typeof( T ) ).Length > 1 )
                    {
                        Debug.LogError( "[Singleton] More than 1 singleton!" );
                        return instance;
                    }

                    if ( null != instance )
                    {
                        return instance;
                    }

					var root = GameObject.Find( NodeName );
					if ( root == null )
					{
						Debug.Log( "[Singleton] First singleton created!" );
						root = new GameObject( NodeName );
						DontDestroyOnLoad( root );
					}
					
					Debug.Log( "[Singleton] Singleton " + typeof( T ) + " is created." );
					instance = root.AddComponent<T>();
                    return instance;
                }
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits
        /// If any script calls Instance after it have been destroyed,
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object
        /// </summary>
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
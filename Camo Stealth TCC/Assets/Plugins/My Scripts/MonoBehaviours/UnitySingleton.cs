using UnityEngine;

public class UnitySingleton<T> : EnhancedBehaviour where T : EnhancedBehaviour {

	private static object _lock = new object();
	
	protected static T _instance;
	
	public static T Instance
	{
		get
		{
			lock(_lock)
			{
				if (_instance == null)
				{
					T[] instances = FindObjectsOfType(typeof(T)) as T[];
 
					if (instances != null && instances.Length > 0 )
					{
						_instance = instances[0];

						if(instances.Length > 1) {

							Debug.LogError("[Singleton] Something went really wrong " +
								" - there should never be more than 1 singleton!" +
								" Reopenning the scene might fix it.");
							return _instance;
						}
					}
 
					if (_instance == null)
					{
						if (!sceneIsFinishing) {
							Debug.LogError("[Singleton] Instance '"+ typeof(T) + "' not found");
							return null;
						}
						else {
							Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
							                 "' already destroyed on application quit." +
							                 " Won't create again - returning null.");
							return null;
						}
					}
					else {
						Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
					}
				}
 
				return _instance;
			}
		}
	}

	/// <summary>
	/// The scene is finishing.
	/// </summary>
	protected static bool sceneIsFinishing = false;

	protected override void EnhancedAwake ()
	{
		base.EnhancedAwake ();
		sceneIsFinishing = false;
	}
	
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	protected override void EnhancedOnDestroy ()
	{
		base.EnhancedOnDestroy();
		sceneIsFinishing = true;
	}
}

using UnityEngine;
using System.Collections;

public class FollowTransform : EnhancedBehaviour {


	[SerializeField]
	Transform target;

	public Transform Target {
		get {
			return target;
		}
		protected set {
			target = value;
		}
	}

	/// <summary>
	/// Follow axis.
	/// </summary>
	[SerializeField]
	public bool followX = true, followY = true, followZ = true;
	[SerializeField]
	public float speed = 1;

	/// <summary>
	/// My transform.
	/// </summary>
	Transform myTransform;
	
	public Transform MyTransform {
		get {
			if(!myTransform) 
				myTransform = transform;
			return myTransform;
		}
	}
	


	protected override void EnhancedLateUpdate ()
	{
		base.EnhancedLateUpdate ();

		if(target) {
			var leftBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 0, dist)
				).x;
			
			var rightBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(1, 0, dist)
				).x;
			
			var topBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 1, dist)
				).x;
			
			var bottomBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 0, dist)
				).x;
			
			if(Physics2D.Raycast(new Vector2(leftBorder, topBorder) + Vector3.left * (MyCollider.size.x / 2f) + Vector3.down * (MyCollider.size.y / 2f), -Vector2.up, minDistanceFromGround, mapMask.value)) {
				state = WalkerState.GROUNDED;
				transform.parent = groundTransform;
				return;
			}


			Vector3 myPos = MyTransform.position;
			myPos.x = followX ? Mathf.Lerp(MyTransform.position.x, target.position.x, speed) : MyTransform.position.x;
			myPos.y = followY ? Mathf.Lerp(MyTransform.position.y, target.position.y, speed) : MyTransform.position.y;
			myPos.z = followZ ? target.position.z : MyTransform.position.z;
			MyTransform.position = myPos;
		}
	}
}

using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Walker))]
public class PlayerCameraControl : EnhancedBehaviour {
	public FollowTransform followCamera;
	Walker _walker;
	
	protected override void EnhancedStart ()
	{
		base.EnhancedStart ();
		_walker = GetComponent<Walker>();
	}
	
	protected override void EnhancedUpdate ()
	{
		base.EnhancedUpdate ();
		var dist = (transform.position - Camera.main.transform.position).z;
		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0.45f, 0, dist)
			).x;
		
		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0.55f, 0, dist)
			).x;


		followCamera.followX = (leftBorder > _walker.transform.position.x || rightBorder < _walker.transform.position.x ); ///Mathf.Abs(followCamera.transform.position.x  - _walker.transform.position.x) > 
		followCamera.followY = _walker.State != WalkerState.GROUNDED;
	}


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovablePlatform : EnhancedBehaviour {

	/// <summary>
	/// The time moving.
	/// </summary>
	[SerializeField]
	float maxDistance = 10f;

	[SerializeField]
	float speed = 1.5f;

	/// <summary>
	/// The start direction.
	/// </summary>
	[SerializeField]
	Vector2 startDirection = Vector2.right;

	/// <summary>
	/// The direction.
	/// </summary>
	Vector2 direction;

	/// <summary>
	/// The start position.
	/// </summary>
	Vector3 startPosition;

	/// <summary>
	/// To position.
	/// </summary>
	Vector3 toPosition;
	
	protected override void EnhancedOnEnable ()
	{
		base.EnhancedOnEnable ();
		Body.fixedAngle = true;
		Body.gravityScale = 0f;
	}

	protected override void EnhancedStart ()
	{
		base.EnhancedStart ();
		startPosition = transform.position;
		direction = startDirection;
		toPosition = startPosition + new Vector3(direction.x, direction.y, transform.position.z) * maxDistance;
	}
	
	protected override void EnhancedUpdate ()
	{
		base.EnhancedUpdate ();

		if(transform.position != toPosition) {
			transform.position = Vector3.MoveTowards(transform.position, toPosition, speed * Time.deltaTime);
		}
		else {
			direction = -direction;
			startPosition = transform.position;
			toPosition = startPosition + new Vector3(direction.x, direction.y, transform.position.z) * maxDistance;
		}
	}

	/// <summary>
	/// My rigidbody.
	/// </summary>
	Rigidbody2D myRigidbody;
	
	public Rigidbody2D Body {
		get {
			if(!myRigidbody) {
				myRigidbody = rigidbody2D;
			}
			return myRigidbody;
		}
	}

	protected override void EnhancedOnDrawGizmosSelected ()
	{
		base.EnhancedOnDrawGizmosSelected ();

		if(!Application.isPlaying) {
			startPosition = transform.position;
			direction = startDirection;
		}

		Vector3 auxToPosition = startPosition + new Vector3(direction.x, direction.y, transform.position.z) * maxDistance;
		Gizmos.DrawLine(startPosition, auxToPosition);
	}
}

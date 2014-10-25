using UnityEngine;
using System.Collections;

public enum WalkerState { GROUNDED, RISING, FALLING }

public class Walker : EnhancedBehaviour {

	/// <summary>
	/// My rigidbody.
	/// </summary>
	Rigidbody2D myRigidbody;

	public Rigidbody2D Body {
		get {
			if(!myRigidbody) {
				myRigidbody = rigidbody2D;
				myRigidbody.fixedAngle = true;
				myRigidbody.gravityScale = 0f;
			}
			return myRigidbody;
		}
	}

	/// <summary>
	/// The state.
	/// </summary>
	[SerializeField]
	WalkerState state;

	public bool Crouched {get; private set;}

	public WalkerState State {
		get {
			return state;
		}
	}

	/// <summary>
	/// The max speed.
	/// </summary>
	[SerializeField]
	float maxSpeed = 10f;

	/// <summary>
	/// The minimum time to jump.
	/// </summary>
	Timer minTimeToJump = new Timer();

	/// <summary>
	/// The base jump force.
	/// </summary>
	[SerializeField]
	float baseJumpForce = 1.5f;

	/// <summary>
	/// The rise acceleration.
	/// </summary>
	[SerializeField]
	float riseAcceleration = 0.25f;

	/// <summary>
	/// The fall acceleration.
	/// </summary>
	[SerializeField]
	float fallAcceleration = 0.15f;

	/// <summary>
	/// The gravity.
	/// </summary>
	[SerializeField]
	float gravity = -1f;

	/// <summary>
	/// The target height vel.
	/// </summary>
	float targetHeightVel;

	protected override void EnhancedStart ()
	{
		base.EnhancedStart ();
		targetHeightVel = gravity;
	}

	Vector2 targetVelocity = Vector2.zero;

	public Vector2 TargetVelocity {
		get {
			return targetVelocity;
		}
	}

	protected override void EnhancedFixedUpdate ()
	{
		base.EnhancedFixedUpdate ();

		// Calculate how fast we should be moving
		targetVelocity.x = Input.GetAxis("Horizontal");
		targetVelocity.y = targetHeightVel;
		targetVelocity = transform.TransformDirection(targetVelocity) * maxSpeed;
		
		// Apply a force that attempts to reach our target velocity
		Vector2 velocityChange = (targetVelocity - Body.velocity);
		Body.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	protected override void EnhancedUpdate ()
	{
		base.EnhancedUpdate ();

		if(MyTransform.position.y < -10f) {
			Application.LoadLevel("game");
		}

		minTimeToJump.UpdateTime(Time.deltaTime);
		
		if(Input.GetButtonDown("Jump")) {
			minTimeToJump.Reset();
		}


		
		if(state == WalkerState.GROUNDED) {
			
			targetHeightVel = gravity;
			Crouched = Input.GetButton("Duck");
			if(!minTimeToJump.IsOver(0.05f)) {
				targetHeightVel = baseJumpForce;
				state = WalkerState.RISING;
			}




		}
		else if(state == WalkerState.RISING) {
			
			targetHeightVel = Mathf.MoveTowards(targetHeightVel, 0f, riseAcceleration);
			
			if(!Input.GetKey(KeyCode.Space)) {
				targetHeightVel = 0f;
			}
			
			if(targetHeightVel <= 0f) {
				state = WalkerState.FALLING;
			}
		}
		else {
			targetHeightVel = Mathf.MoveTowards(targetHeightVel, gravity, fallAcceleration);
		}

		Debug.DrawRay(MyTransform.position + Vector3.left * (MyCollider.size.x / 2f) + Vector3.up * MyCollider.size.y / 2f, -Vector2.up * minDistanceFromGround, Color.yellow);
		Debug.DrawRay(MyTransform.position + Vector3.right * (MyCollider.size.x / 2f) + Vector3.up * MyCollider.size.y / 2f, -Vector2.up * minDistanceFromGround, Color.red);
		Debug.DrawRay((MyTransform.position + Vector3.up * MyCollider.size.y / 2f) + Vector3.left * (MyCollider.size.x / 2f), Vector2.up * minDistanceFromGround, Color.red);
		Debug.DrawRay((MyTransform.position + Vector3.down *  MyCollider.size.y / 2f)  + Vector3.right * (MyCollider.size.x / 2f), Vector2.up * minDistanceFromGround, Color.red);
	}

	Transform myTransform;

	public Transform MyTransform {
		get {
			if(!myTransform)
				myTransform = transform;
			return myTransform;
		}
	}

	/// <summary>
	/// The ground mask.
	/// </summary>
	[SerializeField]
	LayerMask groundMask;

	[SerializeField]
	float minDistanceFromGround = 0.05f;

	protected override void EnhancedOnCollisionEnter2D (Collision2D col)
	{
		base.EnhancedOnCollisionEnter2D (col);
		Body.velocity = Vector2.zero;
		CheckGround(col.transform);
	}

	protected override void EnhancedOnCollisionStay2D (Collision2D col)
	{
		base.EnhancedOnCollisionStay2D (col);

		if(state == WalkerState.GROUNDED)
			return;

		CheckGround(col.transform);
	}

	protected override void EnhancedOnCollisionExit2D (Collision2D col)
	{
		base.EnhancedOnCollisionExit2D (col);
		CheckGround(col.transform);
	}

	[SerializeField]
	BoxCollider2D myCollider;

	public BoxCollider2D MyCollider {
		get {
			if(!myCollider)
				myCollider = GetComponent<BoxCollider2D>();
			return myCollider;
		}
	}

	void CheckGround(Transform groundTransform) {

		if(state == WalkerState.RISING) {
			transform.parent = null;
			CheckRoof();
			return;
		}

		if(Physics2D.Raycast(MyTransform.position + Vector3.left * (MyCollider.size.x / 2f) + Vector3.down * (MyCollider.size.y / 2f), -Vector2.up, minDistanceFromGround, groundMask.value)) {
			state = WalkerState.GROUNDED;
			transform.parent = groundTransform;
			return;
		}

		if(Physics2D.Raycast(MyTransform.position + Vector3.right * (MyCollider.size.x / 2f) + Vector3.down * (MyCollider.size.y / 2f), -Vector2.up, minDistanceFromGround, groundMask.value)) {
			state = WalkerState.GROUNDED;
			transform.parent = groundTransform;
			return;
		}

		state = WalkerState.FALLING;
		transform.parent = null;
	}

	void CheckRoof() {

		if(Physics2D.Raycast((MyTransform.position + Vector3.up * MyCollider.size.y) + Vector3.left * (MyCollider.size.x / 2f), Vector2.up, minDistanceFromGround, groundMask.value)) {
			targetHeightVel = 0f;
			state = WalkerState.FALLING;
			return;
		}
		
		if(Physics2D.Raycast((MyTransform.position + Vector3.up * MyCollider.size.y) + Vector3.down * (MyCollider.size.x / 2f), Vector2.up, minDistanceFromGround, groundMask.value)) {
			state = WalkerState.FALLING;
			targetHeightVel = 0f;
			return;
		}
	}
}

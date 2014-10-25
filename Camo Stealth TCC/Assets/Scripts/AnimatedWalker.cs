using UnityEngine;
using System.Collections;

public class AnimatedWalker : EnhancedBehaviour {

	[SerializeField]
	Walker walker;

	/// <summary>
	/// My animator.
	/// </summary>
	[SerializeField]
	Animator myAnimator;

	protected override void EnhancedStart ()
	{
		base.EnhancedStart ();
		myAnimator = GetComponent<Animator>();

		if(!myAnimator) {
			myAnimator = walker.GetComponent<Animator>();
		}
	}

	protected override void EnhancedUpdate ()
	{
		base.EnhancedUpdate ();

		if(myAnimator) {

			float speed = Mathf.Abs(walker.TargetVelocity.x);
			myAnimator.SetFloat("Speed", speed);
			myAnimator.SetBool("Grounded", walker.State == WalkerState.GROUNDED);
			myAnimator.SetBool("Crouched", walker.Crouched);
			if(Input.GetButtonDown("Fire1")){
				myAnimator.SetTrigger("Atacar");
			}
		}
	}
}

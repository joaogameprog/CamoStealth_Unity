using UnityEngine;
using System.Collections;

public class MirrorWalker : EnhancedBehaviour {

	[SerializeField]
	Walker walker;

	/// <summary>
	/// The mirrored.
	/// </summary>
	[SerializeField]
	bool mirrored;

	protected override void EnhancedStart ()
	{
		base.EnhancedStart ();

		if(mirrored) {

			Axis chosenAxis;
			
			if(useXAxis) {
				chosenAxis = Axis.X;
			}
			else {
				chosenAxis = Axis.Y;
			}

			walker.transform.Mirror(chosenAxis);
		}
	}

	/// <summary>
	/// The use X axis.
	/// </summary>
	[SerializeField]
	bool useXAxis = true;

	protected override void EnhancedUpdate ()
	{
		base.EnhancedUpdate ();

		float delta = 0f;
		Axis chosenAxis;

		if(useXAxis) {
			delta = walker.TargetVelocity.x;
			chosenAxis = Axis.X;
		}
		else {
			delta = walker.TargetVelocity.y;
			chosenAxis = Axis.Y;
		}

		if(delta < 0f) {

			if(!mirrored) {
				walker.transform.Mirror(chosenAxis, ref mirrored);
			}
		}
		else if(delta > 0f) {
			
			if(mirrored) {
				walker.transform.Mirror(chosenAxis, ref mirrored);
			}
		}
	}
}

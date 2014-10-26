using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour {
	#region Fields
	/// <summary>
	/// The life.
	/// </summary>
	float life;
	/// <summary>
	/// Gets or sets the life.
	/// </summary>
	/// <value>The life.</value>
	public float Life
	{ 
		get 
		{ 
			return life; 
		} 
		set 
		{ 
			var fullAlpha = 1f / lifeBar.Length;
			for (int i = 0; i < lifeBar.Length; i++) {
				lifeBar [i].color = new Color (1f, 1f, 1f, Mathf.InverseLerp (fullAlpha * i, fullAlpha * (i + 1), value));
			}
			life = value; 
		}
	}


	/// <summary>
	/// The life bar.
	/// </summary>
	[SerializeField]
	SpriteRenderer [] lifeBar = null;

	/// <summary>
	/// The points text.
	/// </summary>
	[SerializeField]
	TextMesh pointsText;

	/// <summary>
	/// The max points.
	/// </summary>
	const float MAX_POINTS = 10000;

	/// <summary>
	/// The total time in seconds.
	/// </summary>
	public float TotalTime = 300;

	float pointsDeltaTime;

	/// <summary>
	/// The points.
	/// </summary>
	float points;

	/// <summary>
	/// Gets or sets the points.
	/// </summary>
	/// <value>The points.</value>
	public float Points 
	{
		get
		{
			return points;
		}
		set
		{
			points = Mathf.Max(0, value);
			pointsText.text = ((int)points).ToString () + " pts";
		}
	}



	#endregion
	#region Behaviours
	void OnEnable () {
		StartTimer ();
	}
	
	void OnDisable () {
	}


	void FixedUpdate(){
		/// 5 minutes to zero
		Points = Points - Time.deltaTime * pointsDeltaTime;

	}

	public void StartTimer()
	{
		Points = MAX_POINTS;
		pointsDeltaTime = MAX_POINTS / TotalTime;
	}
	#endregion
	
	#region Events

	#endregion
}

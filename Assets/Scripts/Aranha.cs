using UnityEngine;
using System.Collections;

public class Aranha : MonoBehaviour {

	Animator animator;

	public float speed = 0.02f;
	public float walkRange = 6;
	public int restTime = 300;

	public int initialXPosition;
	public int resting = 0;

	public bool playerDetected = false;

	public GameObject bulletType = null;

	void Awake(){
		animator = GetComponent<Animator>();
		initialXPosition = (int)transform.position.x;
		if(restTime <= 0) restTime = 1;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Exit.victory) return;
		if(!playerDetected){
			if(resting > 0){
				if(--resting == 0){
					transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
				}
			}else{
				transform.position += new Vector3(speed*Mathf.Sign(transform.localScale.x),0,0);
				if((int)transform.position.x == initialXPosition+(Mathf.Sign(transform.localScale.x)>=0?walkRange:0))
					resting = restTime;
			}
		}else{
			if(Time.frameCount%10==0){
				int direction = (int)Mathf.Sign(transform.localScale.x);
				GameObject bullet = Instantiate(bulletType,transform.position + new Vector3(0.56f * direction,0.24f,0),Quaternion.identity) as GameObject;
				bullet.GetComponent<Gunshot>().speed *= direction;
			}
		}
		animator.SetInteger("Resting",resting);
		animator.SetBool("PlayerDetected",playerDetected);
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Player))] 
public abstract class PlayerBehaviour : MonoBehaviour{

	private Player myPlayer;
	private Animator myAnimator;
	private Rigidbody2D myRigidbody;
	private SpriteRenderer mySpriteRenderer;
	private MoveSkillModule myMove;
	private HideSkillModule myHide;
	private WallSkillModule myWall;
	private DashSkillModule myDash;
	private JumpSkillModule myJump;
	
	protected Player player{
		get{
			if(myPlayer == null){
				myPlayer = GetComponent<Player>();
			}
			return myPlayer;
		}
		set{
			myPlayer = value;
		}
	}

	protected Animator anim{
		get{
			if(myAnimator == null){
				myAnimator = GetComponent<Animator>();
			}
			return myAnimator;
		}
		set{
			myAnimator = value;
		}
	}

	protected Rigidbody2D rb{
		get{
			if(myRigidbody == null){
				myRigidbody = GetComponent<Rigidbody2D>();
			}
			return myRigidbody;
		}
		set{
			myRigidbody = value;
		}
	}

	protected SpriteRenderer sr{
		get{
			if(mySpriteRenderer == null){
				mySpriteRenderer = GetComponent<SpriteRenderer>();
			}
			return mySpriteRenderer;
		}
		set{
			mySpriteRenderer = value;
		}
	}

	protected MoveSkillModule move{
		get{
			if(myMove == null){
				myMove = GetComponent<MoveSkillModule>();
			}
			return myMove;
		}
		set{
			myMove = value;
		}
	}

	protected HideSkillModule hide{
		get{
			if(myHide == null){
				myHide = GetComponent<HideSkillModule>();
			}
			return myHide;
		}
		set{
			myHide = value;
		}
	}

	protected WallSkillModule wall{
		get{
			if(myWall == null){
				myWall = GetComponent<WallSkillModule>();
			}
			return myWall;
		}
		set{
			myWall = value;
		}
	}

	protected DashSkillModule dash{
		get{
			if(myDash == null){
				myDash = GetComponent<DashSkillModule>();
			}
			return myDash;
		}
		set{
			myDash = value;
		}
	}

	protected JumpSkillModule jump{
		get{
			if(myJump == null){
				myJump = GetComponent<JumpSkillModule>();
			}
			return myJump;
		}
		set{
			myJump = value;
		}
	}
}

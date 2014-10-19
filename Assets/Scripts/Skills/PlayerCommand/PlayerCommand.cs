using UnityEngine;
using System.Collections;

public abstract class PlayerCommand : PlayerBehaviour {

	public virtual void runCommand(){
		if(commandCondition())
			startCommand();
		else 
			endCommand();
	}

	protected abstract bool commandCondition();

	protected abstract void startCommand();

	protected abstract void endCommand();
}

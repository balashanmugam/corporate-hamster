using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {


[SerializeField]
Animator jar, cash,boss;


	void Start () {

	}
	
	
	void Update () {

	}

	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		GameManager.OnGameStateChange += OnGameStateChange;
	}

	/// </summary>
	void OnDisable()
	{
		GameManager.OnGameStateChange += OnGameStateChange;
	}
	void OnGameStateChange(GameStates gamestate){
switch(gamestate){
	case GameStates.GameOver:
	case GameStates.PowerUp: 	cash.SetBool("Cash",false); jar.SetBool("begin",false); boss.SetBool("tailWag",false); break;
	case GameStates.Game: 	cash.SetBool("Cash",true); 	jar.SetBool("begin",true);  boss.SetBool("tailWag",true) ;break;
}

	}
}

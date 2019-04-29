using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hammerplay.Utils;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public static bool gameOver = false;

    public bool gameStarted = false;


    public static GameStates currentGameState;

    [SerializeField]
    public Text corporateScoreText, playerScoreText, menuPlayerScore, menuCorporateScore;

    public int corporateScore = 0, playerScore = 0;

    [SerializeField]
    float scoreTimer = 0 ;

	[SerializeField]
	Image startPanel, retryPanel, scorePanel;


[SerializeField]
Button play, retry;


    public static GameManager instance;
    void Awake()
    {
		CurrentGameState = GameStates.Menu;
        instance = this;
    }
    public static GameStates CurrentGameState
    {
        get
        {
            return currentGameState;
        }
        set
        {
            currentGameState = value;
            OnGameStateChange(currentGameState);
        }
    }
    // Use this for initialization
  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
 

    // Update is called once per frame
    void Update()
    {

        

           
        
        if (Time.time - scoreTimer > .3f && CurrentGameState == GameStates.Game)
        {

            corporateScore += Random.Range(40, 50);
            corporateScoreText.text = corporateScore.ToString();
            scoreTimer = Time.time;
        }



    }

    public static void PlayerScoreUpdate(int score)
    {
        instance.playerScore += score;
        instance.playerScoreText.text = instance.playerScore.ToString();
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        OnGameStateChange += OngameStateChange;
		play.onClick.AddListener(OnButtonClick );
		retry.onClick.AddListener(OnButtonClick);

    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        OnGameStateChange -= OngameStateChange;
		play.onClick.RemoveAllListeners();
		retry.onClick.RemoveAllListeners();
    }
    void OngameStateChange(GameStates gamestate)
    {
        switch (gamestate)
        {
            case GameStates.GameStart:
                PoolManager.ClearAllOnScreen();
				corporateScore = playerScore = 0;
                corporateScoreText.text = corporateScore.ToString();
                playerScoreText.text = playerScore.ToString();
                CurrentGameState = GameStates.Game;
				break;
            case GameStates.GameOver:
              Invoke("ChangeMenuState",3.5f);
				break;
            case GameStates.Menu:
                break;

        }



    }

	void OnButtonClick(){
		startPanel.gameObject.SetActive(false);
		retryPanel.gameObject.SetActive(false);
       CurrentGameState = GameStates.GameStart;
	   scorePanel.gameObject.SetActive(true);
	}

void ChangeMenuState(){
	retryPanel.gameObject.SetActive(true); 
    scorePanel.gameObject.SetActive(false);
				menuCorporateScore.text = corporateScore.ToString();
				menuPlayerScore.text = playerScore.ToString();
				CurrentGameState=GameStates.Menu;
}


    public delegate void OnGameStateChangeHandler(GameStates gamestate);
    public static OnGameStateChangeHandler OnGameStateChange;
}
public enum GameStates
{
    GameStart, GameOver, Menu, PowerUp, Game
}

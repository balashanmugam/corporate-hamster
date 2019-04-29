using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hammerplay.Utils;
public class PlayerInput : MonoBehaviour
{
    public enum PlayerState { Idle, Run, Jump, Crouch ,RunOnPlatform};

    bool toCrouch;

    [SerializeField]
    private float jumpForce, duckForce;

    private Collider2D[] colliders;

    private Animator animator;

    private Rigidbody2D _rb;

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    ParticleSystem coins;

    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audio;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GameManager.CurrentGameState == GameStates.Game)
        {
            if (playerState == PlayerState.Run)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerState = PlayerState.Jump;
                    _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    animator.SetTrigger("Jump");
                    audio.clip = audioClips[0];
                    audio.Play();
                    animator.SetBool("Run", false);

                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    // crouch
                    playerState = PlayerState.Crouch;
                    audio.clip = audioClips[1];
                    audio.Play();
                    animator.SetTrigger("Duck");
                }

            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (playerState == PlayerState.Jump)
                {
                    toCrouch = true;
                    _rb.AddForce(new Vector2(0, -duckForce), ForceMode2D.Impulse);
                    animator.SetBool("Run", true);
                }


            }


        }

        if (Input.GetKeyUp(KeyCode.C))
        {

            if (playerState == PlayerState.Crouch)
            {
                animator.SetTrigger("Up");
                if(GameManager.currentGameState == GameStates.Game)
                {
                    playerState = PlayerState.Run;
                    animator.SetBool("Run", true);
                }
            }
        }
    }
    void GetUp()
    {

        if (!Input.GetKey(KeyCode.C))
        {
            animator.SetTrigger("Up"); animator.SetBool("Run", true);
            playerState = PlayerState.Run;
        }
    }
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (GameManager.CurrentGameState == GameStates.Game)
        {
            if (other.gameObject.GetComponent<SpriteRenderer>() == null  )
            {
if(playerState == PlayerState.Jump){
                if (toCrouch)
                {
                    animator.SetTrigger("Duck");
                    audio.clip = audioClips[1];
                    audio.Play();
                    playerState = PlayerState.Crouch;
                    Invoke("GetUp", 0.03f);
                    toCrouch = false;

                }
                else
                {
               
                    animator.SetBool("Run", true);
                    playerState = PlayerState.Run;
                }

}
else if(playerState == PlayerState.RunOnPlatform){
      animator.SetBool("Run", true);
                    playerState = PlayerState.Run;
}
            }

            if (other.gameObject.GetComponent<Obstacle>())
            {
                audio.clip = audioClips[3];
                audio.Play();
                GameManager.CurrentGameState = GameStates.GameOver;
                GameManager.CurrentGameState = GameStates.Menu;
            }
            if (other.gameObject.GetComponent<Vacation>())
            {
                audio.clip = audioClips[4];
                audio.Play();
                other.gameObject.SetActive(false);
                GameManager.CurrentGameState = GameStates.PowerUp;
                animator.SetBool("Vacation",true);
                PoolManager.ClearAllOnScreen();
                StartCoroutine(returnFromVacation());
            }

            if (other.gameObject.GetComponent<Salary>())
            {
                audio.clip = audioClips[2];
                audio.Play();
                other.gameObject.SetActive(false);
                GameManager.PlayerScoreUpdate((8));
                coins.Play();
            }

            if (other.gameObject.GetComponent<Entity>())
            {

                if (other.gameObject.GetComponent<Entity>().type == EntityType.Platform && other.relativeVelocity.y > 0)
                {
                    playerState = PlayerState.RunOnPlatform;
                    animator.SetBool("Run", true);

                }


                else if (other.gameObject.GetComponent<Entity>().type == EntityType.Platform)
                {

                    GameManager.CurrentGameState = GameStates.GameOver;

                }
            }
        }
    }

    void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    /// </summary>
    void OnDisable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }
    void OnGameStateChange(GameStates gamestate)
    {
        switch (gamestate)
        {

            case GameStates.GameOver: animator.SetTrigger("Death");animator.SetBool("Run", false); break;

            case GameStates.GameStart: animator.ResetTrigger("Up");break;

            case GameStates.Game: animator.SetBool("Run", true); playerState = PlayerState.Run; break;
        }

    }

    IEnumerator returnFromVacation()
    {
        yield return new WaitForSeconds(2.5f);
        GameManager.CurrentGameState = GameStates.Game;
        animator.SetBool("Vacation",false);
    }
}

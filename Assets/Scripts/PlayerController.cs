using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    #region Variables

    // Global variables
    public static bool hasWeapon = false;
    public static int weaponDamage = 0;
    public static string weaponType = "";

    // Movement
    private Vector2 direction;
    public Vector2 _direction
    {
        get { return direction; }
        set { direction = value; }
    }

    // Weapons Controller
    private int weaponRemainingShots = 0;

    public int _weaponRemainingShots
    {
        get { return weaponRemainingShots; }
        set { weaponRemainingShots = value; }
    }

    // Scripts
    private GameController gameController;

    // Player's weapon
    [SerializeField]private SpriteRenderer spriteRenderer;

    // Movement
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float playerSpeed;

    // Components
    private Rigidbody2D playerRB;
    private Animator playerAnimator;

    // Circle of light
    //[SerializeField] private GameObject lightCircle;
    [SerializeField] private List<Sprite> circleLevelList = new List<Sprite>();
    [SerializeField] private SpriteRenderer circleSprite;
    private int playerLife = 5;

    // Audio
    // playerSource = Looped audio || playerSoundEffects = Not Looped audio
    [SerializeField] private AudioSource playerSource, playerSoundEffects;
    [SerializeField] private List<AudioClip> playerSounds = new List<AudioClip>();

    // Actions
    private string playerAction = "idle";
    public string _playerAction
    {
        get { return playerAction; }
        set { playerAction = value; }
    }
    private string lastAction = "";

    private bool playerIsAttacking = false;
    public bool _playerIsAttacking
    {
        get { return playerIsAttacking; }
        set { playerIsAttacking = value; }
    }
    public static bool playerIsDead = false;

    #endregion

    #region MainMethods

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerIsDead = false;
       
    }

    void Update()
    {
        // If game is started
        if (GameController.GameIsOn && !playerIsDead)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Rotation();
            Animation();
            Audio();
            CheckCircleLevel(playerLife);
            if (weaponRemainingShots <= 0 && hasWeapon)
            {
                DestroyWeapon();
            }
        }
    }

    void FixedUpdate()
    {
        // Move the player with the joystick
        if(GameController.GameIsOn && !playerIsDead) playerRB.velocity = direction * playerSpeed;

    }

    #endregion

    #region CollisionsHandler

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player collides with a weapon
        if (collision.gameObject.tag == "Weapon")
        {
            // The player has a weapon
            hasWeapon = true;
            // Getting the attributes of the corresponding weapon and keeping them in variables
            weaponDamage = collision.gameObject.GetComponent<WeaponController>().weapon.damage;
            weaponType = collision.gameObject.GetComponent<WeaponController>().weapon.name;
            weaponRemainingShots = collision.gameObject.GetComponent<WeaponController>().weapon.shotsCount;
            Debug.Log(weaponType + " - " + weaponRemainingShots + " shots!");
            spriteRenderer.sprite = collision.gameObject.GetComponent<WeaponController>().weapon.sprite;
            // Destroy the weapon object that the player collided with
            Destroy(collision.gameObject);
        }

        // If player collides with a candle
        if (collision.gameObject.tag == "Candle")
        {
            // If player's life is less than 5
            if (playerLife < 5)
            {
                // Increase player's life by 1 and the lightCircle scale by 0.2 in all coordinates
                playerLife++;
                //lightCircle.transform.localScale = new Vector3(lightCircle.transform.localScale.x + 0.2f, lightCircle.transform.localScale.y + 0.2f, lightCircle.transform.localScale.z + 0.2f);
            }
            // Destroy the candle object that the player collided with
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player collides with an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // If player's life is greater than 1
            if(playerLife > 1)
            {
                // Play "Hurt" sound effect
                playerSoundEffects.clip = playerSounds[1];
                playerSoundEffects.Play();

                // Decrease player's life by 1 and lightCircle scale by 0.2 in all coordinates
                playerLife--;
                //lightCircle.transform.localScale = new Vector3(lightCircle.transform.localScale.x - 0.2f, lightCircle.transform.localScale.y - 0.2f, lightCircle.transform.localScale.z - 0.2f);
            }
            // Otherwise if the player's life less than 1
            else
            {
                StartCoroutine("Death");
                //Death();
                
            }
        }   
    }

    #endregion

    #region MethodsInUpdate

    // Handling player rotation
    void Rotation()
    {
        // If the joystick is pointing to the right
        if (direction.x > 0)
        {
            // Rotate the player 180 degrees in y
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Otherwise if the joystick is pointing to the left
        else if (direction.x < 0)
        {
            // Return player to their starting rotation
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    // Handling player animation
    void Animation()
    {
        if (!playerIsAttacking && !playerIsDead)
        {
            // If the joystick is pointing in any direction
            if (direction.x != 0 || direction.y != 0)
            {
                // Play "Walk" animation and set playerAction to "walking"
                playerAnimator.SetInteger("transition", 2);
                playerAction = "walking";
            }
            // Otherwise if the joystick is stopped
            else
            {
                // Play "Idle" animation and set playerAction to "idle"
                playerAnimator.SetInteger("transition", 1);
                playerAction = "idle";
            }
        }
    }

    // Handling player audio
    void Audio()
    {
        // If the help variable is different from playerAction
        if (lastAction != playerAction)
        {
            // Help variable gets playerAction
            lastAction = playerAction;
            
            switch (playerAction)
            {
                case "idle":
                    // Play the audio corresponding to the "idle" action
                    playerSource.clip = playerSounds[2];
                    playerSource.Play();
                    break;

                case "walking":
                    // Play the audio corresponding to the "walking" action
                    playerSource.clip = playerSounds[0];
                    playerSource.Play();
                    break;
            }
        }
    }

    #endregion

    #region WeaponsHandler

    // Function to destroy player's current weapon
    void DestroyWeapon()
    {
        // Setting the player's weapon sprite to null
        hasWeapon = false;
        weaponRemainingShots = 0;
        spriteRenderer.sprite = null;
    }

    #endregion

    void CheckCircleLevel(int life)
    {
        switch (life)
        {
            case 5:
                circleSprite.sprite = circleLevelList[0];
                break;
            case 4:
                circleSprite.sprite = circleLevelList[1];
                break;
            case 3:
                circleSprite.sprite = circleLevelList[2];
                break;
            case 2:
                circleSprite.sprite = circleLevelList[3];
                break;
            case 1:
                circleSprite.sprite = circleLevelList[4];
                break;
        }
    }

    public IEnumerator Attacking()
    {
        if (!playerIsDead) 
        { 
            playerIsAttacking = true;
            playerAnimator.SetInteger("transition", 3);
            yield return new WaitForSeconds(1f);
            playerIsAttacking = false;
        }
    }

    IEnumerator Death()
    {
        playerIsDead = true;
        playerRB.velocity = new Vector2(0,0);
        // Playing Dead audio
        playerSoundEffects.clip = playerSounds[3];
        playerSoundEffects.Play();
        // Playing animation
        playerAnimator.SetInteger("transition", 4);
        yield return new WaitForSeconds(1.2f);
        // Call "GameOver" method from GameController
        GameController.GameIsOn = false;
        gameController.GameOver();
    }
}

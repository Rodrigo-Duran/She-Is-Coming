using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables

    // Global variables
    public static bool hasWeapon = false;
    public static int weaponDamage = 0;
    public static string weaponType = "";

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
    [SerializeField] private GameObject lightCircle;
    private int playerLife = 6;

    // Audio
    // playerSource = Looped audio || playerSoundEffects = Not Looped audio
    [SerializeField] private AudioSource playerSource, playerSoundEffects;
    [SerializeField] private List<AudioClip> playerSounds = new List<AudioClip>();
    private string playerAction = "idle";
    private string apoio = "";

    #endregion

    #region MainMethods

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
       
    }

    void Update()
    {
        // If game is started
        if (GameController.GameIsOn)
        {
            Rotation();
            Animation();
            Audio();
        }
    }

    void FixedUpdate()
    {
        // Move the player with the joystick
        if(GameController.GameIsOn) playerRB.velocity = new Vector2(joystick.Horizontal * playerSpeed, joystick.Vertical * playerSpeed);

    }

    #endregion

    #region CollisionsHandler

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player collides with a weapon
        if (collision.gameObject.tag == "Weapon")
        {
            // Stoping the coroutine to destroy the weapon, in case player already has another weapon
            StopCoroutine("DestroyWeapon");
            // The player has a weapon
            hasWeapon = true;
            // Getting the attributes of the corresponding weapon and keeping them in variables
            weaponDamage = collision.gameObject.GetComponent<WeaponController>().weapon.damage;
            weaponType = collision.gameObject.GetComponent<WeaponController>().weapon.name;
            spriteRenderer.sprite = collision.gameObject.GetComponent<WeaponController>().weapon.sprite;
            //Debug.Log("Nova Arma: " + collision.gameObject.GetComponent<WeaponController>().weapon.name);
            // Starting the coroutine to destroy the weapon, informing in the parameter the time the weapon has until it is destroyed
            StartCoroutine(DestroyWeapon(collision.gameObject.GetComponent<WeaponController>().weapon.timeToExpire));
            // Destroy the weapon object that the player collided with
            Destroy(collision.gameObject);
        }

        // If player collides with a candle
        if (collision.gameObject.tag == "Candle")
        {
            // If player's life is less than 6
            if (playerLife < 6)
            {
                // Increase player's life by 1 and the lightCircle scale by 0.2 in all coordinates
                playerLife++;
                lightCircle.transform.localScale = new Vector3(lightCircle.transform.localScale.x + 0.2f, lightCircle.transform.localScale.y + 0.2f, lightCircle.transform.localScale.z + 0.2f);
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
                lightCircle.transform.localScale = new Vector3(lightCircle.transform.localScale.x - 0.2f, lightCircle.transform.localScale.y - 0.2f, lightCircle.transform.localScale.z - 0.2f);
            }
            // Otherwise if the player's life less than 1
            else
            {
                // Playing Dead audio
                playerSoundEffects.clip = playerSounds[3];
                playerSoundEffects.Play();
                // Call "GameOver" method from GameController
                gameController.GameOver();
            }
        }   
    }

    #endregion

    #region MethodsInUpdate

    // Handling player rotation
    void Rotation()
    {
        // If the joystick is pointing to the right
        if (joystick.Horizontal > 0)
        {
            // Rotate the player 180 degrees in y
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Otherwise if the joystick is pointing to the left
        else if (joystick.Horizontal < 0)
        {
            // Return player to their starting rotation
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    // Handling player animation
    void Animation()
    {
        // If the joystick is pointing in any direction
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
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

    // Handling player audio
    void Audio()
    {
        // If the help variable is different from playerAction
        if (apoio != playerAction)
        {
            // Help variable gets playerAction
            apoio = playerAction;
            
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

    // Coroutine to destroy player's current weapon
    IEnumerator DestroyWeapon(float time)
    {
        // Wait for the corresponding weapon time
        yield return new WaitForSeconds(time);
        // Setting the player's weapon sprite to null
        spriteRenderer.sprite = null;
        hasWeapon = false;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    #region Variables

    // Bullets
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] List<GameObject> bulletsList = new List<GameObject>();
    [SerializeField] Transform bulletTransform;

    // Firing
    private bool canFire;
    private float timer;
    private float timeBetweenFiring;

    // Audio
    [SerializeField] private AudioSource playerSoundEffects;
    [SerializeField] private AudioClip playerAttack;

    #endregion

    #region MainMethods

    //Awake
    void Awake()
    {
        // Player can fire
        canFire = true;

        // Setting the parameters between fires
        timer = 0f;
        timeBetweenFiring = 0.5f;
    }

    // Update
    void Update()
    {
        // If game is started
        if (GameController.GameIsOn)
        {
            InUpdate();
        }
    }

    #endregion

    #region FireHandler

    // Method to fire (Called by the attack button)
    public void Fire()
    {
        // If player can fire and has a weapon
        if (canFire && PlayerController.hasWeapon)
        {
            // Player can't fire
            canFire = false;
            // Playing sound effect
            playerSoundEffects.clip = playerAttack;
            playerSoundEffects.Play();
            // Checking the weapon type and instantiating the corresponding bullet
            switch (PlayerController.weaponType) {
                case "Faca":
                    //Debug.Log("FACA");
                    Instantiate(bulletsList[0], bulletTransform.position, Quaternion.identity);
                    break;
                case "Galho":
                    //Debug.Log("GALHO");
                    Instantiate(bulletsList[1], bulletTransform.position, Quaternion.identity);
                    break;
                case "Tijolo":
                    //Debug.Log("TIJOLO");
                    Instantiate(bulletsList[2], bulletTransform.position, Quaternion.identity);
                    break;
            }
        }
    }

    #endregion

    #region MethodsInUpdate

    void InUpdate()
    {
        // The target is following the joystick movement
        Vector3 rotation = new Vector3(joystick.Horizontal, joystick.Vertical);

        float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationInZ);

        // If player can't fire, in other words, if player just fired a shot
        if (!canFire)
        {
            // Timer is increasing
            timer += Time.deltaTime;
            // When timer equals the "TimeBetweenFiring" value
            if (timer > timeBetweenFiring)
            {
                // Player can fire again
                canFire = true;
                // Timer is set to 0
                timer = 0f;
            }

        }
    }

    #endregion

}

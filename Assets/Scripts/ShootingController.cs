using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    #region Variables

    // Rotation
    [SerializeField] GameObject player;

    // Bullets
    private Vector3 mousePosition;
    private Camera mainCamera;
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
        timeBetweenFiring = 1.2f;

        // Camera
        mainCamera = FindFirstObjectByType<Camera>();
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
    public IEnumerator Fire()
    {
        // If player can fire and has a weapon
        if (canFire && PlayerController.hasWeapon)
        {
            // Player can't fire
            canFire = false;
            // Play animation
            StartCoroutine(player.GetComponent<PlayerController>().Attacking());
            yield return new WaitForSeconds(0.8f);
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
                case "Shuriken":
                    //Debug.Log("TIJOLO");
                    Instantiate(bulletsList[3], bulletTransform.position, Quaternion.identity);
                    break;
                case "Machado":
                    //Debug.Log("TIJOLO");
                    Instantiate(bulletsList[4], bulletTransform.position, Quaternion.identity);
                    break;
                case "Chinelo":
                    //Debug.Log("TIJOLO");
                    Instantiate(bulletsList[5], bulletTransform.position, Quaternion.identity);
                    break;
            }
            // Playing sound effect
            playerSoundEffects.clip = playerAttack;
            playerSoundEffects.Play();
            // Number of remaining shots decreased
            player.GetComponent<PlayerController>()._weaponRemainingShots--;
            Debug.Log("Remaining shots: " + player.GetComponent<PlayerController>()._weaponRemainingShots);
        }
    }

    #endregion

    #region MethodsInUpdate

    void InUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Fire());
        }

        // If the player is idle
        if (player.GetComponent<PlayerController>()._playerAction == "idle")
        {
            // Check his rotation, if he is pointing to the right, rotate the target to the right
            if (player.transform.rotation.y == 180) transform.rotation = Quaternion.Euler(0, 0, 0);
            // If he is pointing to the left, rotate the target to the left
            else if (player.transform.rotation.y == 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // If the player is moving, rotate the target by the player movement
        else
        {
            // Pegando a posição do mouse através do input do mouse na camera
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = transform.position - mousePosition;
            float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationInZ);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    #region Variables

    // NavMesh
    private Transform target;
    private NavMeshAgent agent;

    // Components
    private int enemyLife;
    private PolygonCollider2D enemyCollider;

    // Enemy HUD
    private HealthBarController healthBarController;

    // Audio
    private AudioSource enemyHurtSource;
    private AudioSource enemyDeathSource;

    // Help variables
    private bool isDead = false;

    #endregion

    #region MainMethods

    void Awake()
    {
        // NavMesh
        // Set player as enemy target
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Components
        enemyCollider = GetComponent<PolygonCollider2D>();
        enemyLife = GameController.enemyBasicLife;

        // HUD
        healthBarController = GetComponentInChildren<HealthBarController>();

        // Audio
        enemyHurtSource = GameObject.FindGameObjectWithTag("EnemyHurtSource").GetComponent<AudioSource>();
        enemyDeathSource = GameObject.FindGameObjectWithTag("EnemyDeathSource").GetComponent<AudioSource>();


        Debug.Log("START - Enemy Life: " + GameController.enemyBasicLife);
    }

    void Update()
    {
        // Tell the enemy to follow the target
        agent.SetDestination(target.position);

        // If enemy is dead
        if (enemyLife <= 0 && !isDead)
        {
            // Locking the method
            isDead = true;
            // Playing Death audio
            enemyDeathSource.Play();
            // Decrease the number of living enemies in the scene and destroy this enemy object
            GameController.numberOfEnemies--;
            Destroy(gameObject);
            Debug.Log("Number of enemies Alive: " + GameController.numberOfEnemies);
        }
    }

    #endregion

    #region CollisionsHandler

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collides with the player
        if (collision.gameObject.tag == "Player")
        {
            // Decrease the number of living enemies in the scene and destroy this enemy object
            GameController.numberOfEnemies--;
            Destroy(gameObject);
            Debug.Log("Number of enemies Alive: " + GameController.numberOfEnemies);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the enemy is colliding with another enemy or the environment collider
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnvironmentCollider")
        {
            // Ignore this collision
            Physics2D.IgnoreCollision(enemyCollider, collision.gameObject.GetComponent<PolygonCollider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the enemy collides with a bullet
        if (collision.gameObject.tag == "Bullet")
        {
            // Playing Hurt audio
            enemyHurtSource.Play();
            // Decrease enemy health with weapon damage value and update the enemy health bar
            enemyLife -= PlayerController.weaponDamage;
            healthBarController.UpdateHealthBarValue(enemyLife, GameController.enemyBasicLife);
            Debug.Log("EnemyLife: " + enemyLife);
        }
    }

    #endregion
}

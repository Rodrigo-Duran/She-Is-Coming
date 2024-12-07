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
    private Animator animator;

    // Enemy HUD
    private HealthBarController healthBarController;

    // Audio
    [SerializeField]private AudioSource enemyHurtSource;
    [SerializeField]private AudioSource enemyDeathSource;

    // Help variables
    private bool isDead = false;
    [SerializeField] private int ghostType;

    // Raycast
    /*private float raycastRange = 0.2f;
    private bool isSeeingPlayer = false;*/

    // Force after shots
    private float backForce = 2f;

    // Movement
    private Vector2 initalPosition;

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
        animator = GetComponent<Animator>();

        // HUD
        healthBarController = GetComponentInChildren<HealthBarController>();

        // Audio
        //enemyHurtSource = GameObject.FindGameObjectWithTag("EnemyHurtSource").GetComponent<AudioSource>();
        //enemyDeathSource = GameObject.FindGameObjectWithTag("EnemyDeathSource").GetComponent<AudioSource>();

        initalPosition = gameObject.transform.position;
        animator.SetInteger("transition", 1);
        Debug.Log("START - Enemy Life: " + GameController.enemyBasicLife);
    }

    void Update()
    {
        // Tell the enemy to follow the target
        if(GameController.GameIsOn && !PlayerController.playerIsDead)agent.SetDestination(target.position);

       /* // FAZER RAYCAST PARA VER O PLAYER E MUDAR VELOCIDADE
        Vector3 direction = target.position - transform.position;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.TransformDirection(direction * raycastRange));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * raycastRange));
        if (ray.collider != null)
        {
            isSeeingPlayer = ray.collider.CompareTag("Player");
            if (isSeeingPlayer)
            {
                Debug.Log("VIU O PLAYER");
            }
        }*/

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

        if (PlayerController.playerIsDead)
        {
            agent.SetDestination(transform.position);
        }

        if (agent.velocity.x > 0)
        {
            if (ghostType == 1) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if(ghostType == 2) transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if(ghostType == 1) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if(ghostType == 2) transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    #endregion

    #region CollisionsHandler

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Decrease the number of living enemies in the scene and destroy this enemy object
            //GameController.numberOfEnemies--;
            //Destroy(gameObject);
            //Debug.Log("Number of enemies Alive: " + GameController.numberOfEnemies);
            transform.position = initalPosition;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the enemy is colliding with another enemy or the environment collider
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag ("EnvironmentCollider"))
        {
            // Ignore this collision
            Physics2D.IgnoreCollision(enemyCollider, collision.gameObject.GetComponent<PolygonCollider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the enemy collides with a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Playing Hurt audio
            enemyHurtSource.Play();
            // Decrease enemy health with weapon damage value and update the enemy health bar
            enemyLife -= PlayerController.weaponDamage;
            healthBarController.UpdateHealthBarValue(enemyLife, GameController.enemyBasicLife);
            // COLOCAR FORÇA PRA TRAS
            Vector3 forceAfterShot = new Vector3(-agent.velocity.x * backForce, -agent.velocity.y * backForce, 0f);
            agent.velocity = forceAfterShot;
            Debug.Log("EnemyLife: " + enemyLife);
        }
    }

    #endregion
}

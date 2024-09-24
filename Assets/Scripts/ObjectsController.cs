using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{

    #region Variables

    // Components
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;

    // Player
    [SerializeField] private Transform joao, maria;
    private Transform player;

    #endregion

    #region MainMethods

    private void Awake()
    {
        // Components
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Getting the corresponding player
        if (GameController.characterToPlay == 1) player = joao;
        else if (GameController.characterToPlay == 2) player = maria;
    }

    #endregion

    #region CollisionsHandler

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the object is colliding with the player
        if (collision.gameObject.tag == "Player")
        {
            // If the player is below the object's pivot point
            if (player.position.y < transform.position.y)
            {
                // The object's sprite order in layer is set to 0
                spriteRenderer.sortingOrder = 0;
            }
            // If the player is above the object's pivot point
            else
            {
                // The object's sprite order in layer is set to 17
                spriteRenderer.sortingOrder = 17;
            }
        }

        // The same for enemies
        if (collision.gameObject.tag == "Enemy")
        {
            // Getting the enemy's object to access its transform
            GameObject enemy = collision.gameObject;

            if (enemy.transform.position.y < transform.position.y)
            {
                spriteRenderer.sortingOrder = 0;
            }
            else
            {
                spriteRenderer.sortingOrder = 17;
            }
        }
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{

    #region Variables

    // BulletMovement
    private Transform bulletDirection;
    private float force;
    private string directionToRotate;

    // Components
    private Rigidbody2D rb;

    #endregion

    #region MainMethods

    void Awake()
    {
        bulletDirection = GameObject.FindGameObjectWithTag("BulletDirection").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        force = 12f;

        Vector3 direction = bulletDirection.position - transform.position;
        Vector3 rotation = transform.position - bulletDirection.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        if (direction.x > 0) directionToRotate = "right";
        else directionToRotate = "left";

        float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationInZ + 90);

        
    }

    void Update()
    {
        if(directionToRotate == "right") transform.Rotate(0f, 0f, -50f * Time.deltaTime);
        else if(directionToRotate == "left") transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    #endregion

    #region CollisionsHandler

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Candle") && !collision.gameObject.CompareTag("Weapon") /*&& !collision.gameObject.CompareTag("PlayerLightCircle")*/) Destroy(gameObject);
    }

    #endregion

    
}

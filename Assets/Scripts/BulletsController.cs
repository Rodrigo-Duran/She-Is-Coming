using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{

    #region Variables

    // BulletMovement
    private Transform bulletDirection;
    private float force;

    // Components
    private Rigidbody2D rb;

    #endregion

    #region MainMethods

    void Awake()
    {
        bulletDirection = GameObject.FindGameObjectWithTag("BulletDirection").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        force = 8f;

        Vector3 direction = bulletDirection.position - transform.position;
        Vector3 rotation = transform.position - bulletDirection.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;


        float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationInZ + 90);

        
    }

    void Update()
    {
        transform.Rotate(0f, 0f, 50f * Time.deltaTime);
    }

    #endregion

    #region CollisionsHandler

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Candle" && collision.gameObject.tag != "Weapon") Destroy(gameObject);
    }

    #endregion

    
}

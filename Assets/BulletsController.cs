using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{

    private Transform bulletDirection;
    private Rigidbody2D rb;
    private float force;

    // Start is called before the first frame update
    void Start()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

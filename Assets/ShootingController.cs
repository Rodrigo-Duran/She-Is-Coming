using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private FixedJoystick joystick;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    private bool canFire;
    private float timer;
    private float timeBetweenFiring;

    //Awake
    void Awake()
    {
        canFire = true;
        timer = 0f;
        timeBetweenFiring = 0.5f;
    }

    // Update
    void Update()
    {
        Vector3 rotation = new Vector3(joystick.Horizontal, joystick.Vertical);

        float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationInZ);
    
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0f;
            }

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canFire)
            {
                canFire = false;
                Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            }
        }

    }

    //Fire
    public void Fire()
    {
        if (canFire)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        }
    }

}

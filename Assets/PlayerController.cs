using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float playerSpeed;
    private Rigidbody2D playerRB;
    private Animator playerAnimator;

    // Awake
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerAnimator.SetInteger("transition", 1);

            if (joystick.Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (joystick.Horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            playerAnimator.SetInteger("transition", 0);
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(joystick.Horizontal * playerSpeed, joystick.Vertical * playerSpeed);

    }
}

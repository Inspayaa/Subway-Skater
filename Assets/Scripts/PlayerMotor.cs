using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    private const float TURN_SPEED = 0.05f;

    //Animation
    private Animator anim;

    //Movement
    private CharacterController controller;
    private float jumpForce = 4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private float speed = 7.0f;
    private int desiredLane = 1; // 0 = Left 1 = middle 2 = right 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Gather the input on which lane we should be
        if (MobileInput.instance.SwipeLeft)
        {
            MoveLane(false);
        }
           

        if (MobileInput.instance.SwipeRight)
        {
            MoveLane(true);
        }
            


       // Calculate what position where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;

        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;


       // Calculate the move delta/vector
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        // Calculate Y
        if (isGrounded)//if Gounded
        {
            verticalVelocity = -0.1f;

            if (MobileInput.instance.SwipeUp)
                {
                    //Jump
                    anim.SetTrigger("Jump");
                    verticalVelocity = jumpForce;
                }
        }
        else
        {
            // Gradually reduce and introduce gravity with time
            verticalVelocity -= (gravity * Time.deltaTime);

            //Fast falling mechanic
            if (MobileInput.instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //Move the Pingu
        
         controller.Move(moveVector * Time.deltaTime);

        // Rotate the Pingu towards where he is going
        Vector3 dir = controller.velocity;

        if (dir != Vector3.zero)
        {
            dir.y = 0; // To prevent y rotation when jumping
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
        
    }
    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
           
    }


}

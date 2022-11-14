using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovementController : MonoBehaviour
{
    public Joystick joystick;
    public FixedTouchField fixedTouchField;

    private RigidbodyFirstPersonController rigidBodyFirstPersonController;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodyFirstPersonController = this.GetComponent<RigidbodyFirstPersonController>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rigidBodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
        rigidBodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;
        rigidBodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;

        animator.SetFloat("horizontal", joystick.Horizontal);
        animator.SetFloat("vertical", joystick.Vertical);

        if(Mathf.Abs(joystick.Horizontal) > 0.9 || Mathf.Abs(joystick.Vertical) > 0.9)
        {
            animator.SetBool("isRunning", true);
            rigidBodyFirstPersonController.movementSettings.ForwardSpeed = 10;
        }

        else
        {
            animator.SetBool("isRunning", false);
            rigidBodyFirstPersonController.movementSettings.ForwardSpeed = 5;
        }
    }
}

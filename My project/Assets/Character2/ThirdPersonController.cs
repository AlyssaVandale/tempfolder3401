using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //Input Variables
    private Vector2 move_input;

    //Camera Variables
    public Transform camera;

    //Player Variables
    public Rigidbody rigidbody; //Our player game objects.
    public Transform player;
    public Transform player_model;
    public Transform orientation;
    public float move_force; //Force applied to the player to make it move.
    public float rotation_speed; //How fast the model rotates towards the move_direction.
    private Vector3 direction; //Which way the player will face and move.
    public float jump_force = 600f;

    //Raycast Variables
    private float ray_length = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayerModel();

        //Shows placement of raycast.
        Debug.DrawRay(transform.position, Vector3.down * ray_length, Color.red);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //Capture Input
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
        //Debug.Log(move_input);
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        //Only Jump when the key is first pressed.
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void RotatePlayerModel()
    {
        //Get what direction to face.
        var cam_position = new Vector3(camera.position.x, player.position.y, camera.position.z);
        Vector3 view_direction = player.position - cam_position;

        //Point the orientation in the direction of the view_direction.
        orientation.forward = view_direction;

        //Set the direction.
        direction = orientation.right * move_input.x + orientation.forward * move_input.y;
        direction = direction.normalized;
    
        //If pressing a key...
        if(move_input != Vector2.zero)
        {
            //Create the new rotation we want the player model to look at.
            Quaternion new_rotation = Quaternion.LookRotation(direction, Vector3.up);

            //Have the player model rotation slowly move towards the new_rotation.
            player_model.rotation = Quaternion.Slerp(player_model.rotation, new_rotation, rotation_speed * Time.deltaTime); 
        }
    }

    public void MovePlayer()
    {
        rigidbody.AddForce(direction * move_force, ForceMode.Force);
    }

    public void Jump()
    {
        if(IsOnGround())
        {
            rigidbody.AddForce(Vector3.up * jump_force);
        }
    }

    bool IsOnGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, ray_length);
    }
}

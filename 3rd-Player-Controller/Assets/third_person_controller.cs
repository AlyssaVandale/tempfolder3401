using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class third_person_controller : MonoBehaviour
{
    private Vector2 input_vector;
    // Start is called before the first frame update

    //Cmera variables
    public Transform camera;

    //Player Varaibles
    public Rigidbody rigidbody;
    public Transform Player;
    public Transform player_model;
    public Transform orientation;

    public float move_force;
    public float rotation_speed;
    public Vector3 direction;

    public float jump_force = 600;

    public float ray_length = 1.1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayerModel();
        Debug.DrawRay(transform.position, Vector3.down * ray_length, Color.blue);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        input_vector = context.ReadValue<Vector2>();
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void RotatePlayerModel()
    {
        Vector3 view_direction = Player.position - new Vector3(camera.position.x, Player.position.y, camera.position.z);

        orientation.forward = view_direction;

        direction = orientation.right * input_vector.x + orientation.forward * input_vector.y;
        direction = direction.normalized;

        if(input_vector != Vector2.zero)
        {
            Quaternion new_rotation = Quaternion.LookRotation(direction, Vector3.up);

            player_model.rotation = Quaternion.Slerp(player_model.rotation , new_rotation, Time.deltaTime * rotation_speed);
        }
    }

    public void MovePlayer()
    {
        rigidbody.AddForce(direction * move_force, ForceMode.Force);
    }

    public void Jump()
    {
        if (isOnGround())
        {
            rigidbody.AddForce(Vector3.up * jump_force);
        }
       

    }

    bool isOnGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, ray_length);
    }
}

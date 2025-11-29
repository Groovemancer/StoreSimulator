using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;

    public InputActionReference jumpAction;

    public CharacterController charCon;

    public float moveSpeed;

    public float jumpForce;

    private float ySpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        //transform.position = transform.position + new Vector3(moveInput.x * Time.deltaTime * moveSpeed, 0f, moveInput.y * Time.deltaTime * moveSpeed);

        Vector3 moveAmount = new Vector3(moveInput.x, 0, moveInput.y);

        moveAmount = moveAmount * moveSpeed;

        if (charCon.isGrounded == true)
        {
            ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame())
            {
                ySpeed = jumpForce;
            }
        }

        ySpeed = ySpeed + (Physics.gravity.y * Time.deltaTime);

        moveAmount.y = ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);
    }
}

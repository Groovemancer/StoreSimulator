using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;

    public InputActionReference jumpAction;

    public InputActionReference lookAction;

    public InputActionReference pickupAction;
    public InputActionReference releaseAction;
    public InputActionReference takeItemAction;

    public CharacterController charCon;

    public float moveSpeed;

    public float jumpForce;

    public float lookSpeed;

    public float minLookAngle, maxLookAngle;

    public Camera theCam;

    public LayerMask whatIsStock;
    public LayerMask whatIsShelf;

    public float interactionRange;

    public Transform holdPoint;

    public float throwForce;

    private StockObject heldPickup;

    private float ySpeed;
    private float horiRot, vertRot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        horiRot += lookInput.x * lookSpeed * Time.deltaTime;
        vertRot -= lookInput.y * lookSpeed * Time.deltaTime;

        vertRot = Mathf.Clamp(vertRot, minLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(0, horiRot, 0f);
        theCam.transform.localRotation = Quaternion.Euler(vertRot, 0f, 0f);



        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        //transform.position = transform.position + new Vector3(moveInput.x * Time.deltaTime * moveSpeed, 0f, moveInput.y * Time.deltaTime * moveSpeed);

        //Vector3 moveAmount = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 vertMove = transform.forward * moveInput.y;

        Vector3 horiMove = transform.right * moveInput.x;

        Vector3 moveAmount = horiMove + vertMove;
        moveAmount = moveAmount.normalized;

        moveAmount = moveAmount * moveSpeed;

        if (charCon.isGrounded == true)
        {
            ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame())
            {
                ySpeed = jumpForce;
            }
        }

        ySpeed += (Physics.gravity.y * Time.deltaTime);

        moveAmount.y = ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);


        // check for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (heldPickup == null)
        {
            if (pickupAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    //heldPickup = hit.collider.gameObject;
                    //heldPickup.transform.SetParent(holdPoint);
                    //heldPickup.transform.localPosition = Vector3.zero;
                    //heldPickup.transform.localRotation = Quaternion.identity;
                    //
                    //heldPickup.GetComponent<Rigidbody>().isKinematic = true;

                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();
                }
            }

            if (takeItemAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    heldPickup = hit.collider.GetComponent<ShelfSpaceController>().GetStock();

                    if (heldPickup != null)
                    {
                        heldPickup.transform.SetParent(holdPoint);
                        heldPickup.Pickup();
                    }
                }
            }
        }
        else
        {
            if (pickupAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    //heldPickup.transform.position = hit.transform.position;
                    //heldPickup.transform.rotation = hit.transform.rotation;
                    //
                    //heldPickup.transform.SetParent(null);
                    //heldPickup = null;

                    //heldPickup.MakePlaced();
                    //heldPickup.transform.SetParent(hit.transform);
                    //heldPickup = null;

                    hit.collider.GetComponent<ShelfSpaceController>().PlaceStock(heldPickup);
                    
                    if (heldPickup.isPlaced == true)
                    {
                        heldPickup = null;
                    }
                }
            }

            if (releaseAction.action.WasPressedThisFrame())
            {
                //Rigidbody pickupRB = heldPickup.GetComponent<Rigidbody>();
                //pickupRB.isKinematic = false;

                heldPickup.Release();

                heldPickup.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                heldPickup.transform.SetParent(null);
                heldPickup = null;
            }
        }
    }
}

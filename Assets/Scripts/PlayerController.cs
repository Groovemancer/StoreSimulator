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
    public InputActionReference interactAction;
    public InputActionReference openBoxAction;
    public InputActionReference moveFurnitureAction;
    public InputActionReference placeFurnitureAction;

    public CharacterController charCon;

    public float moveSpeed;

    public float jumpForce;

    public float lookSpeed;

    public float minLookAngle, maxLookAngle;

    public Camera theCam;

    public LayerMask whatIsStock;
    public LayerMask whatIsShelf;
    public LayerMask whatIsStockBox;
    public LayerMask whatIsBin;
    public LayerMask whatIsFurniture;
    public LayerMask whatIsCheckout;

    public float interactionRange;

    public Transform holdPoint;
    public Transform boxHoldPoint;
    public Transform furniturePoint;

    public float throwForce;

    private StockObject heldPickup;
    public StockBoxController heldBox;
    public FurnitureController heldFurniture;

    public float waitToPlaceStock;
    private float placeStockCounter;
    
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
        if (UIController.instance.updatePricePanel != null)
        {
            if (UIController.instance.updatePricePanel.activeSelf == true)
            {
                return;
            }
        }

        if (UIController.instance.buyMenuScreen != null)
        {
            if (UIController.instance.buyMenuScreen.activeSelf == true)
            {
                return;
            }
        }

        if (UIController.instance.pauseScreen != null)
        {
            if (UIController.instance.pauseScreen.activeSelf == true)
            {
                return;
            }
        }

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

                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySFX(8);
                }
            }
        }
        
        ySpeed += (Physics.gravity.y * Time.deltaTime);

        moveAmount.y = ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);

        // check for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (heldPickup == null && heldBox == null && heldFurniture == null)
        {
            if (pickupAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();

                    if (AudioManager.instance != null)
                    {
                        AudioManager.instance.PlaySFX(6);
                    }

                    return;
                }

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox))
                {
                    heldBox = hit.collider.GetComponent<StockBoxController>();

                    heldBox.transform.SetParent(boxHoldPoint);
                    heldBox.Pickup();

                    if (heldBox.flap1.activeSelf == true)
                    {
                        heldBox.OpenClose();
                    }

                    return;
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
                        return;
                    }
                }
            }

            if (interactAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    hit.collider.GetComponent<ShelfSpaceController>().StartPriceUpdate();
                    return;
                }

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsCheckout))
                {
                    hit.collider.GetComponent<Checkout>().CheckoutCustomer();
                }
            }

            if (openBoxAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox))
                {
                    hit.collider.GetComponent<StockBoxController>().OpenClose();
                }
            }

            if (moveFurnitureAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsFurniture))
                {
                    heldFurniture = hit.transform.GetComponent<FurnitureController>();

                    heldFurniture.transform.SetParent(furniturePoint);
                    heldFurniture.transform.localPosition = Vector3.zero;
                    heldFurniture.transform.localRotation = Quaternion.identity;

                    heldFurniture.MakePlaceable();
                }
            }
        }
        else
        {
            if (heldPickup != null)
            {
                if (pickupAction.action.WasPressedThisFrame())
                {
                    if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                    {
                        hit.collider.GetComponent<ShelfSpaceController>().PlaceStock(heldPickup);

                        if (heldPickup.isPlaced == true)
                        {
                            heldPickup = null;

                            
                        }

                        if (AudioManager.instance != null)
                        {
                            AudioManager.instance.PlaySFX(7);
                        }
                    }
                }

                if (releaseAction.action.WasPressedThisFrame())
                {
                    heldPickup.Release();

                    heldPickup.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    heldPickup.transform.SetParent(null);
                    heldPickup = null;

                    if (AudioManager.instance != null)
                    {
                        AudioManager.instance.PlaySFX(9);
                    }
                }
            }

            if (heldBox != null)
            {
                if (releaseAction.action.WasPressedThisFrame())
                {
                    heldBox.Release();

                    heldBox.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    heldBox.transform.SetParent(null);
                    heldBox = null;
                }

                if (openBoxAction.action.WasPressedThisFrame())
                {
                    heldBox.OpenClose();
                }

                if (pickupAction.action.WasPressedThisFrame())
                {
                    if (heldBox.stockInBox.Count > 0)
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            placeStockCounter = waitToPlaceStock;

                            if (AudioManager.instance != null)
                            {
                                AudioManager.instance.PlaySFX(7);
                            }
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsBin))
                        {
                            Destroy(heldBox.gameObject);
                            heldBox = null;

                            if (AudioManager.instance != null)
                            {
                                AudioManager.instance.PlaySFX(10);
                            }
                        }
                    }
                }

                if (pickupAction.action.IsPressed())
                {
                    placeStockCounter -= Time.deltaTime;

                    if (placeStockCounter <= 0)
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            placeStockCounter = waitToPlaceStock;
                        }
                    }
                }
            }

            if (heldFurniture != null)
            {
                heldFurniture.transform.position = new Vector3(furniturePoint.position.x, 0f, furniturePoint.position.z);
                heldFurniture.transform.LookAt(new Vector3(transform.position.x, 0f, transform.position.z));

                if (placeFurnitureAction.action.WasPressedThisFrame())
                {
                    heldFurniture.transform.SetParent(null);

                    heldFurniture.PlaceFurniture();

                    heldFurniture = null;
                }
            }
        }
    }
}

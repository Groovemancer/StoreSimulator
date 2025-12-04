using System;
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

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_jumpForce;

    [SerializeField]
    private float m_mouseLookSpeed;

    [SerializeField]
    private float m_gamepadLookSpeed;

    [SerializeField]
    private float m_minLookAngle;

    [SerializeField]
    private float m_maxLookAngle;

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
    private StockBoxController m_heldBox;
    private FurnitureController m_heldFurniture;

    public float waitToPlaceStock;
    private float m_placeStockCounter;
    
    private float m_ySpeed;
    private float m_horiRot, m_vertRot;


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

        float cameraLookSpeed = GetLookSpeed();

        m_horiRot += lookInput.x * cameraLookSpeed * Time.deltaTime;
        if (PlayerConfigSettings.Instance.InvertedCamera)
        {
            m_vertRot += lookInput.y * cameraLookSpeed * Time.deltaTime;
        }
        else
        {
            m_vertRot -= lookInput.y * cameraLookSpeed * Time.deltaTime;
        }

        m_vertRot = Mathf.Clamp(m_vertRot, m_minLookAngle, m_maxLookAngle);

        transform.rotation = Quaternion.Euler(0, m_horiRot, 0f);
        theCam.transform.localRotation = Quaternion.Euler(m_vertRot, 0f, 0f);



        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        //transform.position = transform.position + new Vector3(moveInput.x * Time.deltaTime * moveSpeed, 0f, moveInput.y * Time.deltaTime * moveSpeed);

        //Vector3 moveAmount = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 vertMove = transform.forward * moveInput.y;

        Vector3 horiMove = transform.right * moveInput.x;

        Vector3 moveAmount = horiMove + vertMove;
        moveAmount = moveAmount.normalized;

        moveAmount = moveAmount * m_moveSpeed;

        if (charCon.isGrounded == true)
        {
            m_ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame())
            {
                m_ySpeed = m_jumpForce;

                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySFX(8);
                }
            }
        }
        
        m_ySpeed += (Physics.gravity.y * Time.deltaTime);

        moveAmount.y = m_ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);

        // check for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (heldPickup == null && m_heldBox == null && m_heldFurniture == null)
        {
            if (pickupAction.action.WasPressedThisFrame())
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();

                    return;
                }

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox))
                {
                    m_heldBox = hit.collider.GetComponent<StockBoxController>();

                    m_heldBox.transform.SetParent(boxHoldPoint);
                    m_heldBox.Pickup();

                    if (m_heldBox.flap1.activeSelf == true)
                    {
                        m_heldBox.OpenClose();
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
                    m_heldFurniture = hit.transform.GetComponent<FurnitureController>();

                    m_heldFurniture.transform.SetParent(furniturePoint);
                    m_heldFurniture.transform.localPosition = Vector3.zero;
                    m_heldFurniture.transform.localRotation = Quaternion.identity;

                    m_heldFurniture.MakePlaceable();
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

            if (m_heldBox != null)
            {
                if (releaseAction.action.WasPressedThisFrame())
                {
                    m_heldBox.Release();

                    m_heldBox.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    m_heldBox.transform.SetParent(null);
                    m_heldBox = null;
                }

                if (openBoxAction.action.WasPressedThisFrame())
                {
                    m_heldBox.OpenClose();
                }

                if (pickupAction.action.WasPressedThisFrame())
                {
                    if (m_heldBox.stockInBox.Count > 0)
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            m_heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            m_placeStockCounter = waitToPlaceStock;
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsBin))
                        {
                            Destroy(m_heldBox.gameObject);
                            m_heldBox = null;

                            if (AudioManager.instance != null)
                            {
                                AudioManager.instance.PlaySFX(10);
                            }
                        }
                    }
                }

                if (pickupAction.action.IsPressed())
                {
                    m_placeStockCounter -= Time.deltaTime;

                    if (m_placeStockCounter <= 0)
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            m_heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            m_placeStockCounter = waitToPlaceStock;
                        }
                    }
                }
            }

            if (m_heldFurniture != null)
            {
                m_heldFurniture.transform.position = new Vector3(furniturePoint.position.x, 0f, furniturePoint.position.z);
                m_heldFurniture.transform.LookAt(new Vector3(transform.position.x, 0f, transform.position.z));

                if (placeFurnitureAction.action.WasPressedThisFrame())
                {
                    m_heldFurniture.transform.SetParent(null);

                    m_heldFurniture.PlaceFurniture();

                    m_heldFurniture = null;
                }
            }
        }
    }

    private float GetLookSpeed()
    {
        if (GameController.instance.playerInput.currentControlScheme == "Gamepad")
        {
            return m_gamepadLookSpeed * (PlayerConfigSettings.Instance.ControllerSensitivity + 0.5f);
        }
        return m_mouseLookSpeed * (PlayerConfigSettings.Instance.MouseSensitivity + 0.5f);
    }
}

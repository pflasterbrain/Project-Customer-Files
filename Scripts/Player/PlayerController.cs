using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Cursor Lock variables
    public bool isCursorLocked = true;
    public bool isNewScene;

    // POV Camera variables
    public float sensitivity = 1f;
    public Transform player;  // Reference to the player's body (for rotating)
    private float pitch = 0f;

    // Player Interaction variables
    public LayerMask interactableLayer;
    public Transform playerCamera;
    public float interactRange = 1.5f;
    private Ray rayOrigin;
    public GameObject currentInteractableObject;
    public GameObject currentPutdownObject;
    public GameObject currentHolster;
    public GameObject currentSafeObject;
    public Transform bodyTransform;
    public bool canPickup = false;
    public bool canPutdown = false;
    public bool canHolster = false;
    public bool canOpenShop = false;
    public bool goToStore = false;
    public bool goToHouse = false;
    public bool canSell = false;
    public bool canSafe = false;
    public bool canMoveHolsterToHip;
    public SceneManager sceneManager;

    public Pickup hasItem;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI putdownText;
    public TextMeshProUGUI shopText;
    public TextMeshProUGUI gunSafeText;
    public TextMeshProUGUI holsterText;
    public TextMeshProUGUI sellText;
    public TextMeshProUGUI pickUpHolsterText;
    public TextMeshProUGUI goStoreText;
    public TextMeshProUGUI goHomeText;
    public Dialogue dialogue;

    private void Start()
    {
        if (pickupText != null)
        {
            pickupText.enabled = false;
        }
        if (putdownText != null)
        {
            putdownText.enabled = false;
        }
        if (shopText != null)
        {
            shopText.enabled = false;
        }
        if (holsterText != null)
        {
            holsterText.enabled = false;
        }
        if (pickUpHolsterText != null)
        {
            pickUpHolsterText.enabled = false;
        }
        if (gunSafeText != null)
        {
            gunSafeText.enabled = false;
        }
    }
    void Update()
    {
        // Handle First Person Camera Movement
        HandleCameraMovement();

        // Handle Player Interaction
        HandlePlayerInteraction();
    }

    // Handle camera rotation based on mouse movement
    void HandleCameraMovement()
    {
        if (isCursorLocked)
        {
            float lookX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float lookY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            player.Rotate(Vector3.up * lookX);

            pitch -= lookY;
            pitch = Mathf.Clamp(pitch, -45f, 60f);
            playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }

    // Handle player interaction with objects
    void HandlePlayerInteraction()
    {
        // Cast a ray from the center of the screen
        rayOrigin = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
        RaycastHit hit;


        if (Physics.Raycast(rayOrigin, out hit, interactRange, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            // Handle interaction with Pickup objects
            if (hit.collider.CompareTag("Pickup") || hit.collider.CompareTag("Pistol") && !hasItem.hasItem)
            {
                canPickup = true;
                currentInteractableObject = hit.collider.gameObject;
                pickupText.enabled = true;
                putdownText.enabled = false;
                holsterText.enabled = false;
            }
            else
            {
                canPickup = false;
                currentInteractableObject = null;
                pickupText.enabled = false;
                
            }
            GameObject heldItem = hasItem.heldItem;
            if (heldItem != null)
            {
                string heldItemTag = heldItem.tag;

                if (hit.collider.CompareTag("Holster") && heldItemTag == "Pistol")
                {
                    canHolster = true;
                    currentHolster = hitObject;
                    holsterText.enabled = true;
                    putdownText.enabled = false;
                }
                else
                {
                    canHolster = false;
                    holsterText.enabled = false;
                }
                
            }
            if (heldItem != null)
            {
                string heldItemTag = heldItem.tag;

                if (heldItemTag == "Holster" && hit.collider.CompareTag("Safe"))
                {
                    canSafe = true;
                    gunSafeText.enabled = true;
                    currentSafeObject = hitObject;
                    pickUpHolsterText.enabled = false;
                }
                else
                {

                    gunSafeText.enabled = false;
                    canSafe = false;

                }
            }
            if (heldItem == null && hit.collider.CompareTag("Holster"))
            {
                pickUpHolsterText.enabled = true;
                holsterText.enabled = false;
            }
            else
            {
                pickUpHolsterText.enabled = false;
            }
            
            // Handle interaction with PutDown objects
            if (hit.collider.CompareTag("PutDown") && hasItem.hasItem)
            {
                canPutdown = true;
                currentPutdownObject = hit.collider.gameObject;
                putdownText.enabled = true;
                pickupText.enabled = false;
                holsterText.enabled = false;
            }
            else
            {
                canPutdown = false;
                currentPutdownObject = null;
                putdownText.enabled = false;
            }

            if (hit.collider.CompareTag("Sellpoint"))
            {
                canSell = true;
                sellText.enabled = true;
                shopText.enabled = false;
            }
            else
            {
                canSell= false;
                sellText.enabled = false;
            }
            
            
            
            if (hit.collider.CompareTag("HouseDoor"))
            {
                goToStore = true;
                goStoreText.enabled = true;
            }
            else
            {
                goToStore = false;
                goHomeText.enabled = false;
            }
            if (hit.collider.CompareTag("ShopDoor"))
            {
                goToHouse = true;
                goHomeText.enabled = true;
            }
            else
            {
                goToHouse = false;
                goHomeText.enabled = false;
            }
            if (hit.collider.CompareTag("Shop"))
        {
            canOpenShop = true;
            shopText.enabled = true;
        if(Input.GetKeyDown("e"))
        {
          dialogue.StartShopDialogue();
        }


        }
else
{
    canOpenShop = false;

}
            Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * interactRange, Color.blue);
            }
            else
            {
            canPickup = false;
            canPutdown = false;
            goToStore = false;
            goToHouse = false ;
            canSell = false;
            canSafe = false;
            currentInteractableObject = null;
            currentPutdownObject = null;
            pickupText.enabled = false;
            putdownText.enabled = false;
            shopText.enabled = false;
            holsterText.enabled = false;
            goStoreText.enabled = false;
            goHomeText.enabled = false; 
            gunSafeText.enabled = false;
            sellText.enabled = false;
            Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * interactRange, Color.green);
            }
    }
}
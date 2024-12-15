using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float dropForce = 5f; // Force applied when dropping the weapon
    [SerializeField] private float raycastDistance = 3f; // Max distance for raycast pickup detection

    private bool isHeld = true;
    private Rigidbody rb;
    private Transform playerHand;
    private Camera playerCamera;

    // Store the initial local transform
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Store initial local transform
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        initialLocalScale = transform.localScale;

        // Disable gravity when held
        rb.useGravity = false;
        rb.isKinematic = true;

        // Find the main camera
        playerCamera = Camera.main;
    }

    private void Update()
    {
        // Drop weapon when G is pressed
        if (isHeld && Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }

        // Pick up weapon when E is pressed while looking at it
        if (!isHeld && Input.GetKeyDown(KeyCode.E))
        {
            TryPickupWeapon();
        }
    }

    private void DropWeapon()
    {
        // Enable physics
        rb.useGravity = true;
        rb.isKinematic = false;

        // Detach from player's hand
        transform.parent = null;

        // Apply a slight force when dropping
        rb.AddForce(playerCamera.transform.forward * dropForce, ForceMode.Impulse);

        isHeld = false;
    }

    private void TryPickupWeapon()
    {
        // Find the player (you might want to modify this to suit your specific setup)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && playerCamera != null)
        {
            // Create a ray from the center of the screen
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if the ray hits this weapon
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                        // Find the hand/attachment point (adjust as needed)
                        playerHand = playerCamera.transform.Find("Hand"); // Modify this path to match your hierarchy

                        if (playerHand != null)
                        {
                            // Disable physics
                            rb.useGravity = false;
                            rb.isKinematic = true;

                            // Attach to player's hand while maintaining local transform
                            transform.parent = playerHand;
                            transform.localPosition = initialLocalPosition;
                            transform.localRotation = initialLocalRotation;
                            transform.localScale = initialLocalScale;

                            isHeld = true;
                        }
                }
            }
        }
    }

    // Optional: Visualize the raycast in the scene view for debugging
    private void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);
        }
    }
}
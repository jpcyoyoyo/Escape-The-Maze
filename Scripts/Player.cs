using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public Text keyAmount;

    private GameManager gameManager;
    private Rigidbody2D rb;
    
    // AudioSource and AudioClip for key collection and door unlocking sounds
    public AudioSource audioSource;
    public AudioClip keyPickupSound;
    public AudioClip doorUnlockSound;

    // Volume controls for each sound effect
    [Range(0, 1)]
    public float keyPickupVolume = 0.5f;
    [Range(0, 1)]
    public float doorUnlockVolume = 0.7f;

    // Delay time for the door unlock sound
    public float doorUnlockSoundDelay = 0.5f;

    // Track collected keys by their type
    private Dictionary<string, int> collectedKeys = new Dictionary<string, int>();

    // List of doors with specific required keys and key count
    [System.Serializable]
    public class Door
    {
        public List<GameObject> doorObjects;  // The list of door GameObjects
        public string requiredKeyType;       // The key type required to unlock these doors
        public int requiredKeyCount = 1;     // How many keys of this type are needed to unlock these doors
        public bool isUnlocked = false;      // Track if the doors are already unlocked
    }

    public List<Door> doors = new List<Door>(); // List of doors to be dynamically populated

    private PlayerInput inputActions; // Reference to the generated Input Action class
    private Vector2 moveInput; // Store move input value

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        // Enable the input actions
        inputActions.Enable();

        // Subscribe to the Move action
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Disable();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void FixedUpdate()
    {
        // Get the movement vector from the input
        Vector2 movement = speed * moveInput;
        rb.velocity = movement;

        // Check if the player has collected enough keys for each door
        foreach (Door door in doors)
        {
            if (!door.isUnlocked && CountCollectedKeys(door.requiredKeyType) >= door.requiredKeyCount)
            {
                door.isUnlocked = true;
                StartCoroutine(PlayDoorUnlockSoundWithDelay());
                foreach (GameObject doorObject in door.doorObjects)
                {
                    if (doorObject != null)
                    {
                        Destroy(doorObject);
                    }
                }

                Debug.Log($"Enough keys for door group. Required: {door.requiredKeyCount}, Collected: {CountCollectedKeys(door.requiredKeyType)}");
                break; // Exit after unlocking one door groupbreak;  // Exit after unlocking one door
            }
            else if (!door.isUnlocked)
            {
                Debug.Log($"Enough keys for door group. Required: {door.requiredKeyCount}, Collected: {CountCollectedKeys(door.requiredKeyType)}");
            }
        }
    }

    // Handle the movement input using the left stick from the new Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        // Get the input vector (left stick) from the Input System
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Keys"))
        {
            string keyType = collision.gameObject.name;  // Assuming key name corresponds to its type

            // Add key to dictionary, increase count for the type of key
            if (!collectedKeys.ContainsKey(keyType))
            {
                collectedKeys[keyType] = 0;
            }

            collectedKeys[keyType]++;
            keyAmount.text = "Keys: " + GetTotalKeyCount();

            // Play the key collection sound with volume control
            if (audioSource != null && keyPickupSound != null)
            {
                audioSource.PlayOneShot(keyPickupSound, keyPickupVolume);
            }

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Exit"))
        {
            gameManager.WinGame();
        }

        if (collision.gameObject.CompareTag("Enemies"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            Vector2 repulsiveForce = 0.3f * speed * collision.contacts[0].normal;
            rb.AddForce(repulsiveForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator PlayDoorUnlockSoundWithDelay()
    {
        yield return new WaitForSeconds(doorUnlockSoundDelay);

        if (audioSource != null && doorUnlockSound != null)
        {
            audioSource.PlayOneShot(doorUnlockSound, doorUnlockVolume);
        }
    }

    // Method to count the number of collected keys of a specific type
    private int CountCollectedKeys(string keyType)
    {
        if (collectedKeys.ContainsKey(keyType))
        {
            return collectedKeys[keyType];
        }
        return 0;
    }

    // Method to get the total number of keys the player has collected across all types
    private int GetTotalKeyCount()
    {
        int total = 0;
        foreach (var key in collectedKeys)
        {
            total += key.Value;
        }
        return total;
    }
}

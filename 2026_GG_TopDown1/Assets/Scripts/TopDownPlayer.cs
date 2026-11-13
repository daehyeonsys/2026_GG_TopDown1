
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TopDownPlayerFixed : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float frameTime = 0.15f;

    public Sprite[] downSprites;
    public Sprite[] upSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    public DialogueBox dialogueBox;

    private Vector2 moveInput;
    private Vector2 facingDir = Vector2.down;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float frameTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentSprites = downSprites;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput != Vector2.zero)
        {
            facingDir = moveInput;
        }
    }

    void Update()
    {
        Vector2 movement = moveInput;

        if (dialogueBox != null && dialogueBox.isOpen)
        {
            movement = Vector2.zero;
        }

        if (Mathf.Abs(facingDir.x) > Mathf.Abs(facingDir.y))
        {
            currentSprites = facingDir.x < 0
                ? leftSprites : rightSprites;
        }
        else
        {
            currentSprites = facingDir.y > 0
                ? upSprites : downSprites;
        }

        if (currentSprites == null ||
            currentSprites.Length == 0)
        {
            return;
        }

        if (movement != Vector2.zero)
        {
            frameTimer -= Time.deltaTime;

            if (frameTimer <= 0f)
            {
                frameTimer = Mathf.Max(0.01f, frameTime);
                frameIndex++;

                if (frameIndex >= currentSprites.Length)
                {
                    frameIndex = 0;
                }
            }
        }
        else
        {
            frameIndex = 0;
            frameTimer = 0f;
        }

        sr.sprite = currentSprites[frameIndex];
    }

    void FixedUpdate()
    {
        Vector2 movement = moveInput;

        if (dialogueBox != null && dialogueBox.isOpen)
        {
            movement = Vector2.zero;
        }

        rb.linearVelocity = movement * moveSpeed;
    }
}

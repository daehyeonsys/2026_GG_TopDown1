
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TopDownPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;

    [Header("Animation")]
    public float frameTime = 0.15f;

    [Header("Down Sprites")]
    public Sprite[] downSprites = new Sprite[3];

    [Header("Up Sprites")]
    public Sprite[] upSprites = new Sprite[3];

    [Header("Left Sprites")]
    public Sprite[] leftSprites = new Sprite[3];

    [Header("Right Sprites")]
    public Sprite[] rightSprites = new Sprite[3];

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private Vector2 facingDir = Vector2.down;

    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float frameTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentSprites = downSprites;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            facingDir = moveInput;
        }
    }

    void Update()
    {
        // Select walking direction
        if (Mathf.Abs(facingDir.y) >= Mathf.Abs(facingDir.x))
        {
            currentSprites = facingDir.y > 0
                ? upSprites
                : downSprites;
        }
        else
        {
            currentSprites = facingDir.x < 0
                ? leftSprites
                : rightSprites;
        }

        // Prevent missing sprite errors
        if (currentSprites == null ||
            currentSprites.Length == 0)
        {
            return;
        }

        // Walking animation
        if (moveInput.sqrMagnitude > 0.01f)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameTime)
            {
                frameTimer = 0f;
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

        frameIndex %= currentSprites.Length;

        Sprite nextSprite = currentSprites[frameIndex];

        if (nextSprite != null)
        {
            sr.sprite = nextSprite;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity =
            moveInput.normalized * moveSpeed;
    }
}

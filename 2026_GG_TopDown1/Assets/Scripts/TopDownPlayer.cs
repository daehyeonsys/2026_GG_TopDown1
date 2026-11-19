
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TopDownPlayerFixed : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 4f;
    public float frameTime = 0.15f;

    [Header("기본 캐릭터 스프라이트")]
    public Sprite[] downSprites;
    public Sprite[] upSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    [Header("대화 시스템")]
    public DialogueBox dialogueBox;

    [Header("무기 시스템")]
    public WeaponSystem weaponSystem;

    private Vector2 moveInput;
    private Vector2 facingDir = Vector2.down;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float frameTimer = 0f;

    private int previousWeapon = -1;
    private int previousDirection = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (weaponSystem == null)
        {
            weaponSystem = GetComponent<WeaponSystem>();
        }

        currentSprites = downSprites;
    }

    public void OnMove(InputValue value)
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

        int direction = GetDirection();

        int currentWeapon = 0;

        if (weaponSystem != null)
        {
            currentWeapon = weaponSystem.equippedWeapon;
        }

        if (currentWeapon != previousWeapon ||
            direction != previousDirection)
        {
            frameIndex = 0;
            frameTimer = 0f;

            previousWeapon = currentWeapon;
            previousDirection = direction;
        }

        currentSprites = GetCurrentSprites(direction);

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

        if (frameIndex >= currentSprites.Length)
        {
            frameIndex = 0;
        }

        if (currentSprites[frameIndex] != null)
        {
            sr.sprite = currentSprites[frameIndex];
        }
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

    int GetDirection()
    {
        if (Mathf.Abs(facingDir.x) > Mathf.Abs(facingDir.y))
        {
            return facingDir.x < 0 ? 2 : 3;
        }

        return facingDir.y > 0 ? 1 : 0;
    }

    Sprite[] GetCurrentSprites(int direction)
    {
        // 무기 장착 중이면 무기 스프라이트 사용
        if (weaponSystem != null &&
            weaponSystem.equippedWeapon != 0)
        {
            Sprite[] weaponSprites =
                weaponSystem.GetSprites(direction);

            if (weaponSprites != null &&
                weaponSprites.Length > 0 &&
                weaponSprites[0] != null)
            {
                return weaponSprites;
            }
        }

        // 무기를 장착하지 않았다면 기본 스프라이트
        switch (direction)
        {
            case 0:
                return downSprites;

            case 1:
                return upSprites;

            case 2:
                return leftSprites;

            case 3:
                return rightSprites;
        }

        return downSprites;
    }
}

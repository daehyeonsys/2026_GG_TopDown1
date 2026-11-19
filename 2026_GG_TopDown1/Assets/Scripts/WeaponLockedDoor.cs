
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponLockedDoor : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("이동할 위치")]
    public Transform destination;

    [Header("무기 시스템")]
    public WeaponSystem weaponSystem;

    [Header("대화 시스템")]
    public DialogueBox dialogueBox;

    [Header("문 안내 UI")]
    public GameObject prompt;
    public TMP_Text promptText;

    [Header("설정")]
    public bool requireBothWeapons = true;

    private bool playerNearby = false;
    private bool isUnlocked = false;

    void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);

        CheckDoorState();
    }

    void Update()
    {
        CheckDoorState();

        if (!playerNearby)
            return;

        if (dialogueBox != null && dialogueBox.isOpen)
            return;

        Keyboard kb = Keyboard.current;

        if (kb == null)
            return;

        if (kb.eKey.wasPressedThisFrame)
        {
            TryEnterDoor();
        }
    }

    void CheckDoorState()
    {
        if (!requireBothWeapons)
        {
            isUnlocked = true;
        }
        else if (weaponSystem != null)
        {
            isUnlocked = weaponSystem.HasAllWeapons();
        }
        else
        {
            isUnlocked = false;
        }

        if (playerNearby && promptText != null)
        {
            promptText.text = isUnlocked
                ? "E : 문 열기"
                : "문이 잠겨 있습니다.\n무기를 챙겨야 합니다.";
        }
    }

    void TryEnterDoor()
    {
        if (!isUnlocked)
        {
            Debug.Log("무기가 없어 문을 열 수 없습니다.");
            return;
        }

        if (player == null || destination == null)
        {
            Debug.LogError(
                "Player 또는 Destination이 연결되지 않았습니다."
            );
            return;
        }

        // 이동 중 물리 속도 초기화
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        player.position = destination.position;

        playerNearby = false;

        if (prompt != null)
            prompt.SetActive(false);

        Debug.Log("문을 통과해 다음 장소로 이동했습니다.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        CheckDoorState();

        if (prompt != null)
            prompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (prompt != null)
            prompt.SetActive(false);
    }
}

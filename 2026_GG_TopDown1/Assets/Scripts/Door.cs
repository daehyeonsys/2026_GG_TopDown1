
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("이동할 위치")]
    public Transform target;

    [Header("무기 시스템")]
    public WeaponSystem weaponSystem;

    [Header("대화 시스템")]
    public DialogueBox dialogueBox;

    [Header("문 안내 UI")]
    public GameObject prompt;
    public TMP_Text promptText;

    [Header("문 설정")]
    public bool requireWeapons = true;

    private bool playerNearby = false;
    private bool isUnlocked = false;

    void Start()
    {
        if (prompt != null && prompt != gameObject)
        {
            prompt.SetActive(false);
        }

        UpdateDoorState();
    }

    void Update()
    {
        UpdateDoorState();

        if (!playerNearby)
            return;

        if (dialogueBox != null && dialogueBox.isOpen)
            return;

        Keyboard kb = Keyboard.current;

        if (kb == null)
            return;

        if (kb.eKey.wasPressedThisFrame)
        {
            Use();
        }
    }

    void UpdateDoorState()
    {
        if (!requireWeapons)
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked =
                weaponSystem != null &&
                weaponSystem.HasAllWeapons();
        }

        if (playerNearby && promptText != null)
        {
            if (isUnlocked)
            {
                promptText.text = "E : 문 열기";
            }
            else
            {
                promptText.text =
                    "문이 잠겨 있습니다.\n무기를 챙겨야 합니다.";
            }
        }
    }

    public void Use()
    {
        UpdateDoorState();

        // 무기를 획득하지 않았다면 이동 불가
        // 기존 열쇠 대화창은 표시하지 않음
        if (!isUnlocked)
        {
            Debug.Log("무기를 획득해야 문을 열 수 있습니다.");
            return;
        }

        if (player == null || target == null)
        {
            Debug.LogError(
                "Door: Player 또는 Target이 연결되지 않았습니다."
            );
            return;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        player.position = target.position;

        playerNearby = false;

        if (prompt != null && prompt != gameObject)
        {
            prompt.SetActive(false);
        }

        Debug.Log("문을 통과했습니다.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        UpdateDoorState();

        if (prompt != null && prompt != gameObject)
        {
            prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (prompt != null && prompt != gameObject)
        {
            prompt.SetActive(false);
        }
    }
}

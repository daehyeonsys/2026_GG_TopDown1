
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [Header("문 이동 설정")]
    public Transform target;
    public GameObject prompt;

    [Header("대화창 설정")]
    public DialogueBox dialogueBox;
    public Talk lockedTalk;

    [Header("필요한 열쇠 개수")]
    public int needKeys = 0;

    private Transform player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;

            if (prompt != null)
                prompt.SetActive(true);

            if (dialogueBox != null)
                dialogueBox.nearDoor = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;

            if (prompt != null)
                prompt.SetActive(false);

            if (dialogueBox != null &&
                dialogueBox.nearDoor == this)
            {
                dialogueBox.nearDoor = null;
            }
        }
    }

    public void Use()
    {
        if (player == null || target == null)
            return;

        Inventory inventory =
            player.GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning(
                "Player에 Inventory가 없습니다!"
            );
            return;
        }

        if (inventory.keyCount >= needKeys)
        {
            player.position = target.position;
        }
        else
        {
            if (lockedTalk != null)
                lockedTalk.Say();
        }
    }
}

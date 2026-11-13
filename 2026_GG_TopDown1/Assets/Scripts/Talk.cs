
using UnityEngine;
using UnityEngine.Events;

public class Talk : MonoBehaviour
{
    // 대화 내용
    public DialogueLine[] lines;

    // 선택지
    public string choice1;
    public string choice2;

    // 선택지별 이벤트
    public UnityEvent onChoice1;
    public UnityEvent onChoice2;

    // 대화 안내 및 대화창
    public GameObject prompt;
    public DialogueBox dialogueBox;

    // 대화 시작
    public void Say()
    {
        if (dialogueBox != null)
        {
            dialogueBox.Open(this);
        }
    }

    // 플레이어가 대화 범위에 들어왔을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }

            if (dialogueBox != null)
            {
                dialogueBox.nearTalk = this;
            }
        }
    }

    // 플레이어가 대화 범위에서 벗어났을 때
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (prompt != null)
            {
                prompt.SetActive(false);
            }

            if (dialogueBox != null &&
                dialogueBox.nearTalk == this)
            {
                dialogueBox.nearTalk = null;
            }
        }
    }
}

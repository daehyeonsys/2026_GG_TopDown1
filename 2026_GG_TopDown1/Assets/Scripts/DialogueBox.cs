
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("대화창 UI")]
    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text bodyText;

    [Header("선택지 UI")]
    public GameObject choicePanel;
    public TMP_Text choiceText1;
    public TMP_Text choiceText2;

    [Header("대화 상태")]
    public bool isOpen = false;
    public Talk nearTalk;

    // 문 상호작용 추가
    public Door nearDoor;

    private Talk current;
    private DialogueLine[] lines;
    private int index = 0;
    private bool choosing = false;

    void Start()
    {
        isOpen = false;
        choosing = false;

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;

        if (kb == null)
            return;

        // 선택지가 표시된 상태
        if (choosing)
        {
            if (kb.digit1Key.wasPressedThisFrame ||
                kb.numpad1Key.wasPressedThisFrame)
            {
                Choose(1);
            }
            else if (kb.digit2Key.wasPressedThisFrame ||
                     kb.numpad2Key.wasPressedThisFrame)
            {
                Choose(2);
            }

            return;
        }

        // E키로 대화 시작, 진행 및 문 사용
        if (kb.eKey.wasPressedThisFrame)
        {
            if (isOpen)
            {
                Next();
            }
            else if (nearTalk != null)
            {
                Open(nearTalk);
            }
            else if (nearDoor != null)
            {
                nearDoor.Use();
            }
        }
    }

    // NPC와 대화 시작
    public void Open(Talk talk)
    {
        if (talk == null ||
            talk.lines == null ||
            talk.lines.Length == 0)
        {
            Debug.LogWarning("대화 내용이 없습니다.");
            return;
        }

        current = talk;
        lines = talk.lines;
        index = 0;
        choosing = false;
        isOpen = true;

        if (panel != null)
            panel.SetActive(true);

        if (choicePanel != null)
            choicePanel.SetActive(false);

        ShowLine();
    }

    // 기존 Show 함수 호환
    public void Show(DialogueLine[] newLines)
    {
        if (newLines == null || newLines.Length == 0)
            return;

        current = null;
        lines = newLines;
        index = 0;
        choosing = false;
        isOpen = true;

        if (panel != null)
            panel.SetActive(true);

        if (choicePanel != null)
            choicePanel.SetActive(false);

        ShowLine();
    }

    // 현재 대사 표시
    void ShowLine()
    {
        if (lines == null ||
            index < 0 ||
            index >= lines.Length)
            return;

        if (nameText != null)
            nameText.text = lines[index].speaker;

        if (bodyText != null)
            bodyText.text = lines[index].text;

        // 마지막 대사인지 확인
        bool isLast = index == lines.Length - 1;

        // 마지막 대사에 선택지가 있다면 표시
        if (isLast &&
            current != null &&
            !string.IsNullOrWhiteSpace(current.choice1))
        {
            ShowChoices();
        }
    }

    // 다음 대사
    void Next()
    {
        if (choosing)
            return;

        index++;

        if (lines != null && index < lines.Length)
        {
            ShowLine();
        }
        else
        {
            Close();
        }
    }

    // 선택지 표시
    void ShowChoices()
    {
        if (current == null)
            return;

        choosing = true;

        if (choiceText1 != null)
        {
            choiceText1.text = "1. " + current.choice1;
        }

        if (choiceText2 != null)
        {
            choiceText2.text = "2. " + current.choice2;
        }

        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
            Debug.Log("선택지 표시 성공!");
        }
        else
        {
            Debug.LogError(
                "DialogueBox의 Choice Panel이 연결되지 않았습니다."
            );
        }
    }

    // 선택지 실행
    public void Choose(int number)
    {
        if (!choosing || current == null)
            return;

        Talk selectedTalk = current;

        Close();

        if (number == 1)
        {
            selectedTalk.onChoice1?.Invoke();
            Debug.Log("1번 선택지 선택");
        }
        else if (number == 2)
        {
            selectedTalk.onChoice2?.Invoke();
            Debug.Log("2번 선택지 선택");
        }
    }

    // 대화 종료
    void Close()
    {
        choosing = false;
        isOpen = false;

        current = null;
        lines = null;
        index = 0;

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (panel != null)
            panel.SetActive(false);
    }
}


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("대화창 UI")]
    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text bodyText;

    [Header("캐릭터별 일러스트 UI")]
    public Image rowenPortraitImage;
    public Image kaiPortraitImage;

    [Header("선택지 UI")]
    public GameObject choicePanel;
    public TMP_Text choiceText1;
    public TMP_Text choiceText2;

    [Header("대화 상태")]
    public bool isOpen = false;
    public Talk nearTalk;
    public Door nearDoor;

    [Header("음악 상자 획득")]
    public Talk musicBoxRewardTalk;
    public ItemPickupUI itemPickupUI;

    [Header("메인 퀘스트")]
    public MainQuestManager questManager;
    public Talk questStartTalk;

    private Talk current;
    private DialogueLine[] lines;
    private int index = 0;
    private bool choosing = false;

    void Start()
    {
        isOpen = false;
        choosing = false;

        if (panel != null)
            panel.SetActive(false);

        if (choicePanel != null)
            choicePanel.SetActive(false);

        HideAllPortraits();
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;

        if (kb == null)
            return;

        // 선택지 입력
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

        // E키 대화 및 문 상호작용
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

    // NPC 대화 시작
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

        DialogueLine line = lines[index];

        if (nameText != null)
            nameText.text = line.speaker;

        if (bodyText != null)
            bodyText.text = line.text;

        UpdatePortrait(line.speaker);

        bool isLast = index == lines.Length - 1;

        if (isLast &&
            current != null &&
            !string.IsNullOrWhiteSpace(current.choice1))
        {
            ShowChoices();
        }
    }

    // 캐릭터 일러스트 변경
    void UpdatePortrait(string speaker)
    {
        HideAllPortraits();

        if (string.IsNullOrWhiteSpace(speaker))
            return;

        string speakerName = speaker.Trim();

        if (speakerName == "로웬")
        {
            if (rowenPortraitImage != null)
                rowenPortraitImage.gameObject.SetActive(true);
        }
        else if (speakerName == "카이")
        {
            if (kaiPortraitImage != null)
                kaiPortraitImage.gameObject.SetActive(true);
        }
    }

    // 일러스트 숨기기
    void HideAllPortraits()
    {
        if (rowenPortraitImage != null)
            rowenPortraitImage.gameObject.SetActive(false);

        if (kaiPortraitImage != null)
            kaiPortraitImage.gameObject.SetActive(false);
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
            Talk finishedTalk = current;

            Close();

            ProcessDialogueRewards(finishedTalk);
        }
    }

    // 대화 종료 후 보상 및 퀘스트 처리
    void ProcessDialogueRewards(Talk finishedTalk)
    {
        if (finishedTalk == null)
            return;

        // 음악 상자 획득
        if (finishedTalk == musicBoxRewardTalk)
        {
            if (itemPickupUI != null)
            {
                itemPickupUI.GetMusicBox();
                Debug.Log("음악 상자 획득!");
            }
        }

        // 메인 퀘스트 시작
        if (finishedTalk == questStartTalk)
        {
            if (questManager != null)
            {
                questManager.StartQuest();
                Debug.Log("로웬 대화 완료! 메인 퀘스트 시작!");
            }
            else
            {
                Debug.LogWarning(
                    "MainQuestManager가 연결되지 않았습니다."
                );
            }
        }
    }

    // 선택지 표시
    void ShowChoices()
    {
        if (current == null)
            return;

        choosing = true;

        if (choiceText1 != null)
            choiceText1.text = "1. " + current.choice1;

        if (choiceText2 != null)
            choiceText2.text = "2. " + current.choice2;

        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
            Debug.Log("선택지 표시 성공!");
        }
        else
        {
            Debug.LogError(
                "Choice Panel이 연결되지 않았습니다."
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

        // 선택지 이벤트 실행
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

        // 선택지 이벤트에서 새 대화를 열었다면
        // 이전 대화의 보상은 실행하지 않음
        if (!isOpen)
        {
            ProcessDialogueRewards(selectedTalk);
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

        HideAllPortraits();

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (panel != null)
            panel.SetActive(false);
    }
}

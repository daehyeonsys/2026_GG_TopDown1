
using UnityEngine;
using TMPro;
using System.Collections;

public class MainQuestManager : MonoBehaviour
{
    [Header("메인 퀘스트 UI")]
    public GameObject questPanel;
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;

    [Header("퀘스트 완료 메시지")]
    public TMP_Text completeText;
    public float completeDisplayTime = 2f;

    [Header("퀘스트 상태")]
    public bool questStarted = false;
    public bool questCompleted = false;
    public int questStage = 0;

    private Coroutine completeCoroutine;

    void Start()
    {
        if (questStarted)
        {
            UpdateQuestUI();
        }
        else if (questPanel != null)
        {
            questPanel.SetActive(false);
        }

        if (completeText != null)
            completeText.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // 퀘스트 시작 후 패널이 꺼지면 다시 활성화
        if (questStarted && questPanel != null)
        {
            if (!questPanel.activeSelf)
            {
                questPanel.SetActive(true);
                Debug.LogWarning(
                    "QuestPanel이 비활성화되어 다시 켰습니다."
                );
            }
        }
    }

    public void StartQuest()
    {
        if (questStarted)
            return;

        questStarted = true;
        questCompleted = false;
        questStage = 1;

        UpdateQuestUI();

        Debug.Log("메인 퀘스트 시작!");
    }

    public void ReachWeaponTable()
    {
        if (!questStarted || questStage != 1)
            return;

        questStage = 2;

        UpdateQuestUI();
        ShowCompleteMessage();
    }

    public void CollectWeapons()
    {
        if (!questStarted || questStage != 2)
            return;

        questStage = 3;

        UpdateQuestUI();
        ShowCompleteMessage();
    }

    void UpdateQuestUI()
    {
        if (questPanel != null)
            questPanel.SetActive(true);

        if (questTitleText != null)
            questTitleText.text = "MAIN QUEST";

        if (questDescriptionText == null)
            return;

        string questList = "";

        if (questStage >= 1)
        {
            questList += questStage > 1
                ? "<color=#8C8C8C>✔ 작업대로 이동하기</color>\n"
                : "<color=#FFD166>○ 작업대로 이동하기</color>\n";
        }

        if (questStage >= 2)
        {
            questList += questStage > 2
                ? "<color=#8C8C8C>✔ 장비 챙기기</color>\n"
                : "<color=#FFD166>○ 장비 챙기기</color>\n";
        }

        if (questStage >= 3)
        {
            questList +=
                "<color=#FFD166>○ 배달 시작하기</color>";
        }

        questDescriptionText.richText = true;
        questDescriptionText.text = questList;
    }

    void ShowCompleteMessage()
    {
        if (completeText == null)
            return;

        completeText.text = "퀘스트 완료!";
        completeText.gameObject.SetActive(true);

        if (completeCoroutine != null)
            StopCoroutine(completeCoroutine);

        completeCoroutine =
            StartCoroutine(HideCompleteMessage());
    }

    IEnumerator HideCompleteMessage()
    {
        yield return new WaitForSecondsRealtime(
            completeDisplayTime
        );

        if (completeText != null)
            completeText.gameObject.SetActive(false);

        completeCoroutine = null;
    }
}

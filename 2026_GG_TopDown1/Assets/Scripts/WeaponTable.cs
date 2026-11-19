
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class WeaponTable : MonoBehaviour
{
    [Header("상호작용 안내")]
    public GameObject prompt;

    [Header("무기 시스템")]
    public WeaponSystem weaponSystem;

    [Header("메인 퀘스트")]
    public MainQuestManager questManager;

    [Header("대화 시스템")]
    public DialogueBox dialogueBox;

    [Header("무기 획득 UI")]
    public GameObject pickupPanel;
    public TMP_Text pickupText;
    public float displayTime = 3f;

    private bool playerNearby = false;
    private bool collected = false;

    void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);

        if (pickupPanel != null)
            pickupPanel.SetActive(false);

        if (weaponSystem != null)
            collected = weaponSystem.HasAllWeapons();
    }

    void Update()
    {
        if (!playerNearby || collected)
            return;

        if (dialogueBox != null && dialogueBox.isOpen)
            return;

        Keyboard kb = Keyboard.current;

        if (kb != null && kb.eKey.wasPressedThisFrame)
            CollectWeapons();
    }

    void CollectWeapons()
    {
        if (weaponSystem == null || collected)
            return;

        weaponSystem.GetWeapons();
        collected = true;

        if (prompt != null)
            prompt.SetActive(false);

        if (questManager != null)
            questManager.CollectWeapons();

        if (pickupPanel != null)
        {
            if (pickupText != null)
            {
                pickupText.text =
                    "쇠파이프와 권총을 획득했습니다!\n" +
                    "3 : 쇠파이프 장착\n" +
                    "4 : 권총 장착";
            }

            pickupPanel.SetActive(true);
            StartCoroutine(HidePickupMessage());
        }

        Debug.Log("무기 획득 완료!");
    }

    IEnumerator HidePickupMessage()
    {
        yield return new WaitForSecondsRealtime(displayTime);

        if (pickupPanel != null)
            pickupPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        if (!collected && prompt != null)
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

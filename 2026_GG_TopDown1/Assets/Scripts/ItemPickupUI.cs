
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemPickupUI : MonoBehaviour
{
    [Header("아이템 획득 UI")]
    public GameObject itemPanel;
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    [Header("음악 상자 이미지")]
    public Sprite musicBoxSprite;

    [Header("표시 시간")]
    public float displayTime = 3f;

    [Header("획득 상태")]
    public bool hasMusicBox = false;

    private Coroutine hideCoroutine;

    void Start()
    {
        if (itemPanel != null)
            itemPanel.SetActive(false);
    }

    public void GetMusicBox()
    {
        if (hasMusicBox)
            return;

        hasMusicBox = true;

        if (itemNameText != null)
            itemNameText.text = "오래된 음악 상자";

        if (itemDescriptionText != null)
            itemDescriptionText.text =
                "미아에게 전달해야 하는 중요한 물건.";

        if (itemIcon != null)
        {
            itemIcon.sprite = musicBoxSprite;
            itemIcon.enabled = musicBoxSprite != null;
        }

        if (itemPanel != null)
        {
            itemPanel.SetActive(true);

            if (hideCoroutine != null)
                StopCoroutine(hideCoroutine);

            hideCoroutine =
                StartCoroutine(HideAfterDelay());
        }

        Debug.Log("오래된 음악 상자 획득!");
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displayTime);

        if (itemPanel != null)
            itemPanel.SetActive(false);

        hideCoroutine = null;
    }
}

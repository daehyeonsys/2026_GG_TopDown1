
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public TMP_Text keyText;
    public int keyCount = 0;

    void Start()
    {
        UpdateUI();
    }

    public void AddKey()
    {
        keyCount++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (keyText != null)
        {
            keyText.text = "열쇠: " + keyCount;
        }
    }
}

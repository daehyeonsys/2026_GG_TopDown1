using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class DialogueBox : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text bodyText;
    public bool isOpen = false;
    public Talk nearTalk;
    private DialogueLine[] lines;
    private int index = 0;
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isOpen) { Next(); }
            else if (nearTalk != null) { Show(nearTalk.lines); }
        }
    }
    public void Show(DialogueLine[] newLines)
    {
        lines = newLines;
        index = 0;
        isOpen = true;
        panel.SetActive(true);
        ShowLine();
    }
    void ShowLine()
    {
        nameText.text = lines[index].speaker;
        bodyText.text = lines[index].text;
    }
    void Next()
    {
        index += 1;
        if (index < lines.Length) { ShowLine(); }
        else { isOpen = false; panel.SetActive(false); }
    }
}

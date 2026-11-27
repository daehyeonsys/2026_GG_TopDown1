using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Cutscene : MonoBehaviour
{
    public DialogueLine[] lines;
    public Image picture;
    public TMP_Text nameText;
    public TMP_Text bodyText;
    public string nextScene;

    private int index = 0;

    void Start()
    {
        Time.timeScale = 1f;
        index = 0;

        if (lines == null || lines.Length == 0)
            return;

        Show();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (lines == null || lines.Length == 0)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            index += 1;

            if (index < lines.Length)
            {
                Show();
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    void Show()
    {
        if (nameText != null)
            nameText.text = lines[index].speaker;

        if (bodyText != null)
            bodyText.text = lines[index].text;

        if (picture != null && lines[index].portrait != null)
        {
            picture.sprite = lines[index].portrait;
        }
    }
}
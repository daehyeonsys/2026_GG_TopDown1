using UnityEngine;
public class Talk : MonoBehaviour
{
    public DialogueLine[] lines;
    public GameObject prompt;
    public DialogueBox dialogueBox;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            prompt.SetActive(true);
            dialogueBox.nearTalk = this;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            prompt.SetActive(false);
            dialogueBox.nearTalk = null;
        }
    }
}

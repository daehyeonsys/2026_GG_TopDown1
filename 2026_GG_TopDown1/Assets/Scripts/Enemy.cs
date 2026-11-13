using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public Transform player;
    public DialogueBox dialogueBox;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 toPlayer = player.position - transform.position;
        if (dialogueBox.isOpen || toPlayer.magnitude > chaseRange)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = toPlayer.normalized * moveSpeed;
        }
    }
}

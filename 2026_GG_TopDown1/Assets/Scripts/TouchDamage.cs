using UnityEngine;
public class TouchDamage : MonoBehaviour
{
    public int damage = 2;
    public PlayerStats playerStats;
    private float timer = 0f;
    void Update() { timer -= Time.deltaTime; }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && timer <= 0f)
        {
            playerStats.Damage(damage);
            timer = 1f;
        }
    }
}

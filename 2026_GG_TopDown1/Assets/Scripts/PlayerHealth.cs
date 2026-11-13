using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public int life = 3;
    public float safeTime = 1f;
    public TMP_Text lifeText;
    private float safeTimer = 0f;
    void Update()
    {
        safeTimer -= Time.deltaTime;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        bool isEnemy = collision.gameObject.CompareTag("Enemy");
        if (isEnemy && safeTimer <= 0f)
        {
            life -= 1;
            safeTimer = safeTime;
            lifeText.text = "목숨 " + life;
            if (life <= 0)
            {
                string scene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(scene);
            }
        }
    }
}

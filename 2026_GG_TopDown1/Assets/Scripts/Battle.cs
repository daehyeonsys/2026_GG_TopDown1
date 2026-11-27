using UnityEngine;
using UnityEngine.Events;
public class Battle : MonoBehaviour
{
    public string enemyName = "도적";
    public string startText = "도적이 길을 막았다!";
    public Sprite picture;
    public int hp = 5;
    public int attack = 2;
    public UnityEvent onWin;
    public BattleScreen screen;
    private float restTimer = 0f;
    void Update()
    {
        restTimer -= Time.deltaTime;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        bool isPlayer = collision.gameObject.CompareTag("Player");
        if (isPlayer && restTimer <= 0f && !screen.inBattle)
        {
            screen.StartBattle(this);
        }
    }
    public void Fight() { screen.StartBattle(this); }
    public void Rest() { restTimer = 2f; }
}

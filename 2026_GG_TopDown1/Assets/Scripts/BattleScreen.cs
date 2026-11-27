using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class BattleScreen : MonoBehaviour
{
    public GameObject panel;
    public Image enemyImage;
    public TMP_Text enemyText;
    public TMP_Text messageText;
    public TMP_Text[] menuTexts;
    public BattleAction[] actions;
    public PlayerStats playerStats;
    public int potionHeal = 5;
    public bool inBattle = false;
    private Battle enemy;
    private int enemyHp;
    private int cursor = 0, startFrame = 0;
    public void StartBattle(Battle target)
    {
        enemy = target;
        enemyHp = enemy.hp;
        enemyImage.sprite = enemy.picture;
        cursor = 0; startFrame = Time.frameCount;
        inBattle = true;
        Time.timeScale = 0f;
        panel.SetActive(true);
        messageText.text = enemy.startText;
    }
    void Draw()
    {
        enemyText.text = enemy.enemyName + "  체력 " + enemyHp;
        for (int i = 0; i < menuTexts.Length; i++)
        {
            string mark = "   ";
            if (i == cursor) { mark = "> "; }
            if (i < actions.Length)
            {
                menuTexts[i].text = mark + actions[i].text;
            }
            else { menuTexts[i].text = ""; }
        }
    }
    public void Attack()
    {
        enemyHp -= GameData.attack;
        if (enemyHp <= 0)
        {
            End();
            enemy.onWin.Invoke();
            Destroy(enemy.gameObject);
        }
        else
        {
            string a = enemy.enemyName + "에게 " + GameData.attack;
            EnemyTurn(a + " 피해!");
        }
    }
    public void UsePotion()
    {
        if (GameData.potions > 0)
        {
            GameData.potions -= 1;
            playerStats.AddHp(potionHeal);
            EnemyTurn("물약으로 체력 회복!");
        }
        else { messageText.text = "물약이 없다."; }
    }
    void EnemyTurn(string message)
    {
        messageText.text = message + " 반격 -" + enemy.attack;
        if (GameData.hp <= enemy.attack) { End(); }
        playerStats.Damage(enemy.attack);
    }
    public void Run()
    {
        End();
        enemy.Rest();
    }
    void End()
    {
        inBattle = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
    }
}

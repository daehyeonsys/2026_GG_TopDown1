using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    public TMP_Text statText;
    public GameObject gameOverPanel;
    void Start()
    {
        Time.timeScale = 1f;
        Refresh();
    }
    public void Refresh()
    {
        string a = "체력 " + GameData.hp + " / " + GameData.maxHp;
        string b = "\n공격력 " + GameData.attack;
        statText.text = a + b + "\n물약 " + GameData.potions;
    }
    public void AddHp(int amount)
    {
        GameData.hp += amount;
        if (GameData.hp > GameData.maxHp)
        {
            GameData.hp = GameData.maxHp;
        }
        Refresh();
    }
    public void AddAttack(int amount)
    {
        GameData.attack += amount;
        Refresh();
    }
    public void AddPotion(int amount)
    {
        GameData.potions += amount;
        Refresh();
    }
    public void Damage(int amount)
    {
        GameData.hp -= amount;
        Refresh();
        if (GameData.hp <= 0)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }
    }
    public void Restart()
    {
        GameData.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

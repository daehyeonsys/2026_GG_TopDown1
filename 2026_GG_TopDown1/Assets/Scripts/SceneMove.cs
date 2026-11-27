using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public string sceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { Go(); }
    }
    public void Go()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}

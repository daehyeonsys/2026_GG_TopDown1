using UnityEngine;
using UnityEngine.Events;
public class Pickup : MonoBehaviour
{
    public UnityEvent onPick;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPick.Invoke();
            Destroy(gameObject);
        }
    }
}

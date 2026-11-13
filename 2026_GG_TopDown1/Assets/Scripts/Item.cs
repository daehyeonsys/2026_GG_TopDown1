
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory =
                other.GetComponent<Inventory>();

            if (inventory != null)
            {
                inventory.AddKey();

                Debug.Log("열쇠 획득!");

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning(
                    "Player에 Inventory가 없습니다!"
                );
            }
        }
    }
}

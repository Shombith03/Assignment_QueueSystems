using UnityEngine;

public class MoneyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Add 10 currency");
            GameManager.Instance.IncrementCurrency(10);
            Destroy(gameObject);
        }
    }
}

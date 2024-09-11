using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject box;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            box.transform.position += transform.up;
            Destroy(gameObject);
        }
    }
}

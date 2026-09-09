using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball b = other.GetComponent<Ball>();
            if (GameManager.Instance != null && b != null)
            {
                GameManager.Instance.UnregisterBall(b);
            }
            Destroy(other.gameObject);
        }
    }
}
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int points = 100;
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float dropChance = 0.35f;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterBrick();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBrickDestroyed(points);
            }

            if (powerUpPrefab != null && Random.value <= dropChance)
            {
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float fallSpeed = 3f;
    public int multiplyCount = 2;

    void Update()
    {
      
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Paddle") || collision.gameObject.name.Contains("Paddle"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DuplicateBalls(multiplyCount);
            }
            Destroy(gameObject);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    public int lives = 3;
    public int score = 0;

    [Header("References")]
    public GameObject ballPrefab;
    public Paddle paddle;

    private List<Ball> activeBalls = new List<Ball>();
    private int totalBricks = 0;
    private int initialBrickCount = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (paddle == null) paddle = FindFirstObjectByType<Paddle>();
    }

    // Được MenuManager gọi khi bấm nút Play
    public void StartGame()
    {
        isGameOver = false;
        totalBricks = 0;
        initialBrickCount = 0;
        score = 0;
        activeBalls.Clear();

        // Reset thanh tiến độ về 0%
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.UpdateProgressBar(0f);
        }

        if (paddle == null) paddle = FindFirstObjectByType<Paddle>();
        SpawnInitialBall();
    }

    public void RegisterBrick()
    {
        totalBricks++;
        initialBrickCount++;
    }

    public void OnBrickDestroyed(int points)
    {
        if (isGameOver) return;

        score += points;
        totalBricks--;
        Debug.Log("Điểm hiện tại: " + score);

        // Cập nhật Slider trên MenuManager
        if (MenuManager.Instance != null && initialBrickCount > 0)
        {
            int destroyed = initialBrickCount - totalBricks;
            MenuManager.Instance.UpdateProgressBar((float)destroyed / initialBrickCount);
        }

        // HẾT GẠCH -> CHIẾN THẮNG
        if (totalBricks <= 0)
        {
            isGameOver = true;
            Debug.Log("CHIẾN THẮNG!");

            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.ShowVictory();
            }
        }
    }

    public void RegisterBall(Ball b)
    {
        if (!activeBalls.Contains(b)) activeBalls.Add(b);
    }

    public void UnregisterBall(Ball b)
    {
        activeBalls.Remove(b);

        if (activeBalls.Count <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("HẾT BÓNG - GAME OVER!");

        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ShowLose();
        }
    }

    public void DuplicateBalls(int multiplier)
    {
        List<Ball> current = new List<Ball>(activeBalls);

        foreach (Ball orig in current)
        {
            if (orig == null) continue;
            Rigidbody2D origRb = orig.GetComponent<Rigidbody2D>();
            if (origRb == null) continue;

            Vector2 v = origRb.linearVelocity;
            float spd = orig.speed;

            for (int i = 1; i < multiplier; i++)
            {
                GameObject nbObj = Instantiate(ballPrefab, orig.transform.position, Quaternion.identity);
                Ball nb = nbObj.GetComponent<Ball>();
                Rigidbody2D nRb = nbObj.GetComponent<Rigidbody2D>();

                nb.paddle = paddle.transform;
                nb.isLaunched = true;

                float angle = Random.Range(-30f, 30f);
                Vector2 newDir = Quaternion.Euler(0, 0, angle) * v.normalized;
                nRb.linearVelocity = newDir * spd;
            }
        }
    }

    private void LoseLife()
    {
        lives--;
        Debug.Log("Mất 1 mạng! Mạng còn lại: " + lives);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            paddle.ResetPaddle();
            SpawnInitialBall();
        }
    }

    private void SpawnInitialBall()
    {
        if (ballPrefab == null || paddle == null) return;

        Vector3 spawnPos = paddle.transform.position + Vector3.up * 0.5f;
        GameObject bObj = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        Ball b = bObj.GetComponent<Ball>();
        b.paddle = paddle.transform;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
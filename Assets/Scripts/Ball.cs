using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    public Transform paddle;
    public bool isLaunched = false;

    [Header("Sound")]
    public AudioClip hitSound;

    private Rigidbody2D rb;
    private Vector3 paddleOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (paddle != null)
        {
            paddleOffset = transform.position - paddle.position;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterBall(this);
        }
    }

    void Update()
    {
        if (!isLaunched && paddle != null)
        {
            transform.position = paddle.position + paddleOffset;

            bool spacePressed =
                Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame;

            bool mouseClick =
                Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame;

            if (spacePressed || mouseClick)
            {
                Launch();
            }
        }
    }

    public void Launch()
    {
        isLaunched = true;

        Vector2 launchDirection =
            new Vector2(Random.Range(-0.2f, 0.2f), 1f).normalized;

        rb.linearVelocity = launchDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Lấy vận tốc hiện tại
        Vector2 v = rb.linearVelocity; // Nếu Unity của bạn báo đỏ chữ này, đổi thành: rb.velocity
        float currentSpeed = v.magnitude;
        if (currentSpeed < 1f) currentSpeed = 10f; // Tốc độ cơ bản dự phòng

        // 1. CHỐNG KẸT PHƯƠNG NGANG:
        // Nếu độ lệch trục Y quá nhỏ (|v.y| < 2), bóng đang bay gần như ngang tuyệt đối
        if (Mathf.Abs(v.y) < 2f)
        {
            // Ép bóng phải nảy chúc xuống dưới (hoặc bật lên trên)
            float kickY = (v.y >= 0) ? 2.5f : -2.5f;
            v.y = kickY;
        }

        // 2. CHỐNG KẸT PHƯƠNG DỌC:
        // Nếu độ lệch trục X quá nhỏ (|v.x| < 1), bóng đang nảy thẳng đứng lên xuống
        if (Mathf.Abs(v.x) < 1f)
        {
            float kickX = (Random.value < 0.5f) ? 2f : -2f;
            v.x = kickX;
        }

        // Chuẩn hóa lại hướng và gán nguyên vận tốc cũ
        rb.linearVelocity = v.normalized * currentSpeed; // Nếu Unity báo đỏ, đổi thành rb.velocity
    }

    public void ResetBall()
    {
        isLaunched = false;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;

        if (paddle != null)
        {
            transform.position =
                paddle.position + paddleOffset;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem; // Thư viện Input mới của Unity 6

public class Paddle : MonoBehaviour
{
    [Header("Control Settings")]
    [Tooltip("Tick chọn để dùng chuột, bỏ tick để dùng phím A/D")]
    public bool useMouse = true;

    public float speed = 15f;
    public float xLimit = 7.8f;

    private Vector3 initialPosition;
    private Camera mainCamera;

    void Start()
    {
        initialPosition = transform.position;
        mainCamera = Camera.main;
    }

    void Update()
    {
        float targetX = transform.position.x;

        if (useMouse && Mouse.current != null && mainCamera != null)
        {
           
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -mainCamera.transform.position.z));
            targetX = mouseWorldPos.x;
        }
        else if (Keyboard.current != null)
        {
            
            float moveInput = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput += 1f;

            targetX += moveInput * speed * Time.deltaTime;
        }

    
        targetX = Mathf.Clamp(targetX, -xLimit, xLimit);
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    public void ResetPaddle()
    {
        transform.position = initialPosition;
    }
}
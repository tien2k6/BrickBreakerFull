using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;

    [Header("Grid Size")]
    public int rows = 6;
    public int columns = 12;

    [Header("Play Area")]
    public float playAreaWidth = 14f;
    public float playAreaHeight = 3.5f;
    public float topY = 3.2f;

    [Header("Gap")]
    public float gap = 0.08f;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        if (brickPrefab == null || rows <= 0 || columns <= 0)
            return;

        // Kích thước mong muốn của mỗi viên
        float targetWidth =
            (playAreaWidth - (columns - 1) * gap) / columns;

        float targetHeight =
            (playAreaHeight - (rows - 1) * gap) / rows;

        // Lấy kích thước thật của sprite gốc
        SpriteRenderer prefabRenderer =
            brickPrefab.GetComponent<SpriteRenderer>();

        if (prefabRenderer == null ||
            prefabRenderer.sprite == null)
        {
            Debug.LogError("Brick Prefab chưa có SpriteRenderer hoặc Sprite!");
            return;
        }

        Vector2 originalSize =
            prefabRenderer.sprite.bounds.size;

        // Tính scale đúng
        float scaleX = targetWidth / originalSize.x;
        float scaleY = targetHeight / originalSize.y;

        // Khoảng cách giữa các viên
        float stepX = targetWidth + gap;
        float stepY = targetHeight + gap;

        // Căn giữa
        float startX =
            -((columns - 1) * stepX) / 2f;

        float startY = topY;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector2 pos = new Vector2(
                    startX + c * stepX,
                    startY - r * stepY
                );

                GameObject newBrick = Instantiate(
                    brickPrefab,
                    pos,
                    Quaternion.identity,
                    transform
                );

                newBrick.transform.localScale =
                    new Vector3(scaleX, scaleY, 1f);
            }
        }
    }
}
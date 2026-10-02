using UnityEngine;
using UnityEngine.InputSystem;

public class Coin : MonoBehaviour
{
    public GameManager gameManager;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Bỏ qua mọi thao tác click nếu không ở trạng thái Playing (Paused hoặc GameOver đều ngưng)
        if (gameManager == null || gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        // Kiểm tra lượt click
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue();
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(worldPoint);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                gameManager.AddScore();
                MoveToRandomPosition();
            }
        }
    }

    private void MoveToRandomPosition()
    {
        float randomX = Random.Range(-7f, 7f);
        float randomY = Random.Range(-3f, 3f);
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }
}
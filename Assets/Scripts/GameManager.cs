using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Định nghĩa đầy đủ các trạng thái của Game bằng enum
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    [Header("Game State")]
    public GameState currentState;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;       // UI hiển thị thời gian
    public TextMeshProUGUI gameOverText;    // Text thông báo Game Over
    public GameObject gameOverPanel;        // Panel chứa chữ GameOver + Nút Restart
    public GameObject pausePanel;           // Panel chứa giao diện Pause

    [Header("Game Settings")]
    public float maxTime = 30f;             // Thời lượng mỗi lượt chơi

    private float timer;
    private int score = 0;

    void Start()
    {
        // Khởi tạo game ngay khi vào Scene
        StartNewGame();
    }

    void Update()
    {
        // 1. Phím tắt Pause/Unpause (Phím Esc hoặc P)
        if (Keyboard.current != null)
        {
            // Kiểm tra nếu phím Escape hoặc phím P vừa được ấn xuống trong frame này
            if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
            {
                if (currentState == GameState.Playing)
                {
                    PauseGame();
                }
                else if (currentState == GameState.Paused)
                {
                    ResumeGame();
                }
            }
        }

        // 2. Logic đếm ngược khi đang Playing
        if (currentState == GameState.Playing)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                EndGame();
            }

            UpdateTimerUI();
        }
    }

    // --- BẮT ĐẦU / RESTART GAME ---
    public void StartNewGame()
    {
        score = 0;
        timer = maxTime;
        currentState = GameState.Playing;

        // Reset thời gian chạy game về bình thường
        Time.timeScale = 1f;

        // Ẩn các Panel UI
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    // --- XỬ LÝ CLICK COIN ---
    public void AddScore()
    {
        // CHỈ cho phép tăng điểm khi đang Playing
        if (currentState == GameState.Playing)
        {
            score++;
            UpdateScoreUI();
        }
    }

    // --- PAUSE / RESUME GAME ---
    public void PauseGame()
    {
        // Chỉ Pause được khi đang Playing
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f; // Đóng băng thời gian vật lý và Time.deltaTime

            if (pausePanel != null) pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        // Chỉ Resume được khi đang Paused
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f; // Khôi phục thời gian bình thường

            if (pausePanel != null) pausePanel.SetActive(false);
        }
    }

    // --- KẾT THÚC GAME ---
    private void EndGame()
    {
        currentState = GameState.GameOver;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverText != null) gameOverText.text = "GAME OVER";

        Debug.Log("Hết giờ! GAME OVER.");
    }

    // --- CẬP NHẬT UI ---
    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "SCORE: " + score;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null) timerText.text = "TIME: " + Mathf.CeilToInt(timer).ToString();
    }
}
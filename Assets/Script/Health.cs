using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요합니다

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public float maxHealth = 5f;
    public float currentHealth;
    [SerializeField] public Image hpBarImage;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel; // 문구와 버튼들이 담긴 부모 오브젝트

    void Awake()
    {
        currentHealth = maxHealth;
        // 시작할 때 게임오버 화면은 숨기고, 시간은 정상 흐르게 설정
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PlusHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
    }
    public void MinusHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth - value, 0, maxHealth);

        if (hpBarImage != null)
            hpBarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // 1. 게임 시간 정지
        Time.timeScale = 0f;

        // 2. 게임 오버 UI 띄우기
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // [재시작 버튼]에 연결할 함수
    public void RestartGame()
    {
        // 현재 활성화된 씬을 다시 불러옵니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // [종료 버튼]에 연결할 함수
    public void QuitGame()
    {
        Application.Quit(); // 실제 빌드된 게임 종료

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료
#endif
    }
}
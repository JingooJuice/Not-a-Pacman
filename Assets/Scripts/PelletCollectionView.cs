using TMPro;
using UnityEngine;

public class PelletCollectionView : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private IGameViewModel gameViewModel;

    public void Initialize(IGameViewModel vm)
    {
        gameViewModel = vm;
        UpdateScoreDisplay();

        // Подписываемся на изменения счета
        gameViewModel.Model.OnDataChanged += OnScoreChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pellet"))
        {
            CollectPellet(other.gameObject);
        }
    }

    private void CollectPellet(GameObject pellet)
    {
        if (gameViewModel != null)
        {
            gameViewModel.AddScore(100);
            Destroy(pellet);
            Debug.Log("Score: " + gameViewModel.Model.Score);
        }
    }

    private void OnScoreChanged()
    {
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null && gameViewModel != null)
        {
            scoreText.text = "Score: " + gameViewModel.Model.Score.ToString();
        }
    }

    public int GetScore()
    {
        return gameViewModel?.Model.Score ?? 0;
    }

    private void OnDestroy()
    {
        if (gameViewModel != null)
        {
            gameViewModel.Model.OnDataChanged -= OnScoreChanged;
        }
    }
}
using System;

public interface IGameModel
{
    int Score { get; set; }
    int Lives { get; set; }
    int MaxLives { get; set; }
    bool IsPaused { get; set; }
    bool IsGameOver { get; set; }
    bool IsGameWon { get; set; }
    event Action OnDataChanged;
}

public class GameModel : IGameModel
{
    private int score;
    private int lives;
    private int maxLives;
    private bool isPaused;
    private bool isGameOver;
    private bool isGameWon;

    public int Score { get => score; set { score = value; OnDataChanged?.Invoke(); } }
    public int Lives { get => lives; set { lives = value; OnDataChanged?.Invoke(); } }
    public int MaxLives { get => maxLives; set { maxLives = value; OnDataChanged?.Invoke(); } }
    public bool IsPaused { get => isPaused; set { isPaused = value; OnDataChanged?.Invoke(); } }
    public bool IsGameOver { get => isGameOver; set { isGameOver = value; OnDataChanged?.Invoke(); } }
    public bool IsGameWon { get => isGameWon; set { isGameWon = value; OnDataChanged?.Invoke(); } }
    public event Action OnDataChanged;
}
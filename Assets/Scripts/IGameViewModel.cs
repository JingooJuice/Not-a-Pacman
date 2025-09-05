public interface IGameViewModel
{
    IGameModel Model { get; }
    void TakeDamage(int damage);
    void ResetGameState();
    void TogglePause();
    void ResumeGame();
    void CheckWinCondition();
    void GameOver();
    void WinGame();
    int GetCurrentLife();
    bool IsDead();
    void AddScore(int points);
}

public class GameViewModel : IGameViewModel
{
    public IGameModel Model { get; private set; }

    public GameViewModel(IGameModel model)
    {
        Model = model;
        Model.MaxLives = 3;
        Model.Lives = Model.MaxLives;
    }

    public void AddScore(int points)
    {
        Model.Score += points;
    }

    public void TakeDamage(int damage)
    {
        if (Model.Lives >= 1 && !Model.IsGameOver)
        {
            Model.Lives -= damage;
            if (Model.Lives < 1)
            {
                Model.IsGameOver = true;
            }
        }
    }

    public void ResetGameState()
    {
        Model.Lives = Model.MaxLives;
        Model.Score = 0;
        Model.IsGameOver = false;
        Model.IsGameWon = false;
        Model.IsPaused = false;
    }

    public void TogglePause() => Model.IsPaused = true;
    public void ResumeGame() => Model.IsPaused = false;

    public void CheckWinCondition()
    {
        if (Model.IsGameOver || Model.IsGameWon || Model.IsPaused) return;

        var pellets = UnityEngine.GameObject.FindGameObjectsWithTag("Pellet");
        if (pellets.Length == 0)
        {
            WinGame();
        }
    }

    public void GameOver() => Model.IsGameOver = true;
    public void WinGame() => Model.IsGameWon = true;

    public int GetCurrentLife() => Model.Lives;
    public bool IsDead() => Model.IsGameOver;
}
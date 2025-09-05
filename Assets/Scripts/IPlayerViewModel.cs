using UnityEngine;

public interface IPlayerViewModel
{
    IPlayerModel PlayerModel { get; }
    IGameViewModel GameViewModel { get; }
    void Initialize(Vector3 startPosition);
    void TakeDamage();
    void Respawn();
    bool CanTakeDamage();
}

public class PlayerViewModel : IPlayerViewModel
{
    public IPlayerModel PlayerModel { get; private set; }
    public IGameViewModel GameViewModel { get; private set; }

    public PlayerViewModel(IPlayerModel playerModel, IGameViewModel gameViewModel)
    {
        PlayerModel = playerModel;
        GameViewModel = gameViewModel;
        PlayerModel.InvulnerabilityTime = 2f;
    }

    public void Initialize(Vector3 startPosition)
    {
        PlayerModel.StartPosition = startPosition;
    }

    public void TakeDamage()
    {
        if (!CanTakeDamage()) return;

        GameViewModel.TakeDamage(1);
        PlayerModel.IsInvulnerable = true;
        PlayerModel.TriggerDamageTaken(); // Вызываем через метод модели
    }

    public void Respawn()
    {
        PlayerModel.TriggerRespawn(); // Вызываем через метод модели
    }

    public bool CanTakeDamage()
    {
        return !PlayerModel.IsInvulnerable &&
               !GameViewModel.IsDead() &&
               !GameViewModel.Model.IsPaused;
    }
}
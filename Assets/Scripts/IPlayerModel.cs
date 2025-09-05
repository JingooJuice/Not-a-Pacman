using System;
using UnityEngine;

public interface IPlayerModel
{
    bool IsInvulnerable { get; set; }
    float InvulnerabilityTime { get; set; }
    Vector3 StartPosition { get; set; }
    event Action OnDamageTaken;
    event Action OnRespawn;

    // Методы для вызова событий из самого класса
    void TriggerDamageTaken();
    void TriggerRespawn();
}

public class PlayerModel : IPlayerModel
{
    private bool isInvulnerable;
    private float invulnerabilityTime;
    private Vector3 startPosition;

    public bool IsInvulnerable { get => isInvulnerable; set => isInvulnerable = value; }
    public float InvulnerabilityTime { get => invulnerabilityTime; set => invulnerabilityTime = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public event Action OnDamageTaken;
    public event Action OnRespawn;

    // Вызов событий из самого класса модели
    public void TriggerDamageTaken() => OnDamageTaken?.Invoke();
    public void TriggerRespawn() => OnRespawn?.Invoke();
}
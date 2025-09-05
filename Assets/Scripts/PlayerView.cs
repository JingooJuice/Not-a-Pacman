using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float smoothRotationTime = 0.01f;
    [SerializeField] private float smoothStopTime = 0.1f;

    [Header("Mobile Input")]
    public bool enableMobileInputs = false;
    public FixedJoystick joystick;

    [Header("Dependencies")]
    [SerializeField] private EnemyManager enemyManager;

    private Rigidbody body;
    private IPlayerViewModel viewModel;
    private float currentVelocity;
    private float currentSpeed;
    private float speedVelocity;
    private Renderer playerRenderer;

    public void Initialize(IPlayerViewModel vm)
    {
        viewModel = vm;
        viewModel.PlayerModel.OnDamageTaken += OnDamageTaken;
        viewModel.PlayerModel.OnRespawn += OnRespawn;

        viewModel.Initialize(transform.position);
    }

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        if (enemyManager == null)
            enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        if (viewModel?.GameViewModel.Model.IsPaused == true ||
            viewModel?.GameViewModel.Model.IsGameOver == true)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = GetInput();
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                ref currentVelocity, smoothRotationTime);
            transform.eulerAngles = Vector3.up * rotation;

            float targetSpeed = moveSpeed * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothStopTime);

            body.linearVelocity = new Vector3(inputDir.x, 0, inputDir.y) * currentSpeed;
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref speedVelocity, smoothStopTime);
            body.linearVelocity = Vector3.zero;
        }
    }

    private Vector2 GetInput()
    {
        if (enableMobileInputs && joystick != null)
        {
            return new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && viewModel != null)
        {
            viewModel.TakeDamage();
        }
    }

    private void OnDamageTaken()
    {
        StartCoroutine(BlinkEffect());
        Invoke(nameof(ResetInvulnerability), viewModel.PlayerModel.InvulnerabilityTime);
        viewModel.Respawn(); // Вызываем респавн через ViewModel
    }

    private void ResetInvulnerability()
    {
        if (viewModel != null)
            viewModel.PlayerModel.IsInvulnerable = false;

        if (playerRenderer != null)
            playerRenderer.enabled = true;
    }

    private IEnumerator BlinkEffect()
    {
        if (playerRenderer == null) yield break;

        for (int i = 0; i < 6; i++)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(0.33f);
        }
        playerRenderer.enabled = true;
    }

    private void OnRespawn()
    {
        transform.position = viewModel.PlayerModel.StartPosition;
        body.linearVelocity = Vector3.zero;
        RespawnEnemies();
    }

    private void RespawnEnemies()
    {
        if (enemyManager != null)
        {
            enemyManager.RespawnAllEnemies();
        }
        else
        {
            Debug.LogWarning("EnemyManager not found");
        }
    }

    public bool IsInvulnerable()
    {
        return viewModel?.PlayerModel.IsInvulnerable ?? false;
    }

    private void OnDestroy()
    {
        if (viewModel != null)
        {
            viewModel.PlayerModel.OnDamageTaken -= OnDamageTaken;
            viewModel.PlayerModel.OnRespawn -= OnRespawn;
        }
    }
}
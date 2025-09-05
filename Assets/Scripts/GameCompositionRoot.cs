using UnityEngine;

public class GameCompositionRoot : MonoBehaviour
{
    [SerializeField] private GameManagerView gameManagerView;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PelletCollectionView pelletCollectionView;

    private IGameModel gameModel;
    private IGameViewModel gameViewModel;
    private IPlayerModel playerModel;
    private IPlayerViewModel playerViewModel;

    private void Awake()
    {
        // ������� ������ � ViewModel
        gameModel = new GameModel();
        gameViewModel = new GameViewModel(gameModel);

        playerModel = new PlayerModel();
        playerViewModel = new PlayerViewModel(playerModel, gameViewModel);

        // �������������� ����������
        if (gameManagerView != null)
            gameManagerView.Initialize(gameViewModel);

        if (playerView != null)
            playerView.Initialize(playerViewModel);

        if (pelletCollectionView != null)
            pelletCollectionView.Initialize(gameViewModel);
    }

    public IGameViewModel GetGameViewModel() => gameViewModel;
    public IPlayerViewModel GetPlayerViewModel() => playerViewModel;
}
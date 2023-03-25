using UnityEngine;
using Cinemachine;
public enum GameStates // ��� ��������� ��� ���������� ���� ���� �� ������� ������ 
{
    Menu,Game,Die
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] private GameObject PlayerPrefab; 
    [SerializeField] private PlatformGenerator PlatformPrefab; // ������ ����� ���������

    [SerializeField] private Transform PointSpawnPlatform; // ������� ������ ��������� 
    [SerializeField] private Transform PointSpawnPlayer; // ������� ��� ������ ������ 

    [SerializeField] private GameStates _gameState;

    [SerializeField] private GameObject MenuUI, DieUI; //������� ���� ������� ���� � ���� DIE

    PlayerMovement player;
    int CountPlatform;
    GameStates GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            StateGame();

        }
    }
   
    public void Update()
    {
      
    }
    public void StartGame() //��������� ������������ ���������� �� Event �������, ����� ����� �������� ������( ����� UI-> Canvas-> StartGame) 
    {
        SetGameState(stateGameParam: GameStates.Game);
    }

    // Start is called before the first frame update
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }else if(instance == this){
            Destroy(gameObject);
        }
        SetGameState(stateGameParam: GameStates.Menu); // ������ ��������� ��������� �� ������������� MENU
    }
    #region GameStateController

    public void SetGameState(GameStates stateGameParam) // ������ ��������� ����
    {
        GameState = stateGameParam;
    }
    public void SetPlayerSide(bool set) // ������ �������� ���������� ��� ���������� ������������ � �������
    {
        player.SetBoolSide(param: set);
    }
    public void SetPlayerDie() // �������� ��������� ������ ������
    {
        player.SetState(playerState: PlayerState.Die);
        SetGameState(stateGameParam: GameStates.Die);
    }
    public int GetCountPlayerCube() // ��������� ���������� ������� � ������ 
    {
        return player.CheckCubeCount();

    }

    private void StateGame()
    {
        switch (GameState)
        {
            case GameStates.Menu: // ������� ��� ���� 
                SpawnPlayer();
                for (int i = 0; i < 3; i++)
                {
                    SpawnPlatform(position: PointSpawnPlatform, zCount: -30f);
                }
                MenuUI.SetActive(true);
                break;

            case GameStates.Game: // ������� �� ����� ����
                player.SetState(playerState: PlayerState.Move);
                break;

            case GameStates.Die: // ������� ������ 
                DieUI.SetActive(value: true);
                break;


        }
    } // ��������� ���� 
    #endregion

    #region SpawnManager
    public void SpawnPlatform(Transform position , float zCount)
    {
       
        GameObject platform = Pooler.instance.GetPoolObject(typeGet: PlatformPrefab.Type);
        platform.GetComponent<PlatformGenerator>().ActivateThis(pos: new Vector3(position.position.x, position.position.y, zCount*CountPlatform),rot: position.rotation);
        platform.GetComponent<PlatformGenerator>().GenerateNew();
        CountPlatform++;
    }// ������� ��������� (������������ Pool)

    public void RestartGame()
    {
        Pooler.instance.FindAllObjectAndDisableAndAddtoPool();
       CountPlatform = 0;
       Destroy(player.gameObject);
        SpawnPlayer();

        for (int i = 0; i < 3; i++)
       {
           SpawnPlatform(position: PointSpawnPlatform, zCount: -30f);
       }
       SetGameState(GameStates.Game);
       DieUI.SetActive(false);


    } // ����� �������� ( ���������� �� ������ UI->Canvas->EndGame->Button)
    private void SpawnPlayer()
    {
       GameObject playerInsta = Instantiate(PlayerPrefab, PointSpawnPlayer.position, Quaternion.identity);
       player = playerInsta.GetComponent<PlayerMovement>();
       player.SetState(PlayerState.Stay);
        CameraShake.instance.Player = playerInsta.transform;
    }// ����� ������ ������ 
    #endregion
}

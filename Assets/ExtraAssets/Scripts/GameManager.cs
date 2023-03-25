using UnityEngine;
using Cinemachine;
public enum GameStates // Три состояния для определния типа игры на текущий момент 
{
    Menu,Game,Die
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] private GameObject PlayerPrefab; 
    [SerializeField] private PlatformGenerator PlatformPrefab; // Префаб нашей платформы

    [SerializeField] private Transform PointSpawnPlatform; // Позиция спауна платформы 
    [SerializeField] private Transform PointSpawnPlayer; // Позиция для спауна игрока 

    [SerializeField] private GameStates _gameState;

    [SerializeField] private GameObject MenuUI, DieUI; //Объекты двух панелей МЕНЮ и окна DIE

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
    public void StartGame() //Запускаем передвижение персонажем из Event события, когда игрок касается экрана( найти UI-> Canvas-> StartGame) 
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
        SetGameState(stateGameParam: GameStates.Menu); // Задаем стартовое состояние на инициализицию MENU
    }
    #region GameStateController

    public void SetGameState(GameStates stateGameParam) // Задаем состояние игры
    {
        GameState = stateGameParam;
    }
    public void SetPlayerSide(bool set) // Задаем значение переменной для разрешения передвижения в стороны
    {
        player.SetBoolSide(param: set);
    }
    public void SetPlayerDie() // Передаем состояние смерти игроку
    {
        player.SetState(playerState: PlayerState.Die);
        SetGameState(stateGameParam: GameStates.Die);
    }
    public int GetCountPlayerCube() // Проверяем количество кубиков у игрока 
    {
        return player.CheckCubeCount();

    }

    private void StateGame()
    {
        switch (GameState)
        {
            case GameStates.Menu: // События для меню 
                SpawnPlayer();
                for (int i = 0; i < 3; i++)
                {
                    SpawnPlatform(position: PointSpawnPlatform, zCount: -30f);
                }
                MenuUI.SetActive(true);
                break;

            case GameStates.Game: // Событие во время игры
                player.SetState(playerState: PlayerState.Move);
                break;

            case GameStates.Die: // Событие смерти 
                DieUI.SetActive(value: true);
                break;


        }
    } // Состояния игры 
    #endregion

    #region SpawnManager
    public void SpawnPlatform(Transform position , float zCount)
    {
       
        GameObject platform = Pooler.instance.GetPoolObject(typeGet: PlatformPrefab.Type);
        platform.GetComponent<PlatformGenerator>().ActivateThis(pos: new Vector3(position.position.x, position.position.y, zCount*CountPlatform),rot: position.rotation);
        platform.GetComponent<PlatformGenerator>().GenerateNew();
        CountPlatform++;
    }// Создаем платформы (используется Pool)

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


    } // Метод рестарта ( вызывается по кнопке UI->Canvas->EndGame->Button)
    private void SpawnPlayer()
    {
       GameObject playerInsta = Instantiate(PlayerPrefab, PointSpawnPlayer.position, Quaternion.identity);
       player = playerInsta.GetComponent<PlayerMovement>();
       player.SetState(PlayerState.Stay);
        CameraShake.instance.Player = playerInsta.transform;
    }// Спаун нашего игрока 
    #endregion
}

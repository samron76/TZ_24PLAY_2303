using UnityEngine;

public enum PlayerState
{
    Move,Die,Stay
}
public class PlayerMovement : MonoBehaviour
{
    [Header("Скорость передвижения")]
    [Min(0)] public float SpeedMoveForward;
    [Min(0)] public float SpeedMoveSideway;

    [Header("Крайняя точка по оси X нашей платформы")]
    public float WidthPlatform = 2f;

    [Header("Переменные для персонажа")]
    [SerializeField] private Animator AnimPlayer;
    [Tooltip("Сюда вносить объект который является родителем для нашей модели персонажа, на родителе должны находится <Rigidbody> и <Collider>")]
    [SerializeField] private Transform PlayerModel;
    [SerializeField] private RagdollActivate ragdollPlayer;

    [Header("Переменные для куба")]
    [SerializeField] private GameObject PrefabCube;
    [Tooltip("Сюда вносить объект который будет родителем для наших кубов")]
    [SerializeField] private Transform CubeForChild;

    [SerializeField] private Transform PointForSpawnScore;

    [Header("VFX стака куба")]
    [SerializeField] private ParticleSystem VFXCubeStack;
    [SerializeField] private ParticleSystem VFXWarpEffect;

    private PlayerState _state = PlayerState.Stay;
    private bool isSide = true;

    PlayerState State 
    {
        get => _state;
        set
        {
            _state = value;
            StatePlayer(); //задаем состояние при обновлении параметра 
        }
    }
   public void SetBoolSide(bool param)
    {
        isSide = param;
    }
    public void SetState(PlayerState playerState)
    {
        State = playerState;
    }

    private void Awake()
    {
        
        ragdollPlayer.GetAllComponentIntoPlayer(AnimPlayer.transform); //Получаем все компоненты Rigidbody и Collider 
        ragdollPlayer.IsActivateRagdoll(IsActivate: false); // Отключаем рагдол 
        isSide = true;
        //State = PlayerState.Move;
       // SetState(PlayerState.mo)
    }
    // Start is called before the first frame update


    void Update()
    {
       
        StatePlayer();
     
    }
    private void SwitchStateAnim(string Param, bool State) // переключение анимации на прыжок 
    {
        AnimPlayer.SetBool(name: Param, value: State);
    }
    #region CubeEvent
    public void AddCube( ) // добавляем куб под игрока ( через Pooler )
    {
        VFXCubeStack.Play();
        SwitchStateAnim(Param: "Jump", State: true);

        PlayerModel.position = new Vector3(PlayerModel.position.x, PlayerModel.position.y + 1.5f, PlayerModel.position.z);

        int Counts = CubeForChild.childCount;
        GameObject CubeIsnta = Pooler.instance.GetPoolObject(Pooler.ObjectinPool.TypeObject.BOX);
        CubeIsnta.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        CubeIsnta.GetComponent<PickupBox>().SetBool(true);
        CubeIsnta.transform.SetParent(p: CubeForChild);

        Vector3 position = new Vector3(CubeForChild.position.x, CubeForChild.position.y + Counts + 0.2f, CubeForChild.position.z);
        CubeIsnta.GetComponent<PickupBox>().ActivateThis(position, Quaternion.identity);
        CubeIsnta.GetComponent<Rigidbody>().isKinematic = false;
        CubeIsnta.GetComponent<Rigidbody>().useGravity = true;

        GameObject scoreAdd = Pooler.instance.GetPoolObject(Pooler.ObjectinPool.TypeObject.SCORE);
        scoreAdd.GetComponent<PoolItem>().ActivateThis(PointForSpawnScore.position, PointForSpawnScore.rotation);
        scoreAdd.GetComponent<ScoreAdd>().StartTimer();
    }
    public int CheckCubeCount()
    {
       if(CubeForChild.childCount == 0 )
        {
            SetState(PlayerState.Die);
            GameManager.instance.SetGameState(GameStates.Die);
            return 0;
        }
        return 1;
    } // Проверяем количество кубиков игрока, если равно 0 - то игрок погибает и передается в менеджер состояние игры
    #endregion
   
    #region PlayerStates
    private void StatePlayer() // обработчик состояния игрока 
    {
        switch (State)
        {
            case PlayerState.Move: //вызывает метод движения
                Move();
                break;

            case PlayerState.Die: //вызывает метод смерти 
                Die();

                break;

            case PlayerState.Stay: //вызывает состояние неподвижности 
                VFXWarpEffect.Stop();

                break;


        }
    }
    private void Move() //передвижение нашего игрока 
    {
        if (Input.GetMouseButton(0) && isSide)
        {
            float halfScreen = Screen.width / 2;
            float xPos = -(Input.mousePosition.x - halfScreen) / halfScreen;
            var Position = transform.position += new Vector3(xPos * SpeedMoveSideway * Time.deltaTime, 0,0); // переджвиение по X
            transform.position = GetLimitPos(position : Position); // Расчёт позиции для передвижения в пределах платформы
        }
        transform.position += new Vector3(0, 0, -SpeedMoveForward * Time.deltaTime); // передвижение по Z
        VFXWarpEffect.Play();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EndPlatform")
        {
            GameManager.instance.SpawnPlatform(position: other.transform.parent.transform, zCount:-30f); // Создаем платформу
            other.transform.parent.GetComponent<PlatformGenerator>().DisableAllWall(); // отключаем все стенки прошлой платформы и отправляем в пулл
            other.transform.parent.GetComponent<PlatformGenerator>().DisableThis(); // Отправляем в пул платформу 
        }
        if(other.tag == "WallTrigger")
        {
            foreach (Collider cubes in CubeForChild.GetComponentsInChildren<Collider>())
            {
                cubes.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

                // cubes.transform.parent = null;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WallTrigger")
        {
            foreach (Collider cubes in CubeForChild.GetComponentsInChildren<Collider>())
            {
                cubes.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation ;


                // cubes.transform.parent = null;
            }
        }
    }

    private void Die() //Вызываете когда игрок умер 
    {
        ragdollPlayer.IsActivateRagdoll(IsActivate: true);
        PlayerModel.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        PlayerModel.gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Collider cubes in CubeForChild.GetComponentsInChildren<Collider>())
        {
            cubes.transform.GetComponent<Rigidbody>().useGravity = true;
            cubes.transform.parent = null;
        }
        VFXWarpEffect.Stop();

        AnimPlayer.enabled = false;
    }
    private Vector3 GetLimitPos(Vector3 position) // запрет передвижения по оси X 
    {
        position.x = Mathf.Clamp(value: position.x,min: -WidthPlatform,max: WidthPlatform);
        return position;
    }
    #endregion
}

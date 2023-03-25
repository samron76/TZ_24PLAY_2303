using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance = null;
    public Transform Player { get; set; }
    [SerializeField] private float offset;
    private CinemachineVirtualCamera virtucalCamera;
    private float Timer;
    private CinemachineBasicMultiChannelPerlin perlin;
    // Start is called before the first frame update
    private void Awake()
    {
       
            instance = this;
      
        virtucalCamera = GetComponent<CinemachineVirtualCamera>();// Определяем компонент нашей камеры
        perlin = virtucalCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); // определяем компонент для тряски 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            transform.position = new Vector3(transform.position.x , transform.position.y , Player.position.z + offset );
        }
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                ShakeCamera(0, 0);
            }
        }
       
    }

    public void ShakeCamera(float Impulse , float Time) 
    {

        perlin.m_AmplitudeGain = Impulse;
        Timer = Time;
    } // Метод тряски камеры через импульс и таймер 
}

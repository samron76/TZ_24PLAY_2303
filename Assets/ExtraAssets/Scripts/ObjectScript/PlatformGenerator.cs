
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : PoolItem 
{
  [SerializeField] private Transform[] pointForSpawnCube; // ����� ������ ����� �������
  [SerializeField] private PickupBox PrefabCube; // ������ ������
  [SerializeField] private float SpawnRangeCube; // ������ �������� 

  [SerializeField] private Transform pointForSpawnObstacles; // ����� ������ ������ �����������
  [SerializeField] private Wall PrefabWall; // ������ ������ ������ 

  [SerializeField] private int obstacleWidth = 5; // ������ �����������
  [SerializeField] private int obstacleHeight = 5; // ������ �����������

    List<GameObject> cubes = new List<GameObject>();

    void Start()
    {
        //GenerateNew();
    }
    
    public void GenerateNew() // ������� ����� ��������� ��������� 
    {
       GenerateObstacle(pointForSpawnObstacles); // �� ������ ���������� ����������� 

       GenerateCube(); // ���������� ���� ������ ��� ����� 
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
          //  Pooler.instance.FindAllObjectAndDisableAndAddtoPool();
            GameManager.instance.SpawnPlatform(transform,-30f);
            GenerateObstacle(pointForSpawnObstacles);
        }
    }
    #region Generate 
    private void GenerateCube() 
    {
        for (int i = 0; i < pointForSpawnCube.Length; i++)
        {
            Vector3 point = new Vector3(x: pointForSpawnCube[i].position.x + Random.Range(-SpawnRangeCube, SpawnRangeCube),
                y: pointForSpawnCube[i].position.y,
                z: pointForSpawnCube[i].position.z
                );
            var Cube = Pooler.instance.GetPoolObject(PrefabCube.Type);
            Cube.GetComponent<PickupBox>().ActivateThis(point, Quaternion.identity);
            Cube.GetComponent<Rigidbody>().isKinematic = true;
           // Cube.GetComponent<Rigidbody>().useGravity = true;
        }
    } // ��������� ������� ��� ����� ( ����� Pooler)
    private void GenerateObstacle(Transform startPoint)
    {
        cubes.Clear();
        obstacleHeight = Random.Range(3, 4);

        int centerX = Random.Range(1, obstacleWidth );
        int centerZ = Random.Range(1, obstacleHeight );

        int cubeCount = Random.Range(15, 26);

        // �������� �������� ����� ��� ����������
        bool excludeHorizontal = Random.value < 0.5f;
        int excludeIndex = Random.Range(1, excludeHorizontal ? obstacleHeight : obstacleWidth);

        // ��������� ���������� ������������ ������ ����� � ���������� �� ��� ������� ����������
        Vector3 centerPos = startPoint.position + startPoint.right * centerX + startPoint.forward * centerZ;

        // ���������� �����������
        for (int i = 0; i < obstacleWidth && cubes.Count < cubeCount; i++)
        {
            for (int j = 0; j < obstacleHeight && cubes.Count < cubeCount; j++)
            {
                
                // ���������, ��� �� �� ������� ����� ����� � ��������� �����
                if ((excludeHorizontal && j != excludeIndex) || (!excludeHorizontal && i != excludeIndex))
                {
                    Vector3 pos = centerPos + startPoint.right * (i - centerX) + startPoint.forward * (j - centerZ);
                    Quaternion rot = startPoint.rotation;
                    if (j != Random.Range(0, 5) || i != Random.Range(1, 5));
                    {
                        GameObject cube = Pooler.instance.GetPoolObject(PrefabWall.Type);
                        cube.SetActive(true);
                        cube.GetComponent<Wall>().ActivateThis(pos, rot);
                        cube.GetComponent<Collider>().enabled = true;
                          //  Instantiate(PrefabWall, pos, rot);
                       cubes.Add(cube);
                    }
                }
                else { // ������� ������� �� ������ ����� 
                    Vector3 pos = centerPos + startPoint.right * (i - centerX) + startPoint.forward * (j - centerZ);
                    Quaternion rot = startPoint.rotation;

                    if (j == 0)
                    {
                        GameObject cube = Pooler.instance.GetPoolObject(PrefabWall.Type);
                        cube.GetComponent<Wall>().ActivateThis(pos, rot);
                        cube.SetActive(true);
                        cube.GetComponent<Collider>().enabled = true;


                        cubes.Add(cube);
                    }
                }
               
            }
        }
      // for(int x = 0; x < cubes.Count; x++)
      // {
      //     cubes[x].transform.SetParent(startPoint);
      // }
       
        
    } // ��������� �����������  (����� Pooler)


    public void DisableAllWall()
    {
       foreach(GameObject a in cubes)
       {
           a.GetComponent<Wall>().DisableThis();
           a.GetComponent<Collider>().enabled = true;
       }
        cubes.Clear();
    }
    #endregion

    

}

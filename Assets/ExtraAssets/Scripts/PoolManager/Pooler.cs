using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler instance = null;

    [System.Serializable]
    public struct ObjectinPool
    {
        public enum TypeObject
        {
            BOX,WALL, PLATFORM, SCORE
        }
        public TypeObject Type;
        public GameObject Prefab;
        public int StartCount;


    } // Структура объекта для пула 

    [SerializeField] private List<ObjectinPool> poolList; // Наш пул лист с предметами 

    Dictionary<ObjectinPool.TypeObject, PoolObject> DictPools; // Словарь с нашими объектами пула 

    
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        StartPool(); 

    }
    private void StartPool()
    {
        DictPools = new Dictionary<ObjectinPool.TypeObject, PoolObject>();
        var empty = new GameObject();
        foreach(ObjectinPool a in poolList)
        {
            var container = Instantiate(empty, transform);
            container.name = a.Type.ToString();
            DictPools[a.Type] = new PoolObject(container.transform);
            for(int i = 0; i < a.StartCount; i++)
            {
                GameObject obj = InstantiatePoolObject(a.Type, container.transform);
                DictPools[a.Type].poolObjects.Enqueue(obj);
            }
        }
        Destroy(empty);
    } // Инициализация пула с стандартными параметрами 
    public GameObject GetPoolObject(ObjectinPool.TypeObject typeGet)
    {
        GameObject objFromPool;
        if (DictPools[typeGet].poolObjects.Count > 0)
        {
            objFromPool  = DictPools[typeGet].poolObjects.Dequeue();
        }
        else
        {
            objFromPool = InstantiatePoolObject(type: typeGet ,DictPools[typeGet].Container );
        }
        objFromPool.SetActive(true);
        return objFromPool;
    } //Получаем объект из пула по типу , если его нет то создаем и добавляем в пул 
   
    public void FindAllObjectAndDisableAndAddtoPool()
 {
     PoolItem[] item = (PoolItem[]) FindObjectsOfType(typeof(PoolItem));
     foreach(PoolItem a in item)
     {
         if (a.gameObject.activeInHierarchy == true)
         {
             a.DisableThis();
         }
     }
     
 }// Ищем все обїекты на сцене с классом PoolItem, выключаем их и добавляем в пулл
    public void DisableObjectAndAddtoPool(GameObject objectAdd)
    {
        DictPools[objectAdd.GetComponent<IPoolObject>().Type].poolObjects.Enqueue(objectAdd);
        objectAdd.SetActive(false);
    } //Отключаем объект и добавляем в пул 
    private GameObject InstantiatePoolObject(ObjectinPool.TypeObject type, Transform parent) {

        GameObject obj = Instantiate(poolList.Find(x => x.Type == type).Prefab, parent);
        obj.SetActive(true);
        return obj; 
    } // Создаем объект для пула 
    
}

using UnityEngine;

public class PoolItem : MonoBehaviour , IPoolObject
{
   //реализуем интерфейс "iPoolObject"

    public Pooler.ObjectinPool.TypeObject Type => _type;
    [SerializeField] private Pooler.ObjectinPool.TypeObject _type;

    public void ActivateThis(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    } // При активации задаем позицию и поворот 
    public void DisableThis()
    {
        Pooler.instance.DisableObjectAndAddtoPool(gameObject);
    } // Отключаем объект и помещаем в пулл
}

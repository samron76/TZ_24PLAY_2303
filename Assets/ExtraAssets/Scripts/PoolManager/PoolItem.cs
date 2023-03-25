using UnityEngine;

public class PoolItem : MonoBehaviour , IPoolObject
{
   //��������� ��������� "iPoolObject"

    public Pooler.ObjectinPool.TypeObject Type => _type;
    [SerializeField] private Pooler.ObjectinPool.TypeObject _type;

    public void ActivateThis(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    } // ��� ��������� ������ ������� � ������� 
    public void DisableThis()
    {
        Pooler.instance.DisableObjectAndAddtoPool(gameObject);
    } // ��������� ������ � �������� � ����
}

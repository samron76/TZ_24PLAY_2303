
using UnityEngine;

public class Wall : PoolItem
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            collision.transform.parent = null;
            //collision.transform.GetComponent<Rigidbody>().isKinematic = true;
            if (GameManager.instance.GetCountPlayerCube() != 0)
            {
               // gameObject.GetComponent<BoxCollider>().enabled = false;

            }; // ����� ������������ ��������� ���������� ������� � ������ 
            collision.gameObject.GetComponent<PickupBox>().SetBool(false); // �������� ��������� ��� ����� ������ �� �������� ������ ������
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;

            GameManager.instance.SetPlayerSide(false); // ��������� ����������� �������� ����� ������ ������ �������� ������
            CameraShake.instance.ShakeCamera(1, 0.1f); // ������ ������ ��� ������������ 
           
        }
        if (collision.gameObject.tag == "PlayerModel")
        {
            GameManager.instance.SetPlayerDie();// ������ ���������� ����� ���� ��������, ��� ����� ���� 
        }

    }
    
}

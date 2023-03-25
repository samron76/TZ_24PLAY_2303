
using UnityEngine;

public class PickupBox : PoolItem
{
   [SerializeField ]private bool isPlayerBox = false;

    public void SetBool(bool State) => isPlayerBox = State; // ������ ��������� 
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPlayerBox != true)
        {
            Pooler.instance.DisableObjectAndAddtoPool(gameObject); // ��������� ������ � ��������� � ��� 
            other.gameObject.GetComponent<PlayerMovement>().AddCube(); // ��������� ����� ��� ���������
            

            // SetBool(true);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            GameManager.instance.SetPlayerSide(true);

        }
    }
}

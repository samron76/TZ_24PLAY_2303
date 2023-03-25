
using UnityEngine;

public class PickupBox : PoolItem
{
   [SerializeField ]private bool isPlayerBox = false;

    public void SetBool(bool State) => isPlayerBox = State; // задаем состояние 
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPlayerBox != true)
        {
            Pooler.instance.DisableObjectAndAddtoPool(gameObject); // Отключаем объект и добавляем в пул 
            other.gameObject.GetComponent<PlayerMovement>().AddCube(); // добавляем кубик под персонажа
            

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

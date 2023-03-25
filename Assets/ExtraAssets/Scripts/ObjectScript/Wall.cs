
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

            }; // После столкновения проверяем количество кубиков у игрока 
            collision.gameObject.GetComponent<PickupBox>().SetBool(false); // Передаем состояние что кубик больше не является частью игрока
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;

            GameManager.instance.SetPlayerSide(false); // Отключаем возможность поворота когда кубики игрока касаются стенки
            CameraShake.instance.ShakeCamera(1, 0.1f); // Трясем камеру при столкновении 
           
        }
        if (collision.gameObject.tag == "PlayerModel")
        {
            GameManager.instance.SetPlayerDie();// Задаем состотяние через Гейм Менеджер, что игрок умер 
        }

    }
    
}

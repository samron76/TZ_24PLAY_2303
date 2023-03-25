using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivate : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] ListRigidPlayer;
    [SerializeField]
    private Collider[] ListCollPlayer;
 
    // �������� ������ ���� ����������� "RigidBody" � "Collider" � ��������� <Player>
    public void GetAllComponentIntoPlayer(Transform player)
    {
        ListRigidPlayer = player.GetComponentsInChildren<Rigidbody>();
        ListCollPlayer = player.GetComponentsInChildren<Collider>();
    }
    

    
   //���������� ������
    public void IsActivateRagdoll(bool IsActivate)
    {
        for(int i = 0; i < ListRigidPlayer.Length; i ++)
        {
            ListRigidPlayer[i].isKinematic = !IsActivate;

        }
        for (int i = 0; i < ListCollPlayer.Length; i++)
        {
            ListCollPlayer[i].enabled = IsActivate;

        }

    }
}

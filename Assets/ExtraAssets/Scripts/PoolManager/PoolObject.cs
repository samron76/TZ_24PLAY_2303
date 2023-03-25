using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject
{
    public Transform Container;

    public Queue<GameObject> poolObjects;
    public PoolObject(Transform container)
    {
        Container = container;
        poolObjects = new Queue<GameObject>();
    }
   
  
}

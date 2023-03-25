using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdd : PoolItem
{
   public void StartTimer()
    {
        StartCoroutine(LateCall(1.5f));
    }
    IEnumerator LateCall(float seconds)
    {      
        yield return new WaitForSeconds(seconds);

        DisableThis();
       
    }
}

using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box" || other.tag == "Wall")
        {
            other.GetComponent<PoolItem>().DisableThis();
        }
    }
}

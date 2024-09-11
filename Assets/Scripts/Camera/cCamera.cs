using UnityEngine;

public class cCamera : MonoBehaviour
{
    
    [SerializeField]
    private GameObject target;
    
    [SerializeField]
    Vector3 offset;
    
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        
        transform.position =  desiredPosition;
    }
}

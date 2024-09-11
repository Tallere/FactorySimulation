using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMouse3D : MonoBehaviour {

    public static cMouse3D Instance { get; private set; }

    [SerializeField] 
    private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake() 
    {
        Instance = this;
    }

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) 
        {
            return raycastHit.point;
        } 
        else 
        {
            return Vector3.zero;
        }
    }

}

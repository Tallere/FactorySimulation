using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPlacedObject : MonoBehaviour
{
    public static cPlacedObject Create(Vector3 worldPosistion, Vector2Int origin, cMachine.Dir dir, cMachine machine )
    {
        Transform placedObjectTransform = Instantiate(machine.GetMachineModel.transform, worldPosistion, Quaternion.Euler(0, machine.GetRotationAngle(dir), 0));
        
        cPlacedObject placedObject = placedObjectTransform.GetComponent<cPlacedObject>();
        
        placedObject.origin = origin;
        
        placedObject.placedMachine = machine;
        
        placedObject.dir = dir;
        
        return placedObject;
    }
    
    private cMachine placedMachine;
    
    private Vector2Int origin;
    
    private cMachine.Dir dir;

    public List<Vector2Int> GetGridPositionList()
    {
        return placedMachine.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

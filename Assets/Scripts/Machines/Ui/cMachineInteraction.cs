using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cMachineInteraction : MonoBehaviour
{
    protected bool isPanelOpen = false;
    
    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {               
                    Interact();
                }
            }
        }
    }

    protected virtual void Interact()
    {
        
    }
}

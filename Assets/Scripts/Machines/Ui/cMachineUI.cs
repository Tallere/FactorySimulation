using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMachineUI : MonoBehaviour
{
    
    [SerializeField]
    public GameObject machineUiPanel;
    
    [SerializeField]
    protected GameObject inventoryUiPanel;
    
    protected bool isPanelOpen = false;
    
    [SerializeField] 
    protected cMachine machine;
    
    protected virtual void Start()
    {
        machineUiPanel.SetActive(false);    
    }
    
    public virtual void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        machineUiPanel.SetActive(isPanelOpen);
        inventoryUiPanel.SetActive(isPanelOpen);
    }

    protected virtual void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            if (hit.transform == machine.GetMachineModel.transform && Input.GetMouseButtonDown(0))
            {
                TogglePanel();
            }
        }
    }
}

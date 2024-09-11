using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cFurnaceInteraction : cMachineInteraction
{
    public cFurnaceUI furnaceUI;

    protected void Start()
    {
        if (furnaceUI == null)
        {
            Debug.LogError("cFurnaceUI component not assigned.");
        }
    }
    
    protected override void Update()
    {
        base.Update();
    }


    protected override void Interact()
    {
        furnaceUI.TogglePanel();
    }
}

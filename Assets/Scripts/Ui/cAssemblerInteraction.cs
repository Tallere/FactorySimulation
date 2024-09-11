using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAssemblerInteraction : cMachineInteraction
{
    public cAssemblerUI assemblerUI;

    protected void Start()
    {
        if (assemblerUI == null)
        {
            Debug.LogError("cAssemblerUI component not assigned.");
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Interact()
    {
        assemblerUI.TogglePanel();
    }
}

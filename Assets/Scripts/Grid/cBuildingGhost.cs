using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBuildingGhost : MonoBehaviour
{
    private Transform visual;
    
    private cMachine placedMachine;
    
    private Vector3 previousMousePosition;
    

    [SerializeField] private Material ghostMaterial;
    
    [SerializeField] private Material invalidGhostMaterial;

    public static cBuildingGhost Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshVisual();
        cGridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
        previousMousePosition = cMouse3D.GetMouseWorldPosition();
    }

    private void Instance_OnSelectedChanged(object sender, EventArgs e)
    {
        RefreshVisual();
    }

    private void LateUpdate()
    {
        if (cInventory.Instance.isBuildMode)
        {
            var selectedItem = cInventory.Instance.GetSelectedItem();
            if (selectedItem == null || selectedItem.count <= 0 || !(selectedItem.item is cMachine))
            {
                if (visual != null)
                {
                    visual.gameObject.SetActive(false);
                }
                return;
            }
            else
            {
                if (visual != null)
                {
                    visual.gameObject.SetActive(true);
                }
            }

            Vector3 currentMousePosition = cGridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
            currentMousePosition.y = 0.1f;
            transform.position = currentMousePosition;
            transform.rotation = cGridBuildingSystem.Instance.GetPlacedObjectRotation();
            bool canBuild = CheckBuildValidity();
            UpdateVisualMaterial(canBuild);
        }
        else
        {
            if (visual != null)
            {
                visual.gameObject.SetActive(false);
            }
        }
    }

    public void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        placedMachine = cGridBuildingSystem.Instance.GetPlacedMachine();

        if (placedMachine != null)
        {
            visual = Instantiate(placedMachine.GetMachineModel.transform, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 6);

            var selectedItem = cInventory.Instance.GetSelectedItem();
            bool canBuild = selectedItem != null && selectedItem.count > 0 && CheckBuildValidity();
            UpdateVisualMaterial(canBuild);
            visual.gameObject.SetActive(canBuild);
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;

        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    private void ApplyMaterialOverlayRecursive(Material overlayMaterial)
    {
        Renderer[] renderers = visual.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (!Array.Exists(renderer.sharedMaterials, mat => mat == overlayMaterial))
            {
                Material[] existingMaterials = renderer.sharedMaterials;
                Array.Resize(ref existingMaterials, existingMaterials.Length + 1);
                existingMaterials[existingMaterials.Length - 1] = overlayMaterial;
                renderer.sharedMaterials = existingMaterials;
            }
        }
    }

    private void UpdateVisualMaterial(bool canBuild)
    {
        if (visual != null)
        {
            Material materialToApply = canBuild ? ghostMaterial : invalidGhostMaterial;
            Material materialToRemove = canBuild ? invalidGhostMaterial : ghostMaterial;

            if (!Array.Exists(visual.GetComponentInChildren<Renderer>().sharedMaterials, mat => mat == materialToApply))
            {
                RemoveMaterialOverlayRecursive(materialToRemove);
                ApplyMaterialOverlayRecursive(materialToApply);
            }
        }
    }

    private void RemoveMaterialOverlayRecursive(Material overlayMaterial)
    {
        Renderer[] renderers = visual.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] sharedMaterials = renderer.sharedMaterials;
            List<Material> updatedMaterials = new List<Material>();

            foreach (Material mat in sharedMaterials)
            {
                if (mat != overlayMaterial)
                {
                    updatedMaterials.Add(mat);
                }
            }
            renderer.sharedMaterials = updatedMaterials.ToArray();
        }
    }

    private bool CheckBuildValidity()
    {
        cGridBuildingSystem.Instance.grid.GetXZ(cMouse3D.GetMouseWorldPosition(), out int x, out int z);
        List<Vector2Int> gridPositionList = placedMachine.GetGridPositionList(new Vector2Int(x, z), cGridBuildingSystem.Instance.dir);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!cGridBuildingSystem.Instance.grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }
}

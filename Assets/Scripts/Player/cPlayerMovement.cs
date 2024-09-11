using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class cPlayerMovement : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    Vector3 mousePosition;

    [SerializeField]
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        mousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouseMove();
        }
        
    }


    void mouseMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Use the mouse position

        if (Physics.Raycast(ray, out hit, 100))
        {
            agent.SetDestination(hit.point);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private InputActionReference pointAction;

    private void Awake()
    {
        pointAction.action.started += _ => Debug.Log("started");
        pointAction.action.performed += Pressed;
        pointAction.action.canceled += _ => Debug.Log("canceled");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Pressed(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private InputActionReference pressInput;
    [SerializeField] private InputActionReference pointInput;
    
    private bool _pressed;
    private Vector2 _pressedPosition;
    private Vector2 _currentPosition;

    private void Awake()
    {
        pressInput.action.performed += _ => Pressed(true);
        pressInput.action.canceled += _ => Pressed(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!_pressed) return;
        
    }

    void Pressed(bool performed)
    {
        _pressed = performed;
        if(performed) _pressedPosition = pointInput.action.ReadValue<Vector2>();
        Debug.LogWarning(_pressedPosition);
    }

}
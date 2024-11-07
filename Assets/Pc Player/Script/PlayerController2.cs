using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{

    [SerializeField] private CursorLockMode _cursorMode = CursorLockMode.Locked;
  
    [SerializeField] private GameObject _mainCamera;

    public Transform LookTarget { get; set; }


    private ControllerMovement3D _controllerMovement;
    private InteractionController _interactionController;
    private Vector3 _moveInput;


    private void Awake()
    {
        _controllerMovement = GetComponent<ControllerMovement3D>();
        Cursor.lockState = _cursorMode;
        Cursor.visible = false;

       _interactionController = _mainCamera.GetComponent<InteractionController>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        _moveInput = new Vector3(input.x, 0f, input.y);
    }

    private void OnJump(InputValue value)
    {
        _controllerMovement.Jump();
    } 
    public void OnInteraction(InputValue value)
    {
        _interactionController.IsPicked = !_interactionController.IsPicked;
    }

    private void Update()
    {
        if (_controllerMovement == null) return;


        Vector3 up = Vector3.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Vector3.Cross(right, up);
        _controllerMovement.SetMoveInput(_moveInput);
        _controllerMovement.SetLookDirection(_moveInput);
    }
}

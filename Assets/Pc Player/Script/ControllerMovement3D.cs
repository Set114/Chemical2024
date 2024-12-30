using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllerMovement3D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private GameObject _mainCamera;
    private float _speed = 0f;
    private bool _hasMoveInput;
    private Vector3 _moveInput;
    private Vector3 _lookDirection;
    
    [Header("Jump")]
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _jumpHeight = 2.5f;
    private Vector3 _velocity;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckOffset = 0f;
    [SerializeField] private float _groundCheckDistance = 0.4f;
    [SerializeField] private float _groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;
    private Vector3 _groundNormal;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void SetMoveInput(Vector3 input)
    {
        _hasMoveInput = input.magnitude > 0.1f;
        _moveInput = _hasMoveInput ? input : Vector3.zero;
    }

    public void SetLookDirection(Vector3 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            return;
        }

        _lookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
    }


    public void Jump()
    {
        if (!_isGrounded) return;
        float jumpVelocity = Mathf.Sqrt(2f * -_gravity * _jumpHeight); 
        _velocity = new Vector3(0, jumpVelocity, 0);
    }

    private void FixedUpdate()
    {
        _isGrounded = CheckGround();
        
        _velocity.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);

        _speed = 0;
        float targetRotation = 0f;

        if (_moveInput != Vector3.zero)
        {
            _speed = _moveSpeed;
        }

        // 確保 _lookDirection 向量不為零
        if (_lookDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(_lookDirection).eulerAngles.y + _mainCamera.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _turnSpeed * Time.fixedDeltaTime);

            if (_hasMoveInput)
            {
                _moveInput = rotation * Vector3.forward;
            }
            else
            {
                _moveInput = Vector3.zero; 
            }
        }
        else
        {
            _moveInput = Vector3.zero; // 如果 _lookDirection 為零，設置 _moveInput 為零
        }

        _characterController.Move(_moveInput * _speed * Time.fixedDeltaTime);

        _isGrounded = CheckGround();
    }

    private bool CheckGround()
    {
        Vector3 start = transform.position + Vector3.up * _groundCheckOffset;

        if (Physics.SphereCast(start, _groundCheckRadius, Vector3.down, out RaycastHit hit, _groundCheckDistance, _groundMask))
        {
            _groundNormal = hit.normal;
            return true;
        }
        _groundNormal = Vector3.up;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_isGrounded) Gizmos.color = Color.green;

        Vector3 start = transform.position + Vector3.up * _groundCheckOffset;
        Vector3 end = start + Vector3.down * _groundCheckDistance;

        Gizmos.DrawWireSphere(start, _groundCheckRadius);
        Gizmos.DrawWireSphere(end, _groundCheckRadius);
    }
}
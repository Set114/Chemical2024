using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Setting")]
    [SerializeField] private float _maxDistance = 2f; //有修改 10f
    [SerializeField] private float _visionRadius = 0.5f; //有修改 1f
    [SerializeField] private LayerMask _layerMask;

    [Header("Throw Setting")]
    [SerializeField] private Transform _target;

    [Header("Scroll Setting")]
    [SerializeField] private float _scrollSpeed = 1f; // 控制滾輪速度

    public bool IsPicked { get; set; } = true;

    private Vector3 _origin;
    private Vector3 _direction;
    private RaycastHit _hit;
    private Transform _item;
    private bool isInteracting = false;

    private void Throw()
    {
        _item.position = _target.position;
        _item.GetComponent<Rigidbody>().useGravity = true;
        _item.gameObject.SetActive(true);
        _item = null;
        isInteracting = false;
    }

    private void Dropoff()
    {
        _item.GetComponent<Rigidbody>().useGravity = true;
        _item = null;
        isInteracting = false;
    }

    private void FixedUpdate()
    {
        float moveSpeed = 10f;
        if (_item != null)
        {
            Vector3 newPosition = Vector3.Lerp(_item.position, _target.position, Time.fixedDeltaTime * moveSpeed);
            _item.GetComponent<Rigidbody>().MovePosition(newPosition);
        }
    }

    private void Update()
    {
        _origin = transform.position;
        _direction = transform.forward;

        if (!IsPicked && _item != null)
        {
            Dropoff();
            isInteracting = false;
        }

        if (!isInteracting && Physics.SphereCast(_origin, _visionRadius, _direction, out _hit, _maxDistance, _layerMask))
        {
            if (_hit.transform.TryGetComponent(out IInteractable item) && IsPicked)
            {
                _item = _hit.transform;
                item.Interact();
                isInteracting = true;
            }
        }

        DetectUIClick();
        HandleScroll();
        HandleRightClick();
    }

    private void DetectUIClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out Button button))
                {
                    button.onClick.Invoke();
                }
            }
        }
    }

    private void HandleScroll()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            Vector3 targetPosition = _target.localPosition;
            targetPosition.z += scrollInput * _scrollSpeed;
            _target.localPosition = targetPosition;
        }
    }

    private void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1) && _item != null)
        {
            _item.Rotate(0, 0, 45); // 這裡將物體繞Z軸旋轉45度，可以根據需要調整角度
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_origin, _origin + _direction * _hit.distance);
        Gizmos.DrawWireSphere(_origin + _direction * _hit.distance, _visionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCCameraMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float rotationSpeed = 5f;
    private Vector3 lastMousePosition;
    public static bool CanRotateUser = true;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("XR Origin");
    }
    void Update()
    {
        // 根據 WASD 鍵的輸入移動攝影機，QE移動上下
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float highInput = 0;
        if (Input.GetKey(KeyCode.E))
        {
            highInput = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            highInput = -1;
        }
        Vector3 movement = new Vector3(horizontalInput, highInput, verticalInput) * moveSpeed * Time.deltaTime;
        player.transform.Translate(movement);

        if (Input.GetMouseButtonDown(2) && CanRotateUser)
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2) && CanRotateUser)
        {
            Vector3 mouseOffset = Input.mousePosition - lastMousePosition;

            float rotationX = -mouseOffset.y * Time.deltaTime * rotationSpeed;
            float rotationY = mouseOffset.x * Time.deltaTime * rotationSpeed;

            player.transform.Rotate(Vector3.up, rotationY, Space.World);
            player.transform.Rotate(Vector3.right, rotationX, Space.Self);

            lastMousePosition = Input.mousePosition;
        }

        if (!CanRotateUser && gameObject.transform.rotation != Quaternion.identity)
        {
            ResetRotate();
        }
    }
    void ResetRotate()
    {
        gameObject.transform.rotation = Quaternion.identity;
    }
}

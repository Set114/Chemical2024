using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRControllerKeyboardInput : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    public ActionBasedController leftHandController;
    public ActionBasedController rightHandController;

    public float moveSpeed = 0.1f;

    void Update()
    {
        // 左手控制
        if (leftHand != null)
        {
            Vector3 leftHandMove = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) leftHandMove += Vector3.up;
            if (Input.GetKey(KeyCode.S)) leftHandMove += Vector3.down;
            if (Input.GetKey(KeyCode.A)) leftHandMove += Vector3.left;
            if (Input.GetKey(KeyCode.D)) leftHandMove += Vector3.right;
            if (Input.GetKey(KeyCode.Z)) leftHandMove += Vector3.forward;
            if (Input.GetKey(KeyCode.X)) leftHandMove += Vector3.back;

            leftHand.position += leftHandMove * moveSpeed;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Left hand grip button pressed");
                leftHandController.SendHapticImpulse(0.5f, 0.1f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Left hand trigger button pressed");
                leftHandController.SendHapticImpulse(0.5f, 0.1f);
            }
        }

        // 右手控制
        if (rightHand != null)
        {
            Vector3 rightHandMove = Vector3.zero;
            if (Input.GetKey(KeyCode.I)) rightHandMove += Vector3.up;
            if (Input.GetKey(KeyCode.K)) rightHandMove += Vector3.down;
            if (Input.GetKey(KeyCode.J)) rightHandMove += Vector3.left;
            if (Input.GetKey(KeyCode.L)) rightHandMove += Vector3.right;
            if (Input.GetKey(KeyCode.N)) rightHandMove += Vector3.forward;
            if (Input.GetKey(KeyCode.M)) rightHandMove += Vector3.back;

            rightHand.position += rightHandMove * moveSpeed;

            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("Right hand grip button pressed");
                rightHandController.SendHapticImpulse(0.5f, 0.1f);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Right hand trigger button pressed");
                rightHandController.SendHapticImpulse(0.5f, 0.1f);
            }
        }
    }
}

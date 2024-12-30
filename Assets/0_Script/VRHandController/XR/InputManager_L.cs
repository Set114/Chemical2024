// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// public class InputManager_L : MonoBehaviour
// {

//     public List<ButtonHandler_L> allButtonHandlers_L = new List<ButtonHandler_L>();
//     public List<ButtonHandler1_L> allButtonHandlers1_L = new List<ButtonHandler1_L>();
//     public List<AxisHandler2D_L> allAxisHandler2D_L = new List<AxisHandler2D_L>();
//     public List<AxisHandler_L> allAxisHandler_L = new List<AxisHandler_L>();

//     private XRController controller = null;
//     private void Awake()
//     {
//         controller = GetComponent<XRController>();
//     }

//     private void Update()
//     {
//         HandleButtonEvents_L();
//         HandleButtonEvents1_L();
//         HandleAxis2DEvents_L();
//         HandleAxisEvents_L();
//     }

//     private void HandleButtonEvents_L()
//     {
//         foreach(ButtonHandler_L handler in allButtonHandlers_L)
//         handler.HandleState(controller);
//     }
//     private void HandleButtonEvents1_L()
//     {
//         foreach(ButtonHandler1_L handler in allButtonHandlers1_L)
//         handler.HandleState(controller);
//     }
//     private void HandleAxis2DEvents_L()
//     {
//         foreach(AxisHandler2D_L handler in allAxisHandler2D_L)
//         handler.HandleState(controller);
//     }

//     public void HandleAxisEvents_L()
//     {
//         foreach(AxisHandler_L handler in allAxisHandler_L)
//         handler.HandleState(controller);
//     }
// }


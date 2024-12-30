using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "NewButtonHandler_L")]
public class ButtonHandler_L : InputHandler
{
    public InputHelpers.Button button = InputHelpers.Button.None;

    public delegate void StateChange(XRController controller);
    public event StateChange OnButtonDown;
    public event StateChange OnButtonUp;

    private bool previousPress = false;
    public bool IsPressed
    {
        get {return previousPress;}
    }
        public override void HandleState(XRController controller)
    {
        if(controller.inputDevice.IsPressed(button, out bool pressed, controller.axisToPressThreshold)){
            if(previousPress != pressed){
                previousPress = pressed;
                if(pressed){
                    OnButtonDown?.Invoke(controller);
                }
                else{
                    OnButtonUp?.Invoke(controller);
                }
            }
        }
    }
}
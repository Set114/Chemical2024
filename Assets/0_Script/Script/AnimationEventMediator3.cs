using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AnimationEventMediator3 : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        SendMessage("OnAnimationEnd");
    }

    public void OnAnimationMiddle1()
    {          
        SendMessage("OnAnimationMiddle1");
    }

    public void OnAnimationMiddle2()
    {
        SendMessage("OnAnimationMiddle2");
    }
    public void OnAnimationMiddle3()
    {
        SendMessage("OnAnimationMiddle3");
    }
}

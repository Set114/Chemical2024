using UnityEngine;
using System.Collections;

public class AnimationEventMediator : MonoBehaviour
{
    public LevelEndSequence levelEndSequence;
    
    public void OnAnimationEnd()
    {
        levelEndSequence.EndLevel(true,true, 2f, 20f, 1f, "1", () => { });
    }
}

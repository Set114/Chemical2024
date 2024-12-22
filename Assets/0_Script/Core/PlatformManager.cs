using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] GameObject pcController;
    [SerializeField] GameObject vrController;
    bool isPC = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        // 在編輯器環境開啟物件
        isPC = true;
#else
        // 在其他平台保持關閉
        isPC = false;
#endif
        pcController.SetActive(isPC);
        vrController.SetActive(!isPC);
    }
}

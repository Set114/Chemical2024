using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] GameObject pcController;
    [SerializeField] GameObject vrController;
    bool isPC = false;

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        // �b�s�边���Ҷ}�Ҫ���
        isPC = true;
#else
        // �b��L���x�O������
        isPC = false;
#endif
        pcController.SetActive(isPC);
        vrController.SetActive(!isPC);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BlendShapeControl : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer; // SkinnedMeshRenderer
    public Transform[] followers; // �n���H������]�אּ�}�C�^
    public Transform[] endPositions; // ������m�]�אּ�}�C�^
    public int blendShapeIndex1; // �Ĥ@�� BlendShape �����ޭ�
    public int blendShapeIndex2; // �ĤG�� BlendShape �����ޭ�
    //public GameObject buttonObject;
    public float startValue = 0f; // ��l��
    public float targetValue = 100f; // �ؼЭ�
    public float[] durations; // �ʵe�ɶ����ס]�אּ�}�C�^
    public Button Close_btn;

    private Coroutine[] blendCoroutines;
    [Header("UI")]
    public GameObject TestUI;
    public GameObject TestShowUI;
    //public GameObject questionMark;
    [Header("questionMark")]
    public GameObject questionMark1;
    public GameObject questionMark2;
    [Header("Button")]
    public Button question1_btn;
    //public Button question1End_btn;
    public Button question2_btn;
    [Header("Question_UI")]
    public GameObject Q1_UI;
    public GameObject Q2_UI;
    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;
    int count=0;
    //public Lvl1TestGM lvl1TestGM;
    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;
    private bool flag = true ;

    [SerializeField] GameObject item;
    void Start()
    {
        if (blendCoroutines != null)
        {
            foreach (Coroutine coroutine in blendCoroutines)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine); // �p�G�����b���檺 Coroutine�A�������
                }
            }
        }
        blendCoroutines = new Coroutine[followers.Length];
        for (int i = 0; i < followers.Length; i++)
        {
            blendCoroutines[i] = StartCoroutine(BlendBlendShapes(startValue, targetValue, durations[i], followers[i], endPositions[i]));
        }
        question1_btn.onClick.AddListener(Question1);
        
        question2_btn.onClick.AddListener(Question2);
        Close_btn.onClick.AddListener(CloseQ);
    }

    IEnumerator BlendBlendShapes(float start, float target, float duration, Transform follower, Transform endPos)
    {
        float startTime = Time.time;
        float distance = Vector3.Distance(follower.position, endPos.position);
        float speed = distance / duration;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            float newValue = Mathf.Lerp(start, target, t);

            meshRenderer.SetBlendShapeWeight(blendShapeIndex1, newValue);
            meshRenderer.SetBlendShapeWeight(blendShapeIndex2, newValue);

            float step = speed * Time.deltaTime;
            follower.position = Vector3.MoveTowards(follower.position, endPos.position, step);

            yield return null;
        }

        // �T�O�̫�ȳ]���ؼЭ�
        meshRenderer.SetBlendShapeWeight(blendShapeIndex1, target);
        meshRenderer.SetBlendShapeWeight(blendShapeIndex2, target);

        follower.position = endPos.position;

        // ���ݤ@��
        yield return new WaitForSeconds(1f); 

        // �}�ҫ��s����
        if(flag != false)
        {
            TestShowUI.gameObject.SetActive(false);      
            questionMark1.SetActive(true);
            question1_btn.gameObject.SetActive(true);          
            TestUI.SetActive(true);  
            flag = false ;
            testDataManager.StartLevel();
            testDataManager.GetsId(3);
        }

    }

    void Question1()
    {
        questionMark1.SetActive(false);
        question1_btn.gameObject.SetActive(false);
        Q1_UI.SetActive(true);
    }   
    public void Question2_mark()
    {
        questionMark2.SetActive(true);
        question2_btn.gameObject.SetActive(true);
    }
    public void Question2()
    {
        questionMark2.SetActive(false);
        question2_btn.gameObject.SetActive(false);
        Q2_UI.SetActive(true);
    }

    public void CloseQ()
    {
        if(count == 0)
        {
            loading_sign.SetActive(true);
            testDataManager.CompleteLevel();
            testDataManager.EndLevelWithCallback(() => {StartCoroutine(WaitAndStartNextLevel1());
                                                        loading_sign.SetActive(false); });
            // testDataManager.EndLevel();

            
        }
        else if(count == 1)
        {
            testDataManager.CompleteLevel();
            StartCoroutine(WaitForEndLevelAndShowUI());
        }
    }

    private IEnumerator WaitForEndLevelAndShowUI()
    {
        yield return null; 
        levelEndSequence.EndLevel(true, false, 1f, 2f, 1f, "1", () => {
            TestShowUI.gameObject.SetActive(true);
        });
        count++;
    }

    private IEnumerator WaitAndStartNextLevel1()
    {
        yield return new WaitForSeconds(1f);

        testDataManager.StartLevel();
        testDataManager.GetsId(4);
        Question2_mark();
        count++;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchBrAceticAcid : MonoBehaviour
{
    public GameObject[] CloseObj;
    public GameObject[] OpenObj;
    public Animator Ani;
    public GameObject QA;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "potassium")
        {
            Ani.SetTrigger("Run");
            for (int i = 0; i < CloseObj.Length; i++)
            {
                CloseObj[i].SetActive(false);
            }
            for (int i = 0; i < OpenObj.Length; i++)
            {
                OpenObj[i].SetActive(true);
            }
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        QA.SetActive(true);
    }
}

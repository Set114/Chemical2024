using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMarbles : MonoBehaviour
{
    public Stage4_5 Stage4_5Obj;
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "BlockMarble") {
            Stage4_5Obj.SetTableMarblesCollider(false);
            Stage4_5Obj.TableMarbles[0].SetActive(false);
            Stage4_5Obj.CupMarbles[0].SetActive(true);
            Stage4_5Obj.MarbleUIs[0].SetActive(true);
            Stage4_5Obj.MarbleInfos[0].SetActive(true);
            Stage4_5Obj.MarbleInfos[3].SetActive(false);
            Stage4_5Obj.MarbleInfos[1].SetActive(false);
            Stage4_5Obj.MarbleInfos[2].SetActive(false);
            StartCoroutine(Wait());

        }
        if (hit.GetComponent<Collider>().name == "GranularMarble")
        {
            Stage4_5Obj.SetTableMarblesCollider(false);
            Stage4_5Obj.TableMarbles[1].SetActive(false);
            Stage4_5Obj.CupMarbles[1].SetActive(true);
            Stage4_5Obj.MarbleUIs[1].SetActive(true);
            Stage4_5Obj.MarbleInfos[1].SetActive(true);
            Stage4_5Obj.MarbleInfos[3].SetActive(false);
            Stage4_5Obj.MarbleInfos[0].SetActive(false);
            Stage4_5Obj.MarbleInfos[2].SetActive(false);
            StartCoroutine(Wait());
        }
        if (hit.GetComponent<Collider>().name == "PowderyMarble")
        {
            Stage4_5Obj.SetTableMarblesCollider(false);
            Stage4_5Obj.TableMarbles[2].SetActive(false);
            Stage4_5Obj.CupMarbles[2].SetActive(true);
            Stage4_5Obj.MarbleUIs[2].SetActive(true);
            Stage4_5Obj.MarbleInfos[2].SetActive(true);
            Stage4_5Obj.MarbleInfos[3].SetActive(false);
            Stage4_5Obj.MarbleInfos[1].SetActive(false);
            Stage4_5Obj.MarbleInfos[0].SetActive(false);
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(5f);
        Stage4_5Obj.SetTableMarblesCollider(true);
        for (int i = 0; i < Stage4_5Obj.CupMarbles.Length; i++) {
            Stage4_5Obj.CupMarbles[i].SetActive(false);
            Stage4_5Obj.MarbleUIs[i].SetActive(false);
            //Stage4_5Obj.MarbleInfos[i].SetActive(false);
        }
        if (!Stage4_5Obj.TableMarbles[0].active&& !Stage4_5Obj.TableMarbles[1].active && !Stage4_5Obj.TableMarbles[2].active) {
            Stage4_5Obj.Finished.SetActive(true);
            Stage4_5Obj.ReButtonUI.SetActive(true);

        }
    }
}

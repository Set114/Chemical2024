using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController_Unit3 : MonoBehaviour
{
    public GameObject[] part0Objects; // part0 ���d������
    public GameObject[] part1Objects;
    public GameObject[] part2Objects; 
    public GameObject[] part3Objects; 
    public GameObject[] part4Objects; 
    public GameObject[] part5Objects;

    public CanvasController_Unit3 canvasController_Unit3;
    public Whiteboard whiteboardn;
    public Whiteboard whiteboardo;
    public Whiteboard whiteboardc;
    public Whiteboard whiteboardh;
    public Whiteboard whiteboardfe;

    public void ActivateObjectsForLevel(string partName)
    {
        CloneRemover();
        DeactivateAllObjects(); // �������Ҧ�����

        switch (partName)
        {
            case "part0":
                ActivateObjects(part0Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.ShowTutorial();
                                                          DeactivateAllObjects();     }));
                break;
            case "part1":
                ActivateObjects(part1Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.part1();
                                                          DeactivateAllObjects();     }));
                break;
            case "part2":
                ActivateObjects(part2Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.part2();
                                                          DeactivateAllObjects();     }));
                break;
            case "part3":
                ActivateObjects(part3Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.part3();
                                                          DeactivateAllObjects();     }));
                break;
            case "part4":
                ActivateObjects(part4Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.part4();
                                                          DeactivateAllObjects();     }));
                break;
            case "part5":
                ActivateObjects(part5Objects);
                StartCoroutine(DelayedAction(3.0f, () => {canvasController_Unit3.part5();
                                                          DeactivateAllObjects();     }));
                break;
            default:
                Debug.LogWarning("Unknown level: " + partName);
                break;
        }
    }

    private void ActivateObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
        Debug.Log($"Activated objects for {objects.Length} objects.");
    }

    private void DeactivateAllObjects()
    {
        part0Objects = DeactivateArray(part0Objects);
        part1Objects = DeactivateArray(part1Objects);
        part2Objects = DeactivateArray(part2Objects);
        part3Objects = DeactivateArray(part3Objects);
        part4Objects = DeactivateArray(part4Objects);
        part5Objects = DeactivateArray(part5Objects);
    }

    private GameObject[] DeactivateArray(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        return objects;
    }

    public void CloneRemover()
    {
        // ���������Ҧ��� GameObjects
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // �ˬd����W�٬O�_�]�t "Clone"
            if (obj.name.Contains("Clone"))
            {
                // �R������
                Destroy(obj);
            }
        }
    }

    // �q�Ϊ�����欰��{�A������w�����ƫ����ʧ@
    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
        resetValue();
    }

    public void resetValue()
    {
        whiteboardn.ResetValue();
        whiteboardo.ResetValue();
        whiteboardc.ResetValue();
        whiteboardh.ResetValue();
        whiteboardfe.ResetValue();
    }

}

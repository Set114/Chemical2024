using UnityEngine;
using UnityEngine.UI;

public class OpenObject : MonoBehaviour
{
    public Button button;
    public GameObject[] objectsToOpen;
    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }
    public void OnButtonClicked()
    {
        foreach (GameObject obj in objectsToOpen)
        {
            obj.SetActive(true); // 打開物件
        }
    }
}

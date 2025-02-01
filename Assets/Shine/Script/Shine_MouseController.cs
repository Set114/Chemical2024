using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine_MouseController : MonoBehaviour
{

    public GameObject selectedObject;          //被選中的物件
    Transform selectedObjectParent;     //紀錄被選中的物件原本的Parent
    Vector3 initialPosition;            //物体的初始位置
    Quaternion initialRotation;         //物体的初始世界旋转
    GameObject parentEmpty;             //用來調整相對位置用的
    float planeDistance = 1f;        //平面距離

    public string Tag;
        // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 檢測滑鼠點擊
        if (Input.GetMouseButtonDown(0) && !selectedObject) // 左鍵按下
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 檢測到物件，開始拖曳
                if (hit.collider.gameObject.CompareTag(Tag)) // 確保物件有指定的標籤
                {
                    selectedObject = hit.collider.gameObject;
                    initialPosition = selectedObject.transform.position;
                    initialRotation = selectedObject.transform.rotation;
                }

            }
        }
        if (selectedObject != null && Input.GetMouseButton(0)) // 左鍵按住
        {
            Vector3 mouseWorldPosition = GetMousePositionOnFixedPlane();
            // 將物件移動到滑鼠位置
            transform.position = mouseWorldPosition;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        // 放開滑鼠，結束拖曳
        if (Input.GetMouseButtonUp(0)&&selectedObject != null)
        {
            this.Reset();
        }
    }
    //取得滑鼠前方固定距離的位置
    Vector3 GetMousePositionOnFixedPlane()
    {
        // 獲取滑鼠的螢幕座標
        Vector3 mouseScreenPosition = Input.mousePosition;
        // 設置滑鼠的 Z 軸距離（相對於攝影機）
        mouseScreenPosition.z = planeDistance;
        // 使用攝影機將螢幕座標轉換為世界座標
        Vector3 worldPosition = new Vector3(Camera.main.ScreenToWorldPoint(mouseScreenPosition).x, Camera.main.ScreenToWorldPoint(mouseScreenPosition).y, transform.position.z);

        return worldPosition;
    }
    public void Reset()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position = initialPosition;       //恢復初始位置
            selectedObject.transform.rotation = initialRotation;       //恢復初始角度
        }
        selectedObject = null;
        GetComponent<Rigidbody>().isKinematic = false;

    }
}

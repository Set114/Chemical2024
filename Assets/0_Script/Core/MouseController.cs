using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    GameManager gm;

    Camera mainCamera;                  //主攝影機
    GameObject selectedObject;          //被選中的物件
    Transform selectedObjectParent;     //紀錄被選中的物件原本的Parent
    Vector3 initialPosition;            //物体的初始位置
    Quaternion initialRotation;         //物体的初始世界旋转
    GameObject parentEmpty;             //用來調整相對位置用的
    float planeDistance = 0.65f;        //平面距離

    bool isToolSwitchOn;                //道具是否使用中

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

        mainCamera = Camera.main; // 獲取主攝影機
        parentEmpty = new GameObject("PCControllerEmptyObject");
    }

    // Update is called once per frame
    void Update()
    {
        // 檢測滑鼠點擊
        if (Input.GetMouseButtonDown(0)) // 左鍵按下
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 檢測到物件，開始拖曳
                if (hit.collider.gameObject.CompareTag("Pickable")) // 確保物件有指定的標籤
                {
                    selectedObject = hit.collider.gameObject;
                    //紀錄初始位置、角度
                    initialPosition = selectedObject.transform.position;
                    initialRotation = selectedObject.transform.rotation;
                    //設置empty
                    parentEmpty.transform.position = initialPosition;
                    //parentEmpty.transform.rotation = initialRotation;
                    selectedObjectParent = selectedObject.transform.parent;
                    selectedObject.transform.parent = parentEmpty.transform;
                    switch (selectedObject.name)
                    {
                        case "hammer":
                            Collider[] objCollider = selectedObject.GetComponents<Collider>();
                            objCollider[1].enabled = false;
                            selectedObject.transform.localEulerAngles = new Vector3(90.0f, -90.0f, 0.0f);
                            break;
                        case "GAS":
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.067f);
                            selectedObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 55.0f);
                            isToolSwitchOn = false; //初始化噴燈起始狀態
                            break;
                        case "Iron":                            
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, -0.054f, 0.075f);
                            selectedObject.transform.localEulerAngles = new Vector3(-15.0f, 270.0f, 75.0f);
                            break;
                        case "TestTube":

                            break;
                        case "AlcoholLamp":
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Paper":
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0241f);
                            break;
                        case "DryIce":
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0442f);
                            break;
                        case "Glass":
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.053f); 
                            selectedObject.transform.localEulerAngles = new Vector3( 0.0f, 0.0f, 0.0f);
                            break;
                    }
                }
            }
        }

        // 放開滑鼠，結束拖曳
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            switch (selectedObject.name)
            {
                case "hammer":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    Collider[] objCollider = selectedObject.GetComponents<Collider>();
                    objCollider[0].enabled = true;
                    break;
                case "GAS":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    isToolSwitchOn = false;
                    selectedObject.SendMessage("Fire", isToolSwitchOn);
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "Iron":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "TestTube":
                    selectedObject.transform.parent = selectedObjectParent;
                    break;
                case "AlcoholLamp":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "Paper":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "DryIce":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
            }
            selectedObject = null;
        }

        // 拖曳操作
        if (selectedObject != null && Input.GetMouseButton(0)) // 左鍵按住
        {
            Vector3 mouseWorldPosition = GetMousePositionOnFixedPlane();
            // 將物件移動到滑鼠位置
            parentEmpty.transform.position = mouseWorldPosition;
        }        

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedObject)
            {
                switch (selectedObject.name)
                {
                    case "hammer":  //attach hammer動畫

                        break;
                    case "GAS":
                        isToolSwitchOn = !isToolSwitchOn;
                        selectedObject.SendMessage("Fire", isToolSwitchOn);
                        break;
                }
            }
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
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        return worldPosition;
    }
}

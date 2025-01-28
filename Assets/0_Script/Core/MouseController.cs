using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    GameManager gm;

    Camera mainCamera;                  //主攝影機
    public GameObject selectedObject;          //被選中的物件
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
        if (Input.GetMouseButtonDown(0) && !selectedObject ) // 左鍵按下
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
                        //------------Stage 1--------------
                        case "hammer":
                            planeDistance = 0.722f;
                            Collider[] objColliderhammer = selectedObject.GetComponents<Collider>();
                            objColliderhammer[1].enabled = false;
                            selectedObject.transform.localPosition = new Vector3(0.0013f, 0.0f, 0.0f);
                            selectedObject.transform.localEulerAngles = new Vector3(75.0f, -90.0f, 0.0f);
                            break;
                        case "GAS":
                            planeDistance = 0.717f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.091f, -0.064f, 0.0f);
                            selectedObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 55.0f);
                            isToolSwitchOn = false; //初始化噴燈起始狀態
                            break;
                        case "Iron":
                            planeDistance = 0.725f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, -0.054f, 0.0f);
                            selectedObject.transform.localEulerAngles = new Vector3(-15.0f, 270.0f, 75.0f);
                            break;
                        case "TestTube":
                            planeDistance = 0.68f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "AlcoholLamp":
                            planeDistance = 0.68f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Paper":
                            planeDistance = 0.65f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0241f);
                            break;
                        case "DryIce":
                            planeDistance = 0.65f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0442f);
                            break;
                        case "Glass":
                            planeDistance = 0.65f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.053f); 
                            selectedObject.transform.localEulerAngles = new Vector3( 0.0f, 0.0f, 0.0f);
                            break;
                        //------------Stage 2--------------
                        case "GAS_2_1":
                            planeDistance = 0.77f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.091f, -0.064f, 0.0f);
                            selectedObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 55.0f);
                            isToolSwitchOn = false; //初始化噴燈起始狀態
                            break;
                        case "CalciumChloride_2-2":
                        case "SodiumCarbonate_2-2":
                        case "SodiumCarbonate_2-3":
                        case "HCI_2-3":
                            planeDistance = 0.762f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Tweezers_2-2":
                        case "Tweezers_2-3":
                        case "Tweezers_2-6":
                            planeDistance = 0.671f;
                            Collider[] objColliderTweezers = selectedObject.GetComponents<Collider>();
                            objColliderTweezers[2].enabled = false;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Cap_2-2":     //瓶蓋
                        case "Balloon_2-3": //氣球
                        case "RubberBand_2-3":  //橡皮筋
                            planeDistance = 0.762f;
                            Collider objColliderCap2_2 = selectedObject.GetComponent<Collider>();
                            objColliderCap2_2.isTrigger = true;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "BakingSoda_2-4":
                        case "BakingSoda_2-5":
                        case "Rag_2-4": //抹布
                        case "Rag_Wet_2-4": //抹布
                            planeDistance = 0.681f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Toner_2-6":   //碟子
                        case "CopperOxide_2-6":   //碟子
                            planeDistance = 0.753f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "WaterBottle_2-2":
                        case "WaterBottle_2-3":
                            planeDistance = 0.762f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, -0.104f, 0.0f);
                            break;
                        case "Glass_2-4":
                            planeDistance = 0.681f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            selectedObject.transform.localPosition = new Vector3(0.0f, -0.053f, 0.0f); 
                            selectedObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        //------------Stage 5--------------
                        case "Cap_5-2":
                            planeDistance = 0.753f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Glucose_5-3":
                        case "Water100ml_5-5":
                            planeDistance = 0.681f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                        case "Card_01":
                        case "Card_02":
                        case "Card_03":
                        case "Card_04":
                        case "Card_05":
                        case "Card_06":
                        case "Card_07":
                        case "Card_08":
                        case "Card_09":
                        case "Card_10":
                        case "Card_11":
                        case "Card_12":
                            planeDistance = 0.714f;//0.714f;
                            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                            break;
                    }
                    GameObject.FindWithTag("LevelObject").SendMessage("Grab", selectedObject, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        // 放開滑鼠，結束拖曳
        if (Input.GetMouseButtonUp(0) )
        {
            Debug.Log("Reset");
            this.Reset();
        }

        // 拖曳操作
        if (selectedObject != null && Input.GetMouseButton(0)) // 左鍵按住
        {
            Vector3 mouseWorldPosition = GetMousePositionOnFixedPlane();
            // 將物件移動到滑鼠位置
            parentEmpty.transform.position = mouseWorldPosition;
        }        

        //使用物件
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedObject)
            {
                switch (selectedObject.name)
                {
                    //------------Stage 1--------------
                    case "hammer":  //attach hammer動畫
                        if (!selectedObject.GetComponent<MotionHammer>())
                            selectedObject.AddComponent<MotionHammer>();
                        break;
                    case "GAS":
                        isToolSwitchOn = !isToolSwitchOn;
                        selectedObject.SendMessage("Fire", isToolSwitchOn);
                        break;
                    //------------Stage 2--------------
                    case "GAS_2_1":
                        isToolSwitchOn = !isToolSwitchOn;
                        selectedObject.SendMessage("Fire", isToolSwitchOn);
                        break;
                }
            }
        }
        //使用物件
        if (Input.GetMouseButton(1))
        {
            if (selectedObject)
            {
                Quaternion targetRotation;
                switch (selectedObject.name)
                {
                    case "CalciumChloride_2-2":
                    case "SodiumCarbonate_2-2":
                    case "SodiumCarbonate_2-3":
                    case "HCI_2-3":
                        targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 56.0f));
                        selectedObject.transform.localRotation = Quaternion.Slerp(  selectedObject.transform.localRotation, targetRotation, Time.deltaTime * 1.0f / 0.07f);
                        break;
                    case "BakingSoda_2-4":  //小碟子
                    case "BakingSoda_2-5":
                    case "Toner_2-6":   //碟子
                    case "CopperOxide_2-6":   //碟子
                    case "Glucose_5-3":   //碟子
                    case "Water100ml_5-5":
                        targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 85.0f));
                        selectedObject.transform.localRotation = Quaternion.Slerp(selectedObject.transform.localRotation, targetRotation, Time.deltaTime * 1.0f / 0.07f);
                        break;
                    case "WaterBottle_2-2":
                    case "WaterBottle_2-3":                    
                        targetRotation = Quaternion.Euler(new Vector3( 0.0f, 0.0f, 56.0f));
                        selectedObject.transform.localRotation = Quaternion.Slerp(selectedObject.transform.localRotation, targetRotation, Time.deltaTime * 1.0f / 0.07f);
                        break;
                }
            }
        }
        //使用物件
        else//if(Input.GetMouseButtonUp(1))
        {
            if (selectedObject)
            {
                Quaternion targetRotation;
                switch (selectedObject.name)
                {
                    case "CalciumChloride_2-2":
                    case "SodiumCarbonate_2-2":
                    case "SodiumCarbonate_2-3":
                    case "HCI_2-3":
                    case "BakingSoda_2-4":  //小碟子
                    case "BakingSoda_2-5":
                    case "Toner_2-6":   //碟子
                    case "CopperOxide_2-6":   //碟子
                    case "Glucose_5-3":
                    case "Water100ml_5-5":
                        targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                        selectedObject.transform.localRotation = Quaternion.Slerp(selectedObject.transform.localRotation, targetRotation, Time.deltaTime * 1.0f / 0.07f);
                        break;
                    case "WaterBottle_2-2":
                    case "WaterBottle_2-3": 
                        selectedObject.transform.localPosition = new Vector3(0.0f, -0.104f, 0.0f);
                        targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                        selectedObject.transform.localRotation = Quaternion.Slerp(selectedObject.transform.localRotation, targetRotation, Time.deltaTime * 1.0f / 0.07f);
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

    private void Reset()
    {
        if (selectedObject)
        {
            switch (selectedObject.name)
            {
                //------------Stage 1--------------
                case "hammer":
                    if (selectedObject.GetComponent<MotionHammer>())
                        Destroy(selectedObject.GetComponent<MotionHammer>());
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    Collider[] objColliderHammer = selectedObject.GetComponents<Collider>();
                    objColliderHammer[1].enabled = true;
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
                case "TestTube":
                case "AlcoholLamp":
                case "Paper":
                case "DryIce":
                case "Glass":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                //------------Stage 2--------------
                case "GAS_2_1":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    isToolSwitchOn = false;
                    selectedObject.SendMessage("Fire", isToolSwitchOn);
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "Tweezers_2-2":
                case "Tweezers_2-3":
                case "Tweezers_2-6":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    isToolSwitchOn = false;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    Collider[] objColliderTweezers = selectedObject.GetComponents<Collider>();
                    objColliderTweezers[2].enabled = true;
                    break;
                case "CalciumChloride_2-2":
                case "SodiumCarbonate_2-2":
                case "SodiumCarbonate_2-3":
                case "HCI_2-3":
                case "BakingSoda_2-4":
                case "BakingSoda_2-5":
                case "Rag_2-4": //抹布
                case "Rag_Wet_2-4": //濕抹布
                case "Toner_2-6":   //碟子
                case "CopperOxide_2-6":   //碟子
                case "Glass_2-4":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    isToolSwitchOn = false;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "Cap_2-2":
                case "Balloon_2-3": //氣球
                case "RubberBand_2-3":  //橡皮筋
                    Collider objColliderCap2_2 = selectedObject.GetComponent<Collider>();
                    objColliderCap2_2.isTrigger = false;
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case "WaterBottle_2-2": //恢復物理，但不回原位
                case "WaterBottle_2-3":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    selectedObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                //------------Stage 5--------------
                case "Cap_5-2":                
                    break;
                case "Glucose_5-3":
                case "Water100ml_5-5":
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    selectedObject.transform.position = initialPosition;       //恢復初始位置
                    selectedObject.transform.rotation = initialRotation;       //恢復初始角度
                    isToolSwitchOn = false;                    
                    break;
                case "Card_01":
                case "Card_02":
                case "Card_03":
                case "Card_04":
                case "Card_05":
                case "Card_06":
                case "Card_07":
                case "Card_08":
                case "Card_09":
                case "Card_10":
                case "Card_11":
                case "Card_12":
                    Debug.Log("Reset卡牌");
                    selectedObject.transform.parent = selectedObjectParent;
                    selectedObject.GetComponent<Rigidbody>().isKinematic = false;
                    break;
            }
            selectedObject = null;
        }
    }
}

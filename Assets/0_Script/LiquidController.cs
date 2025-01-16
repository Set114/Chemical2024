using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiquidController : MonoBehaviour
{
    [Header("液體設定")]
    [Tooltip("瓶口中心位置")]
    [SerializeField] private Transform port;
    [Tooltip("液體MeshRenderer")]
    [SerializeField] private MeshRenderer myLiquid;
    [Tooltip("液體顏色")]
    public Color liquidColor;

    [Tooltip("最大容量")]
    [SerializeField] private float maxCapacity = 100f;
    [Tooltip("目前容量")]
    [SerializeField] private float currCapacity = 0f;
    [Tooltip("目前容量比例")]
    private float ratio;

    [Header("倒出相關設定")]
    [Tooltip("流體效果")]
    [SerializeField] private ParticleSystem stream;
    [Tooltip("判斷倒出液體的最小角度")]
    [SerializeField] private float minPourOutAngle = 0f;
    [Tooltip("判斷倒出液體的最大角度")]
    [SerializeField] private float maxPourOutAngle = 0f;
    [Tooltip("判斷倒出液體的角度")]
    [SerializeField] private float pourOutAngle = 0f;
    [Tooltip("最大倒出速度")]
    [SerializeField] private float maxPourOutSpeed = 10000f;
    private float pourOutSpeed;                                 // 目前倒出速度

    [Tooltip("篩選注入液體")]
    public string injectFilter = "";
    [Tooltip("無限液體")]
    [SerializeField] private bool isUnlimited = false;
    private bool isFull = false;
    private bool isReach = false;
    [Space]

    [Tooltip("達成指定比例")]
    public float targerRatio = 0f;

    [Tooltip("注入時觸發")]
    [SerializeField] private UnityEvent _whenInject;
    [Tooltip("倒出時觸發")]
    [SerializeField] private UnityEvent _whenPourOut;
    [Tooltip("注滿時觸發")]
    [SerializeField] private UnityEvent _whenFull;
    [Tooltip("達成指定比例時觸發")]
    [SerializeField] private UnityEvent _whenReachTargerRatio;

    public UnityEvent WhenInject => _whenInject;
    public UnityEvent WhenPourOut => _whenPourOut;
    public UnityEvent WhenFull => _whenFull;
    public UnityEvent WhenReachTargerRatio => _whenReachTargerRatio;

    // Start is called before the first frame update
    void Start()
    {
        ratio = currCapacity / maxCapacity;
        isFull = ratio >= 0.99f;
    }

    // Update is called once per frame
    void Update()
    {
        if (port != null)
        {
            pourOutAngle = Mathf.Lerp(minPourOutAngle, maxPourOutAngle, ratio);
            if (port.up.y < pourOutAngle)
            {
                pourOutSpeed = Mathf.Clamp(maxPourOutSpeed * ratio * Mathf.Abs(port.up.y), maxPourOutSpeed / 2, maxPourOutSpeed);
                if (currCapacity > 0)
                {
                    // 将目标方向从世界坐标系转换到本地坐标系
                    Vector3 localTargetDirection = transform.InverseTransformDirection(-Vector3.up);
                    // 计算旋转以使指针的本地z轴指向目标方向
                    float targetRotation = Mathf.Atan2(localTargetDirection.x, localTargetDirection.z) * Mathf.Rad2Deg;
                    // 将目标旋转应用到指针的本地坐标系中
                    port.localRotation = Quaternion.Euler(0f, targetRotation, 0);

                    if (!isUnlimited)
                    {
                        currCapacity -= pourOutSpeed * Time.deltaTime;
                        if (currCapacity < 0)
                        {
                            currCapacity = 0;
                        }
                        if (ratio < 0.99f && isFull)
                        {
                            isFull = false;
                        }
                    }

                    // 創建射線
                    Ray ray = new Ray(port.position, Vector3.down);
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the water layer
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        LiquidController container = hit.transform.GetComponentInParent<LiquidController>();

                        if (container && container != this)
                        {
                            container.InjectLiquid(gameObject.name, pourOutSpeed);

                            Debug.DrawRay(port.position, Vector3.down * hit.distance, Color.green);
                        }
                        else
                        {
                            Debug.DrawRay(port.position, Vector3.down * hit.distance, Color.yellow);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(port.position, Vector3.down * 1000, Color.white);
                    }
                    if (stream)
                    {
                        if (stream.isStopped)
                            stream.Play();
                    }
                    _whenPourOut.Invoke();
                }
                else
                {
                    if (currCapacity < 0)
                        currCapacity = 0;
                    if (stream)
                    {
                        if (stream.isPlaying)
                            stream.Stop();
                    }
                }
            }
            else
            {
                if (stream)
                {
                    if (stream.isPlaying)
                        stream.Stop();
                }
            }
        }
        ratio = currCapacity / maxCapacity;
        if (myLiquid)
        {
            myLiquid.material.SetFloat("_Fill", ratio);
            myLiquid.material.SetColor("_Color", liquidColor);
        }
        if (stream)
        {
            ParticleSystem.MainModule main = stream.main;
            main.startColor = liquidColor;
        }
    }


    //注入液體
    public void InjectLiquid(string pourInto, float speed)
    {
        if (targerRatio > 0f && !isReach)
        {
            if (ratio >= targerRatio)
            {
                _whenReachTargerRatio.Invoke();
                isReach = true;
            }
        }


        if (isFull)
            return;
        //判斷是否指定注入液體
        if (injectFilter != "" && pourInto != "")
        {
            if (pourInto != injectFilter)
            {
                print("wrong inject");
                return;
            }
        }

        //如果滿了
        if (ratio >= 0.99f)
        {
            _whenFull.Invoke();
            isFull = true;
            return;
        }
        currCapacity += speed * Time.deltaTime;
        _whenInject.Invoke();
    }
}

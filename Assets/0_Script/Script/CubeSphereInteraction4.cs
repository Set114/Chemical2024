using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CubeSphereInteraction4 : MonoBehaviour
{
    public TextMeshProUGUI text1; // 第一个TextMeshPro组件
    public TextMeshProUGUI text2; // 第二个TextMeshPro组件
    public SphereDetection SphereDetection;

    private bool a1 = false;
    private GameObject currentOther;
    private GameObject currentNearestCube;
    float distanceThreshold = 0.3f;

    void Update()
    {
        if (a1 && currentOther.activeSelf == false)
        {
            a1 = false;
            Zz(currentOther);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // 检测到N2(sphere)或O2(sphere)进入Collider
        if (other.CompareTag("gas"))
        {
           

            currentOther = other.gameObject;
            currentNearestCube = FindNearestCube(currentOther); // 找到最近的Cube对象

            if (currentNearestCube != null && SphereDetection.cubeIndexMap.ContainsKey(currentNearestCube))
            {
                int cubeIndex = SphereDetection.cubeIndexMap[currentNearestCube];

                // 复制Cube的坐标给球
                currentOther.transform.position = currentNearestCube.transform.position;

                // 锁定球的Constraints
                Rigidbody sphereRB = currentOther.GetComponent<Rigidbody>();
                if (sphereRB != null)
                {
                    sphereRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                }

                // 禁用Cube的Collider
                currentNearestCube.GetComponent<Collider>().enabled = false;
                SphereDetection.cubeCollidersEnabled[cubeIndex] = false;
                a1 = true;

                SphereDetection.sphereCountPerCube[cubeIndex]++;
                // 确认Collider已被禁用
               

                // 更新数组中的值或执行其他逻辑
                UpdateTexts();
           
               
            }
        }
    }

    public void Zz(GameObject other)
    {
        // 检测到N2(sphere)或O2(sphere)离开Collider
        if (other.CompareTag("gas"))
        {
            


            if (currentNearestCube != null && SphereDetection.cubeIndexMap.ContainsKey(currentNearestCube))
            {
                Debug.Log(3);
                int cubeIndex = SphereDetection.cubeIndexMap[currentNearestCube];

                // 启用Cube的Collider
                currentNearestCube.GetComponent<Collider>().enabled = true;
                SphereDetection.cubeCollidersEnabled[cubeIndex] = true;

                // 取消球的Constraints
                Rigidbody sphereRB = other.GetComponent<Rigidbody>();
                if (sphereRB != null)
                {
                    sphereRB.constraints = RigidbodyConstraints.None;
                }
                SphereDetection.sphereCountPerCube[cubeIndex]--;
                // 确认Collider已被启用
               

                // 更新数组中的值或执行其他逻辑
                UpdateTexts();
            }
        }
    }

    private void UpdateTexts()
    {
        // 更新TextMeshPro显示的数量或状态信息
        int count1 = CountH2Spheres(); // 计算N2(sphere)的数量
        int count2 = CountO2Spheres(); // 计算O2(sphere)的数量

        text1.text = count1.ToString();
        text2.text = count2.ToString();
    }

    private int CountO2Spheres()
    {
        // 计算场景中O2(sphere)的数量，限定只计算与Cube相关的球体
        int count = 0;

        foreach (GameObject cube in SphereDetection.cubeIndexMap.Keys)
        {
            GameObject[] spheres = FindObjectsOfType<GameObject>();
            foreach (GameObject sphere in spheres)
            {
                if (sphere.name.StartsWith("O2") && Vector3.Distance(cube.transform.position, sphere.transform.position) < 0.1f)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private int CountH2Spheres()
    {
        // 计算场景中O2(sphere)的数量，限定只计算与Cube相关的球体
        int count = 0;

        foreach (GameObject cube in SphereDetection.cubeIndexMap.Keys)
        {
            GameObject[] spheres = FindObjectsOfType<GameObject>();
            foreach (GameObject sphere in spheres)
            {
                if (sphere.name.StartsWith("H2") && Vector3.Distance(cube.transform.position, sphere.transform.position) < 0.1f)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private GameObject FindNearestCube(GameObject sphere)
    {
        // 查找最近的Cube对象，可以根据实际需求来实现查找逻辑
        GameObject nearestCube = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject cube in SphereDetection.cubeIndexMap.Keys)
        {
            float distance = Vector3.Distance(cube.transform.position, sphere.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCube = cube;
            }
        }

        return nearestCube;
    }
}

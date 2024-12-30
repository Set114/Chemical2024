using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetection : MonoBehaviour
{
    public Dictionary<GameObject, int> cubeIndexMap; // Cube对象与索引的映射
    public List<bool> cubeCollidersEnabled; // 记录Cube的Collider状态的数组
    public List<int> sphereCountPerCube;

     void Start()
    {
        // 初始化 cubeIndexMap 和 cubeCollidersEnabled
        cubeIndexMap = new Dictionary<GameObject, int>();
        cubeCollidersEnabled = new List<bool>();
        sphereCountPerCube = new List<int>();

        // 查找场景中所有以 "Cube" 开头的对象
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");

        // 遍历每一个 Cube 对象并添加到字典和列表中
        for (int i = 0; i < cubes.Length; i++)
        {
            GameObject cube = cubes[i];
            cubeIndexMap.Add(cube, i);
            cubeCollidersEnabled.Add(true); // 初始化时将所有 Cube 的 Collider 设置为启用状态
            sphereCountPerCube.Add(0);
        }
    }
}

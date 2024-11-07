using UnityEngine;
using System.IO;
using System.Text;
using TMPro;
using System.Collections.Generic;

public class ReadDatas : MonoBehaviour
{
    public static ReadDatas Instance { get; private set; }

    [SerializeField] TMP_Text displayText;

    [SerializeField] TMP_Dropdown city_drop;
    [SerializeField] TMP_Dropdown region_drop;
    [SerializeField] TMP_Dropdown school_drop;
    [SerializeField] TMP_Dropdown year_drop;
    [SerializeField] TMP_Dropdown class_drop;
    [SerializeField] TMP_Dropdown studentID_drop;
    
    string cityCsvFilePath = "Assets/Source/AllCitys.csv";
    string regionCsvFilePath = "Assets/Source/AllRegions.csv";
    
    private Dictionary<string, List<string>> regionDataByCity;
    
    public string cityData;
    public string regionData;
    public string schoolData;
    public string yearData;   
    public string classData;  
    public string studentData; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ReadCityData();
        ReadRegionData();
        ReadYearData();
        
        city_drop.onValueChanged.AddListener(OnCityChanged);
        region_drop.onValueChanged.AddListener(OnRegionChanged);
        school_drop.onValueChanged.AddListener(OnSchoolChanged);
        year_drop.onValueChanged.AddListener(OnYearChanged);
        class_drop.onValueChanged.AddListener(OnClassChanged);
        studentID_drop.onValueChanged.AddListener(OnStudentIDChanged);

        LoadData();
    }

    void ReadCityData()
    {
        if (!File.Exists(cityCsvFilePath))
        {
            Debug.LogError("找不到 CSV 文件: " + cityCsvFilePath);
            return;
        }

        using (StreamReader reader = new StreamReader(cityCsvFilePath, Encoding.UTF8))
        {
            city_drop.options.Clear();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] cells = line.Split(',');

                foreach (string cell in cells)
                {
                    if (string.IsNullOrEmpty(cell) || cell == "null")
                    {
                        continue;
                    }

                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(cell);
                    city_drop.options.Add(optionData);
                }
            }
        }
    }

    void ReadRegionData()
    {
        regionDataByCity = new Dictionary<string, List<string>>();

        if (!File.Exists(regionCsvFilePath))
        {
            Debug.LogError("找不到 CSV 文件: " + regionCsvFilePath);
            return;
        }

        using (StreamReader reader = new StreamReader(regionCsvFilePath, Encoding.UTF8))
        {
            if (reader.EndOfStream)
            {
                Debug.LogError("CSV 文件没有数据: " + regionCsvFilePath);
                return;
            }

            string headerLine = reader.ReadLine();
            string[] cities = headerLine.Split(',');

            foreach (string city in cities)
            {
                if (!string.IsNullOrEmpty(city))
                {
                    regionDataByCity[city] = new List<string>();
                }
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] regions = line.Split(',');

                for (int i = 0; i < cities.Length; i++)
                {
                    if (!string.IsNullOrEmpty(regions[i]))
                    {
                        string city = cities[i];
                        regionDataByCity[city].Add(regions[i]);
                    }
                }
            }
        }
    }

    void OnCityChanged(int index)
    {
        string selectedCity = city_drop.options[index].text;
        cityData = selectedCity;
        print(cityData);

        region_drop.ClearOptions();

        if (regionDataByCity.ContainsKey(selectedCity))
        {
            List<string> regionList = regionDataByCity[selectedCity];

            foreach (string region in regionList)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(region);
                region_drop.options.Add(optionData);
            }
        }
        
        if (region_drop.options.Count > 0)
        {
            region_drop.value = 0;
            regionData = region_drop.options[region_drop.value].text;
        }
    }

    void OnRegionChanged(int index)
    {
        regionData = region_drop.options[index].text;
        print(regionData);

        school_drop.ClearOptions();

        if (regionData == "石門區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("石門實驗國中"));
        }
        else if (regionData == "淡水區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("正德國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("淡水國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("竹圍高中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("私立淡江高中"));
        }
        else if (regionData == "三芝區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("三芝國中"));
        }
        else if (regionData == "三重區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("三重高中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("二重國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("三和國中"));
        }
        else if (regionData == "霧峰區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("霧峰國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("光復國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("新光國中"));
        }
        else if (regionData == "太平區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("太平國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("中平國中"));
        }
        else if (regionData == "東勢區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("東華國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("東勢國中"));
            school_drop.options.Add(new TMP_Dropdown.OptionData("東新國中"));
        }
        else if (regionData == "和平區")
        {
            school_drop.options.Add(new TMP_Dropdown.OptionData("和平國中"));
        }
    }

    void OnSchoolChanged(int index)
    {
        schoolData = school_drop.options[index].text;
        print(schoolData);

        class_drop.ClearOptions();

        if (schoolData == "東勢國中")
        {
            class_drop.options.Add(new TMP_Dropdown.OptionData("一年一班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("一年二班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("二年一班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("三年一班"));
        }
        else if (schoolData == "東新國中")
        {
            class_drop.options.Add(new TMP_Dropdown.OptionData("一年一班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("一年二班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("二年一班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("二年二班"));
            class_drop.options.Add(new TMP_Dropdown.OptionData("三年一班"));
        }

        if (index >= 0 && index < class_drop.options.Count)
        {
            classData = class_drop.options[index].text;
            print(classData+"classData");
        }
        else
        {
            Debug.LogError($"OnSchoolChanged: 索引 {index} 超出了 class_drop.options 列表的范围。");
        }
    }

    void ReadYearData()
    {
        if (year_drop == null)
        {
            Debug.LogError("year_drop 为空，请确保它已在 Unity 编辑器中正确关联。");
            return;
        }
        year_drop.ClearOptions();
        year_drop.options.Add(new TMP_Dropdown.OptionData("113學年"));
        year_drop.options.Add(new TMP_Dropdown.OptionData("112學年"));
        year_drop.options.Add(new TMP_Dropdown.OptionData("111學年"));
    }

    void OnYearChanged(int index)
    {
        yearData = year_drop.options[index].text;
        print(yearData);       
    }

    void OnClassChanged(int index)
    {
        studentID_drop.ClearOptions();

        print(schoolData + yearData + classData);
        if (schoolData == "東勢國中" && yearData == "112學年" && classData == "一年二班")
        {
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("王小明"));
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("黃小美"));
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("林雅文"));
        }
        else if (schoolData == "東勢國中" && yearData == "112學年" && classData == "二年一班")
        {
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("林曼安"));
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("張彥良"));
            studentID_drop.options.Add(new TMP_Dropdown.OptionData("簡雅玲"));
        }
    }

    void OnStudentIDChanged(int index)
    {
        studentData = studentID_drop.options[index].text;
        print(studentData);   
        SaveData(); 
        PrintData();
    }

    void SaveData()
    {
        PlayerPrefs.SetString("CityData", cityData);
        PlayerPrefs.SetString("RegionData", regionData);
        PlayerPrefs.SetString("SchoolData", schoolData);
        PlayerPrefs.SetString("YearData", yearData);
        PlayerPrefs.SetString("ClassData", classData);
        PlayerPrefs.SetString("StudentData", studentData);
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        cityData = PlayerPrefs.GetString("CityData");
        regionData = PlayerPrefs.GetString("RegionData");
        schoolData = PlayerPrefs.GetString("SchoolData");
        yearData = PlayerPrefs.GetString("YearData");
        classData = PlayerPrefs.GetString("ClassData");
        studentData = PlayerPrefs.GetString("StudentData");
    }

    void PrintData()
    {
        Debug.Log($"CityData: {PlayerPrefs.GetString("CityData")}");
        Debug.Log($"RegionData: {PlayerPrefs.GetString("RegionData")}");
        Debug.Log($"SchoolData: {PlayerPrefs.GetString("SchoolData")}");
        Debug.Log($"YearData: {PlayerPrefs.GetString("YearData")}");
        Debug.Log($"ClassData: {PlayerPrefs.GetString("ClassData")}");
        Debug.Log($"StudentData: {PlayerPrefs.GetString("StudentData")}");
    }
}

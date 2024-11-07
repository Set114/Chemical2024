using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class react : MonoBehaviour
{
    public static int dcaseNow = 2;
    public static int sendScreenW = 512, sendScreenH = 288;
    public static int pairLabel = 1001;
    public static string localIP;
    public static bool isConnected = false;
    public GameObject ShowWarnObj;
    public Text ShowIP;
    private bool isSend = false;
    private string StudentIP;
    private int ScreenHeight, ScreenWidth;
    private int IPShowCheck = 0;
    // Start is called before the first frame update
    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;

        // Print the resolutions
        foreach (var res in resolutions)
        {
            ScreenWidth = res.width;
            ScreenHeight = res.height;
        }
        ScreenWidth = ScreenWidth / 2;
        sendScreenW = 512;
        sendScreenH = 288;
        isSend = false;
        ShowWarnObj.SetActive(false);
        ShowIP.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(sendScreenW == 512 && sendScreenH == 288 && !isSend && isConnected)
        {
            string sendString = localIP + " " + "CheckScreen";
            FMNetworkManager.instance.SendToServer(sendString);
            isSend = true;
            string[] IPstring = localIP.Split('.');
            StudentIP = "¾Ç¥ÍIP: " + IPstring[3];
            
        }
    }

    public void Action_ProcessStringData(string _string)
    {
        string[] sData;
        //Debug.Log("Received Data: " + _string);
        sData = _string.Split(' ');
        if(IPShowCheck == 0)
        {
            IPShowCheck = 1;
            ShowIP.text = StudentIP;

        }
        if (sData[0] == "screen1")
        {
            //SenderObj.SetActive(true);
            dcaseNow = 0;
            if(int.Parse(sData[1]) > ScreenWidth)
            {
                sendScreenW = ScreenWidth;
            } else
            {
                sendScreenW = int.Parse(sData[1]);
            }

            if(int.Parse(sData[2]) > ScreenHeight)
            {
                sendScreenH = ScreenHeight;
            } else
            {
                sendScreenH = int.Parse(sData[2]);
            }
        }
        else if (sData[0] == "screen2")
        {
            //SenderObj.SetActive(true);
            dcaseNow = 1;
            if (int.Parse(sData[1]) > ScreenWidth)
            {
                sendScreenW = ScreenWidth;
            }
            else
            {
                sendScreenW = int.Parse(sData[1]);
            }

            if (int.Parse(sData[2]) > ScreenHeight)
            {
                sendScreenH = ScreenHeight;
            }
            else
            {
                sendScreenH = int.Parse(sData[2]);
            }

        } else if (sData[0] == "screen3") {
            //SenderObj.SetActive(true);
            sendScreenW = 512;
            sendScreenH = 288;

        }
        else if (sData[0] == "screen4")
        {
            //SenderObj.SetActive(true);
            pairLabel = int.Parse(sData[1]);

        }
        else if (sData[0] == "screen5")
        {
            sendScreenW = 100;
            sendScreenH = 58;
            //SenderObj.SetActive(false);

        }
        else if (sData[0] == "Open1")
        {
            ShowWarnObj.SetActive(true);
        } else if (sData[0] == "Close1")
        {
            ShowWarnObj.SetActive(false);
        }
        else if (sData[0] == "Open2")
        {
            ShowWarnObj.SetActive(true);
        }
        else if (sData[0] == "Close2")
        {
            ShowWarnObj.SetActive(false);
        }

    }
}

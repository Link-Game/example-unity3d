/** 
 *Author:       wangyan
 *Date:         2017
 *Description:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.cloududu.linkgame.unity3d;

public class Demo : MonoBehaviour
{

    [SerializeField]
    Text logTxt;
    [SerializeField]
    Button getUserBtn;
    [SerializeField]
    Button shareTxtBtn;
    [SerializeField]
    Button shareImgBtn;
    [SerializeField]
    Button shareWebBtn;

    LinkGameOpenSDK lgosdk;
    void Awake()
    {
        getUserBtn.onClick.AddListener(OnAuth);
        shareTxtBtn.onClick.AddListener(OnShareTxt);
        shareImgBtn.onClick.AddListener(OnShareImg);
        shareWebBtn.onClick.AddListener(OnShareWeb);
    }

    // Use this for initialization
    void Start () {
        lgosdk = GetComponent<LinkGameOpenSDK>();
        lgosdk.getUserHandler = GetUserResultHandler;
        lgosdk.shareHandler = OnShareResultHandler;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void BeginLog(string msg)
    {
        logTxt.text = msg;
    }

    void AddLog(string msg)
    {
        logTxt.text += System.Environment.NewLine + msg;
    }

    bool CheckApp()
    {
        if (lgosdk.IsInstallApp() == false)
        {
            BeginLog("未安装游戏互联App!");
            return false;
        }

        return true;
    }

    void OnAuth()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始授权!");
        lgosdk.GetUserInfo();
    }

    void OnShareTxt()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始分享文字!");
        lgosdk.ShareText("分享一段文字");
    }

    void OnShareImg()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始分享本地图片!");

        Image img = shareImgBtn.GetComponent<Image>();
        Texture2D tex = img.sprite.texture;

        string dataPath = Application.persistentDataPath;
#if UNITY_EDITOR
        dataPath = Application.dataPath + "/..";
#endif

        string path = dataPath + "/share.jpg";
        System.IO.File.WriteAllBytes(path, tex.EncodeToJPG(50));

        lgosdk.ShareImage(path);
    }

    void OnShareWeb()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始分享图文链接!");
        lgosdk.ShareWeblinkWithUrl("分享标题", "分享内容", "www.baidu.com", "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1526052914596&di=57574136a555da480c1869f9e020158a&imgtype=0&src=http%3A%2F%2Fwww.eventdove.com%2Fresource%2F20160522%2F442472_20160522165632791.png");
    }

    void GetUserResultHandler(LGOpenResponseState state, string message, Hashtable result)
    {
        AddLog("授权回调!");
        string msg = "link_game_sdk ";
        string data = MiniJSON.jsonEncode(result);
        switch (state)
        {
            case LGOpenResponseState.Success:
                {
                    msg += "get user success: ";
                    msg += string.Format("json[{0}]", data);
                }
                break;
            case LGOpenResponseState.Fail:
                {
                    msg += string.Format("get user fail: {0}", message);
                }
                break;
            case LGOpenResponseState.Cancel:
                {
                    msg += string.Format("get user cancel: {0}", message);
                }
                break;
        }

        AddLog(string.Format("{0} data[{1}]", msg, data));
    }

    void OnShareResultHandler(LGOpenResponseState state, string message, Hashtable result)
    {
        AddLog("分享回调!");
        string msg = "link_game_sdk ";
        string data = MiniJSON.jsonEncode(result);
        switch (state)
        {
            case LGOpenResponseState.Success:
                {
                    string.Format("share success: {0}", message);
                }
                break;
            case LGOpenResponseState.Fail:
                {
                    msg += string.Format("share fail: {0}", message);
                }
                break;
            case LGOpenResponseState.Cancel:
                {
                    msg += string.Format("share cancel: {0}", message);
                }
                break;
        }

        AddLog(string.Format("{0} data[{1}]", msg, data));
    }
}

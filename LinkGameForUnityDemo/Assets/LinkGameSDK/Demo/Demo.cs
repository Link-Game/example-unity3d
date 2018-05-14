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
    string webUrl = "https://www.baidu.com/";
    [SerializeField]
    string imgUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1526052914596&di=57574136a555da480c1869f9e020158a&imgtype=0&src=http%3A%2F%2Fwww.eventdove.com%2Fresource%2F20160522%2F442472_20160522165632791.png";
    [SerializeField]
    LinkGameOpenSDK lgosdk;
    [SerializeField]
    Text logTxt;
    [SerializeField]
    Button getUserBtn;
    [SerializeField]
    Button shareTxtBtn;
    [SerializeField]
    Button shareImgBtn;
    [SerializeField]
    Button shareWebUrlBtn;
    [SerializeField]
    Button shareWebPathBtn;

    string imgPath;
    void Awake()
    {
        getUserBtn.onClick.AddListener(OnAuth);
        shareTxtBtn.onClick.AddListener(OnShareTxt);
        shareImgBtn.onClick.AddListener(OnShareImg);
        shareWebUrlBtn.onClick.AddListener(OnShareWebUrl);
        shareWebPathBtn.onClick.AddListener(OnShareWebPath);
    }

    // Use this for initialization
    void Start () {
        lgosdk.getUserHandler = GetUserResultHandler;
        lgosdk.shareHandler = OnShareResultHandler;
        SaveImg();
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

        lgosdk.ShareImage(imgPath);
    }

    void OnShareWebUrl()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始分享图文链接!");
        lgosdk.ShareWeblinkWithUrl("分享标题", "分享内容", webUrl, imgUrl);
    }

    void OnShareWebPath()
    {
        if (CheckApp() == false)
            return;

        BeginLog("开始分享图文链接!");
        lgosdk.ShareWeblinkWithPath("分享标题", "分享内容", webUrl, imgPath);

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

    void SaveImg()
    {
        Image img = shareImgBtn.GetComponent<Image>();
        Texture2D tex = img.sprite.texture;

        string dataPath = Application.persistentDataPath;
#if UNITY_EDITOR
        dataPath = Application.dataPath + "/..";
#endif

        imgPath = dataPath + "/share.jpg";
        System.IO.File.WriteAllBytes(imgPath, tex.EncodeToJPG(50));
    }
}

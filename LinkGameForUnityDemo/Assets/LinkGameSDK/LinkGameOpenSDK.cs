using UnityEngine;
using System;
using System.Collections;

namespace com.cloududu.linkgame.unity3d
{
    public enum LGOpenAction
    {
        AUTHORIZING = 0,
        SHARE,
        GET_USER,
    }

    public enum LGOpenResponseState
    {
        Success = 1,            //Success
        Fail = 2,               //Failure
        Cancel = 3              //Cancel
    }

    public class LinkGameOpenSDK : MonoBehaviour
    {
        public string AppId = "0124578befjklmopuy";
        public string AppSecret = "e9af7fa05833abf1296fb274fd5b0582";

        LinkGameOpenAuth lgopenAuth;
        LinkGameOpenSDKImpl lgopenSDKUtils;

        public ResultHandler getUserHandler;
        public ResultHandler shareHandler;

        private void Awake()
        {
#if UNITY_ANDROID
            lgopenSDKUtils = new AndroidLinkGameOpenSDKImpl(gameObject.name, "_Callback");
#elif UNITY_IPHONE
            lgopenSDKUtils = new iOSLinkGameOpenSDKImpl(gameObject.name, "_Callback");
#else
            lgopenSDKUtils = new LinkGameOpenSDKImpl();
#endif
            lgopenAuth = new LinkGameOpenAuth(this);
            lgopenSDKUtils.InitSDK(AppId, AppSecret);
        }

        public bool IsInstallApp()
        {
            return lgopenSDKUtils.IsInstallApp();
        }

        public void GetUserInfo()
        {
            lgopenSDKUtils.Authorizing();
        }

        public void ShareText(string text)
        {
            lgopenSDKUtils.ShareText(text);
        }

        public void ShareImage(string filePath)
        {
            lgopenSDKUtils.ShareImage(filePath);
        }

        public void ShareWeblinkWithPath(string title, string text, string url, string imagePath)
        {
            lgopenSDKUtils.ShareWeblinkWithPath(title, text, url, imagePath);
        }

        public void ShareWeblinkWithUrl(string title, string text, string url, string imageUrl)
        {
            lgopenSDKUtils.ShareWeblinkWithUrl(title, text, url, imageUrl);
        }

        public void _Callback(string data)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> _Callback: {0}",data);

            if (string.IsNullOrEmpty(data))
                return;

            Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
            if (res == null || res.Count <= 0)
                return;

            int code = Convert.ToInt32(res["code"]);
            string msg = Convert.ToString(res["message"]);
            Hashtable result = (Hashtable)res["data"];

            CheckSDK(code, msg, result);
            CheckGetUser(code, msg, result);
        }

        void CheckSDK(int code, string message, Hashtable result)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> CheckSDK: code[{0}] message[{1}] result[{2}]", code, message, MiniJSON.jsonEncode(result));

            if (result == null || result.Count <= 0)
                return;

            int type = Convert.ToInt32(result["type"]);

            switch (code)
            {
                //sdk获取token成功
                case 20000:
                    {
                        OnComplete((LGOpenAction)type, message, result);
                    }
                    break;
                //用户取消
                case 20001:
                //用户拒绝
                case 20002:
                    {
                        OnCancel((LGOpenAction)type, message, result);
                    }
                    break;
                //参数错误
                case 20003:
                //超时
                case 20004:
                //其他错误
                case 20005:
                    {
                        OnError((LGOpenAction)type, message, result);
                    }
                    break;
            }
        }

        void CheckGetUser(int code, string message, Hashtable result)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> CheckGetUser: code[{0}] json[{1}]", code, MiniJSON.jsonEncode(result));

            switch (code)
            {
                //获取用户信息成功
                case 200:
                    {
                        OnComplete(LGOpenAction.GET_USER, message, result);
                    }
                    break;
                case 20100:
                case 20101:
                case 20103:
                case 20104:
                case 20105:
                case 30400:
                    {
                        OnError(LGOpenAction.GET_USER, message, result);
                    }
                    break;
            }
        }

        void OnComplete(LGOpenAction act, string message, Hashtable result)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> OnComplete: act[{0}] data[{1}]", act.ToString(), MiniJSON.jsonEncode(result));

            switch (act)
            {
                case LGOpenAction.AUTHORIZING:
                    {
                        lgopenAuth.OnAuthResultHandler(LGOpenResponseState.Success, message, result);
                    }
                    break;
                case LGOpenAction.SHARE:
                    {
                        shareHandler(LGOpenResponseState.Success, message, result);
                    }
                    break;
                case LGOpenAction.GET_USER:
                    {
                        getUserHandler(LGOpenResponseState.Success, message, result);
                    }
                    break;
            }
        }

        void OnError(LGOpenAction act, string message, Hashtable result)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> OnError: act[{0}] data[{1}]", act.ToString(), MiniJSON.jsonEncode(result));

            switch (act)
            {
                case LGOpenAction.AUTHORIZING:
                    {
                        lgopenAuth.OnAuthResultHandler(LGOpenResponseState.Fail, message, result);
                    }
                    break;
                case LGOpenAction.SHARE:
                    {
                        shareHandler(LGOpenResponseState.Fail, message, result);
                    }
                    break;
                case LGOpenAction.GET_USER:
                    {
                        getUserHandler(LGOpenResponseState.Fail, message, result);
                    }
                    break;
            }
        }

        void OnCancel(LGOpenAction act, string message, Hashtable result)
        {
            Debug.LogFormat("LinkGameOpenSDK ===> OnCancel: act[{0}] data[{1}]", act.ToString(), MiniJSON.jsonEncode(result));

            switch (act)
            {
                case LGOpenAction.AUTHORIZING:
                    {
                        lgopenAuth.OnAuthResultHandler(LGOpenResponseState.Cancel, message, result);
                    }
                    break;
                case LGOpenAction.SHARE:
                    {
                        shareHandler(LGOpenResponseState.Cancel, message, result);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act">ActionType</param>
        /// <param name="data">Json</param>
        public delegate void ResultHandler(LGOpenResponseState state, string message, Hashtable data);
    }
}

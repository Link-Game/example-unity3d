using System;
using System.Collections;
using UnityEngine;

namespace com.cloududu.linkgame.unity3d
{
    public class LinkGameOpenAuth
    {
        public const string API_URL = "http://api.zhuanxinyu.com/v1/user/get-info";

        LinkGameOpenSDK cmosdk;

        public LinkGameOpenAuth(LinkGameOpenSDK sdk)
        {
            cmosdk = sdk;
        }

        public void OnAuthResultHandler(LGOpenResponseState state, string message, Hashtable res)
        {
            string msg = "cmopend ";
            switch(state)
            {
                case LGOpenResponseState.Success:
                    {
                        msg += "get auth success: ";

                        string refreshToken = Convert.ToString(res["refreshToken"]);

                        msg += string.Format("refresh_token[{0}]", refreshToken);

                        cmosdk.StartCoroutine(GetUserInfo(refreshToken));
                    }
                    break;
                case LGOpenResponseState.Fail:
                    {
                        msg += string.Format("get auth fail: message[{0}]", message);
                        cmosdk.getUserHandler(LGOpenResponseState.Fail, message, res);
                    }
                    break;
                case LGOpenResponseState.Cancel:
                    {
                        msg += string.Format("get auth cancel: message[{0}]", message);
                        cmosdk.getUserHandler(LGOpenResponseState.Cancel, message, res);
                    }
                    break;
            }

            Debug.LogFormat("{0} ==> data[{1}]", msg, res);
        }

        IEnumerator GetUserInfo(string token)
        {
            WWWForm reqForm = new WWWForm();
            reqForm.AddField("app_id", cmosdk.AppId);
            reqForm.AddField("refresh_token", token);

            WWW www = new WWW(API_URL, reqForm);
            yield return www;

            string data = "";
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                data = www.text;
            }
            else
            {
                Hashtable res = new Hashtable();
                res.Add("code", 30400);
                res.Add("message", www.error);
                data = MiniJSON.jsonEncode(res);
            }

            cmosdk._Callback(data);
        }
    }
}


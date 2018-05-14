using System;
using System.Collections;
using UnityEngine;

namespace com.cloududu.linkgame.unity3d
{
#if UNITY_ANDROID
    public class AndroidLinkGameOpenSDKImpl : LinkGameOpenSDKImpl
    {
        private AndroidJavaObject lgosdk;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GameObject Name"></param>
        /// <param name="Callback Function"></param>
        public AndroidLinkGameOpenSDKImpl(string name, string callback)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> AndroidLinkGameOpenSDKImpl!");
            try
            {
                lgosdk = new AndroidJavaObject("com.cloududu.linkgame.opensdk.domain.LinkGameSDK");

                lgosdk.Call("registerSyncCallBack", name, callback);
            }
            catch (Exception e) 
            {
                Debug.LogErrorFormat("LinkGameOpenSDK {0} Exception caught.", e);
            }
        }

        public override void InitSDK(string appId, string appSecret)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> InitSDK!");
            if (lgosdk != null)
            {
                lgosdk.Call("init", appId, appSecret);
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> init!");
            }
        }

        public override void Authorizing()
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> Authorizing!");
            if (lgosdk != null)
            {
                lgosdk.Call("authorization");
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> authorization!");
            }
        }

        public override void ShareText(string text)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> ShareText!");
            if (lgosdk != null)
            {
                lgosdk.Call("shareText", text);
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> shareText!");
            }
        }

        public override void ShareImage(string filePath)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> ShareImage!");
            if (lgosdk != null)
            {
                filePath = "file://" + filePath;
                lgosdk.Call("shareImageWithFilePath", filePath);
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> shareImageWithFilePath!");
            }
        }

        public override void ShareWeblinkWithPath(string title, string text, string url, string imagePath)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> ShareWeblinkWithPath!");
            if (lgosdk != null)
            {
                imagePath = "file://" + imagePath;
                lgosdk.Call("shareWebPageWithUrl", title, text, url, imagePath);
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> shareWebPageWithPath!");
            }
        }

        public override void ShareWeblinkWithUrl(string title, string text, string url, string imageUrl)
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> ShareWeblinkWithUrl!");
            if (lgosdk != null)
            {
                lgosdk.Call("shareWebPageWithUrl", title, text, url, imageUrl);
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> shareWebPageWithUrl!");
            }
        }

        public override bool IsInstallApp()
        {
            Debug.Log("AndroidLinkGameOpenSDKImpl ===> IsInstallApp!");
            if (lgosdk != null)
            {
                Debug.Log("AndroidLinkGameOpenSDKImpl ===> Call -> isLinkGameInstalled!");
                return lgosdk.Call<bool>("isLinkGameInstalled");
            }
            return false;
        }
    }
#endif
}

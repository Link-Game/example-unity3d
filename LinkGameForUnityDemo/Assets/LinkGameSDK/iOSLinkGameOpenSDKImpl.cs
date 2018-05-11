using System.Runtime.InteropServices;
using UnityEngine;

namespace com.cloududu.linkgame.unity3d
{
#if UNITY_IPHONE
    public class iOSLinkGameOpenSDKImpl : LinkGameOpenSDKImpl
    {
        [DllImport("__Internal")]
        private static extern void cmopensdk_initCMOpenSDK(string appID, string appSecret);
        [DllImport("__Internal")]
        private static extern void ckopensdk_registerCallBack(string callBackObjectName, string callBackMethodName);
        [DllImport("__Internal")]
        private static extern void cmopensdk_requestLogin();

        [DllImport("__Internal")]
        private static extern void cmopensdk_requestShareText(string text);
        [DllImport("__Internal")]
        private static extern void cmopensdk_requestShareImageWithPath(string filePath);
        [DllImport("__Internal")]
        private static extern void cmopensdk_requestShareWebLinkWithImageFilePath(string title, string text, string url, string imageFilePath);
        [DllImport("__Internal")]
        private static extern void cmopensdk_requestShareWebLinkWithImageUrl(string title, string text, string url, string imageUrl);

        [DllImport("__Internal")]
        private static extern bool cmopensdk_isInstallYXHL();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GameObject Name"></param>
        /// <param name="Callback Function"></param>
        public iOSLinkGameOpenSDKImpl(string name, string callback)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> iOSCMOpenSDKImpl!");

            ckopensdk_registerCallBack(name, callback);
        }

        public override void InitSDK(string appId, string appSecret)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> InitSDK!");

            cmopensdk_initCMOpenSDK(appId, appSecret);
        }

        public override void Authorizing()
        {
            Debug.Log("iOSCMOpenSDKImpl ===> Authorizing!");

            cmopensdk_requestLogin();
        }

        public override void ShareText(string text)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareText!");

            cmopensdk_requestShareText(text);
        }

        public override void ShareImage(string filePath)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareImage!");

            cmopensdk_requestShareImageWithPath(filePath);
        }

        public override void ShareWeblinkWithPath(string title, string text, string url, string imagePath)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareWeblinkWithPath!");

            cmopensdk_requestShareWebLinkWithImageFilePath(title, text, url, imagePath);
        }

        public override void ShareWeblinkWithUrl(string title, string text, string url, string imageUrl)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareWeblinkWithUrl!");

            cmopensdk_requestShareWebLinkWithImageUrl(title, text, url, imageUrl);
        }

        public override bool IsInstallApp()
        {
            Debug.Log("iOSCMOpenSDKImpl ===> IsInstallApp!");

            return cmopensdk_isInstallYXHL();
        }
    }
#endif
}

using System.Runtime.InteropServices;
using UnityEngine;

namespace com.cloududu.linkgame.unity3d
{
#if UNITY_IPHONE
    public class iOSLinkGameOpenSDKImpl : LinkGameOpenSDKImpl
    {
        [DllImport("__Internal")]
        private static extern void lgopensdk_initLGOpenSDK(string appID, string appSecret);
        [DllImport("__Internal")]
        private static extern void ckopensdk_registerCallBack(string callBackObjectName, string callBackMethodName);
        [DllImport("__Internal")]
        private static extern void lgopensdk_requestLogin();

        [DllImport("__Internal")]
        private static extern void lgopensdk_requestShareText(string text);
        [DllImport("__Internal")]
        private static extern void lgopensdk_requestShareImageWithPath(string filePath);
        [DllImport("__Internal")]
        private static extern void lgopensdk_requestShareWebLinkWithImageFilePath(string title, string text, string url, string imageFilePath);
        [DllImport("__Internal")]
        private static extern void lgopensdk_requestShareWebLinkWithImageUrl(string title, string text, string url, string imageUrl);

        [DllImport("__Internal")]
        private static extern bool lgopensdk_isInstallYXHL();

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

            lgopensdk_initLGOpenSDK(appId, appSecret);
        }

        public override void Authorizing()
        {
            Debug.Log("iOSCMOpenSDKImpl ===> Authorizing!");

            lgopensdk_requestLogin();
        }

        public override void ShareText(string text)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareText!");

            lgopensdk_requestShareText(text);
        }

        public override void ShareImage(string filePath)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareImage!");

            lgopensdk_requestShareImageWithPath(filePath);
        }

        public override void ShareWeblinkWithPath(string title, string text, string url, string imagePath)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareWeblinkWithPath!");

            lgopensdk_requestShareWebLinkWithImageFilePath(title, text, url, imagePath);
        }

        public override void ShareWeblinkWithUrl(string title, string text, string url, string imageUrl)
        {
            Debug.Log("iOSCMOpenSDKImpl ===> ShareWeblinkWithUrl!");

            lgopensdk_requestShareWebLinkWithImageUrl(title, text, url, imageUrl);
        }

        public override bool IsInstallApp()
        {
            Debug.Log("iOSCMOpenSDKImpl ===> IsInstallApp!");

            return lgopensdk_isInstallYXHL();
        }
    }
#endif
}

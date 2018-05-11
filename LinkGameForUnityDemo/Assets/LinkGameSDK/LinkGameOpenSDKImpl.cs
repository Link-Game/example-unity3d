
namespace com.cloududu.linkgame.unity3d
{
    public class LinkGameOpenSDKImpl
    {
        public virtual void InitSDK(string appId, string appSecret) { }

        public virtual void Authorizing() { }

        public virtual void ShareText(string text) { }
        public virtual void ShareImage(string filePath) { }
        public virtual void ShareWeblinkWithPath(string title, string text, string url, string imagePath) { }
        public virtual void ShareWeblinkWithUrl(string title, string text, string url, string imageUrl) { }
        public virtual bool IsInstallApp() { return false; }
    }
}

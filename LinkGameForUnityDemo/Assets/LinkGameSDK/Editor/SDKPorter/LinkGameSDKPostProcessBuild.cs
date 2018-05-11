using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using com.cloududu.unity3d.sdkporter;
using System.IO;


public static class ShareSDKPostProcessBuild {
	[PostProcessBuild]
	public static void onPostProcessBuild(BuildTarget target,string targetPath){
		string unityEditorAssetPath = Application.dataPath;

		if (target != BuildTarget.iOS) {
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		XCProject project = new XCProject (targetPath);
		//var files = System.IO.Directory.GetFiles( unityEditorAssetPath, "*.projmods", System.IO.SearchOption.AllDirectories );
		var files = System.IO.Directory.GetFiles( unityEditorAssetPath + "/Editor/SDKPorter", "*.projmods", System.IO.SearchOption.AllDirectories);
		foreach( var file in files ) {
			project.ApplyMod( file );
		}

		//如需要预配置Xocode中的URLScheme 和 白名单,请打开下两行代码,并自行配置相关键值
		string projPath = Path.GetFullPath (targetPath);
		EditInfoPlist (projPath);


		//Finally save the xcode project
		project.Save();

	}

	private static void EditInfoPlist(string projPath){

		XCPlist plist = new XCPlist (projPath);

		//URL Scheme 添加
		string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
			<array>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>wx6b7f3e78c2f722a4</string>
					</array>
					<key>CFBundleURLName</key>
					<string>wxShare</string>
				</dict>
                <dict>
                  <key>CFBundleURLSchemes</key>
                  <array>
                    <string>cmsy01789abefghjmqtuyz</string>
                  </array>
                  <key>CFBundleURLName</key>
                  <string>yxhlApp</string>
                </dict>
                <dict>
                  <key>CFBundleURLSchemes</key>
                  <array>
                    <string>app23jj</string>
                  </array>
                  <key>CFBundleURLName</key>
                  <string>jiujiuApp</string>
                </dict>
			</array>";

		//白名单添加
		string LSAdd = @"
		<key>LSApplicationQueriesSchemes</key>
			<array>
			<string>whatsapp</string>
			<string>wechat</string>
			<string>weixin</string>
			<string>cmsyyouxihulian</string>
		</array>
        <key>NSMicrophoneUsageDescription</key>
		<string></string>
		<key>NSPhotoLibraryUsageDescription</key>
		<string></string>
        <key>NSLocationWhenInUseUsageDescription</key>
        <string></string>";


		//在plist里面增加一行
		plist.AddKey(PlistAdd);
		plist.AddKey (LSAdd);
		plist.Save();
	}


}
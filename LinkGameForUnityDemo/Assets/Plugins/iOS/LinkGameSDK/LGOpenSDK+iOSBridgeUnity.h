//
//  LGOpenSDK+iOSBridgeUnity.h
//  UnityPluginProject
//
//  Created by 刘万林 on 2018/4/16.
//

#import <Foundation/Foundation.h>
#define __CString const char *

#ifdef __cplusplus
extern "C" {
#endif

    /**
     初始化SDK

     @param appID APPID
     @param appSecret appSecret
     */
    extern void lgopensdk_initLGOpenSDK(__CString appID, __CString appSecret);

    /**
     注册回调

     回调数据格式:

     {
         "code":LGOpenSDKErrorCode,
         "msg":"",
         "data":{
             "type":LGRequestType,
             "refreshToken":""
            }
     }

     参数说明:

     code   请求结果状态码
                20000:没有错误
                20001:用户取消
                20002:用户拒绝
                20003:参数错误
                20004:超时,仅在授权登录时会出现,分享时不会出现该错误
                20005:其他错误
     msg    对应状态码的说明
     data   返回的数据
     type   返回数据的类型:0 授权登录的返回,1 分享的返回
     refreshToken 授权登录成功后得到的用于访问用户数据的token,仅在type为0时 有此参数.

     @param callBackObjectName 接收回调的UnityObject名称
     @param callBackMethodName 接收回调的方法名
     */
    extern void ckopensdk_registerCallBack(__CString callBackObjectName,__CString callBackMethodName);

    /**
     请求使用游戏互联登录

     */
    extern void lgopensdk_requestLogin(void);

    /**
     发起分享文字的请求

     @param text 要分享的文字

     */
    extern void lgopensdk_requestShareText(__CString text);

    /**
     发起分享图片的请求

     @param filePath 图片文件的路径

     */
    extern void lgopensdk_requestShareImageWithPath(__CString filePath);

    /**
     发起分享图片的请求

     @param imageData 图片的二进制数据
     @param lenth 数据的长度(单位:Byte)

     */
    extern void lgopensdk_requestShareImageWithData(const void * imageData,const int lenth);

    /**
     发起分享网页链接的请求

     @param title 标题
     @param text 内容
     @param url 网页链接
     @param imageFilePath 图片文件的路径(请保证图片不大于32KB)

     */
    extern void lgopensdk_requestShareWebLinkWithImageFilePath(__CString title,__CString text,__CString url,__CString imageFilePath);

    /**
     发起分享网页链接的请求

     @param title 标题
     @param text 内容
     @param url 网页链接
     @param fileData 图片文件数据(请保证图片不大于32KB)
     @param lenth 数据长度(单位:Byte)

     */
    extern void lgopensdk_requestShareWebLinkWithImageFileData(__CString title,__CString text,__CString url,const void * fileData,const int lenth);

    /**
     发起分享网页链接的请求
     
     @param title 标题
     @param text 内容
     @param url 网页链接
     @param imageUrl 图片的网络url(请使用HTTPS链接,否则iOS端可能无法正常显示)

     */
    extern void lgopensdk_requestShareWebLinkWithImageUrl(__CString title,__CString text,__CString url,__CString imageUrl);

    /**
     检测是否安装了游戏互联

     @return 是否安装了游戏互联
     */
    extern BOOL lgopensdk_isInstallLinkGame();

#ifdef __cplusplus
}
#endif


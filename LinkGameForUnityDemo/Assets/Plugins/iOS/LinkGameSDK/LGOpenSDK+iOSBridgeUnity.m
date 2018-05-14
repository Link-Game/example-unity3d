//
//  LGOpenSDK+iOSBridgeUnity.m
//  UnityPluginProject
//
//  Created by 刘万林 on 2018/4/16.
//

#import "LGOpenSDK+iOSBridgeUnity.h"
#import "UnityAppController.h"
#import <LinkGameOpenSDK/LGOpenSDK.h>

#define _StrC2OC(__CStr) [NSString stringWithUTF8String:__CStr]

//void UnitySendMessage(const char* obj, const char* method, const char* msg);

@interface LGOpenSDKUnityTool:NSObject<LinkGameOpenSDKDelegate>
+(instancetype)share;
@property (copy, nonatomic) NSString * callBackObjectName;
@property (copy, nonatomic) NSString * callBackMethodName;
@end

@implementation LGOpenSDKUnityTool
+(instancetype)share{
    static LGOpenSDKUnityTool * tool = nil;
    if(!tool){
        tool = [[LGOpenSDKUnityTool alloc] init];
    }
    return tool;
}
#pragma mark - ● LinkGameOpenSDKDelegate
- (void)didResponsed:(LGBaseResponse *)response {
    NSMutableDictionary * data = [NSMutableDictionary dictionary];
    data[@"type"] = @(response.ResultRequestType);
    if (response.ResultRequestType == LGRequestTypeAuthorize) {
        LGAuthorizeResponse * authResponse = (LGAuthorizeResponse *)response;
        data[@"refreshToken"] = authResponse.RefreshToken;
    }
    [LGOpenSDKUnityTool sendToUnityCode:response.ErrorCode Message:response.Message data:data];
}

+(void)sendToUnityCode:(NSInteger)code Message:(NSString *)message data:(NSDictionary *)dic{
    NSDictionary * dataDic = @{
                               @"code":@(code),
                               @"message":message?:@"",
                               @"data":dic?:[NSNull new]
                               };
    NSError * error = nil;
    NSData * jsonData = [NSJSONSerialization dataWithJSONObject:dataDic options:NSJSONWritingPrettyPrinted error:&error];
    if (!error) {
        NSString * messageStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage([LGOpenSDKUnityTool share].callBackObjectName.UTF8String, [LGOpenSDKUnityTool share].callBackMethodName.UTF8String, messageStr.UTF8String);
    }else{
        NSString * messageStr = @"{\"code\":20005,\"message\":\"json解析出错\",\"data\":[]}";
        UnitySendMessage([LGOpenSDKUnityTool share].callBackObjectName.UTF8String, [LGOpenSDKUnityTool share].callBackMethodName.UTF8String, messageStr.UTF8String);
    }
}

@end



/**
 初始化SDK

 @param appID APPID
 @param appSecret appSecret
 */
void lgopensdk_initLGOpenSDK(__CString appID, __CString appSecret){
    [[LGOpenSDK share] RegisterAppID:_StrC2OC(appID) AppSecret:_StrC2OC(appSecret) Delegate:[LGOpenSDKUnityTool share]];
}

/**
 注册回调

 @param callBackObjectName 接收回调的UnityObject名称
 @param callBackMethodName 接收回调的方法名
 */
void ckopensdk_registerCallBack(__CString callBackObjectName,__CString callBackMethodName){
    [LGOpenSDKUnityTool share].callBackObjectName = _StrC2OC(callBackObjectName);
    [LGOpenSDKUnityTool share].callBackMethodName = _StrC2OC(callBackMethodName);
}

/**
 请求使用游戏互联登录

 @return 发起请求是否成功
 */
void lgopensdk_requestLogin(void){
    LGAuthRequest * request = [LGAuthRequest new];
    [[LGOpenSDK share] sendRequest:request];
}

/**
 发起分享文字的请求

 @param text 要分享的文字
 @return 发起请求是否成功
 */
void lgopensdk_requestShareText(__CString text){
    if (text==NULL) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文本不能为空" data:nil];
        return;
    }
    LGTextShareRequest * request = [LGTextShareRequest RequestWithText:_StrC2OC(text)];
    [[LGOpenSDK share]sendRequest:request];
}

/**
 发起分享图片的请求

 @param filePath 图片文件的路径
 @return 发起请求是否成功
 */
void lgopensdk_requestShareImageWithPath(__CString filePath){
    if (filePath==NULL) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件路径不能为空" data:nil];
        return;
    }
    BOOL isDirectory = NO;
    if (![[NSFileManager defaultManager] fileExistsAtPath:_StrC2OC(filePath) isDirectory:&isDirectory]) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件不存在或不能访问" data:nil];
        return;
    }
    if (isDirectory) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件路径错误,指向的是文件夹" data:nil];
        return;
    }

    LGImageShareRequest * request = [LGImageShareRequest RequestWithImageData:[NSData dataWithContentsOfFile:_StrC2OC(filePath)]];
    [[LGOpenSDK share] sendRequest:request];
}

/**
 发起分享图片的请求

 @param imageData 图片的二进制数据
 @param lenth 数据的长度(单位:Byte)
 @return 发起请求是否成功
 */
void lgopensdk_requestShareImageWithData(const void * imageData,const int lenth){
    if (imageData==NULL) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"图片不能为空" data:nil];
        return;
    }
    if (lenth==0) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"图片数据长度不能为0" data:nil];
        return;
    }
    LGImageShareRequest * request = [LGImageShareRequest RequestWithImageData:[NSData dataWithBytes:imageData length:lenth]];
    [[LGOpenSDK share] sendRequest:request];
}

/**
 发起分享网页链接的请求

 @param title 标题
 @param text 内容
 @param url 网页链接
 @param imageFilePath 图片文件的路径(请保证图片不大于32KB)
 @return 发起请求是否成功
 */
void lgopensdk_requestShareWebLinkWithImageFilePath(__CString title,__CString text,__CString url,__CString imageFilePath){

    if (imageFilePath==NULL) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件路径不能为空" data:nil];
        return;
    }
    BOOL isDirectory = NO;
    if (![[NSFileManager defaultManager] fileExistsAtPath:_StrC2OC(imageFilePath) isDirectory:&isDirectory]) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件不存在或不能访问" data:nil];
        return;
    }
    if (isDirectory) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"文件路径错误,指向的是文件夹" data:nil];
        return;
    }
    if ([[[NSFileManager defaultManager] attributesOfItemAtPath:_StrC2OC(imageFilePath) error:nil] fileSize]>32*1024) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"分享网页链接的图片文件不能大于32KB" data:nil];
        return;
    }
    LGWebPageShareRequest * request = [LGWebPageShareRequest RequestWithImageFilePath:_StrC2OC(imageFilePath) Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString: _StrC2OC(url)]];
    [[LGOpenSDK share]sendRequest:request];
}

/**
 发起分享网页链接的请求

 @param title 标题
 @param text 内容
 @param url 网页链接
 @param fileData 图片文件数据(请保证图片不大于32KB)
 @param lenth 数据长度(单位:Byte)
 @return 发起请求是否成功
 */
void lgopensdk_requestShareWebLinkWithImageFileData(__CString title,__CString text,__CString url,const void * fileData,const int lenth){
    if (lenth>32*1024) {
        [LGOpenSDKUnityTool sendToUnityCode:LGOpenSDKErrorCodeParematersError Message:@"图片不能大于32KB" data:nil];
        return;
    }
    if (fileData==NULL||lenth==0) {
        LGWebPageShareRequest * request = [LGWebPageShareRequest RequestWithImageData:nil Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
        [[LGOpenSDK share] sendRequest:request];
    }else{
    LGWebPageShareRequest * request = [LGWebPageShareRequest RequestWithImageData:[NSData dataWithBytes:fileData length:lenth] Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
        [[LGOpenSDK share] sendRequest:request];

    }
}

/**
 发起分享网页链接的请求

 @param title 标题
 @param text 内容
 @param url 网页链接
 @param imageUrl 图片的网络url
 @return 发起请求是否成功
 */
void lgopensdk_requestShareWebLinkWithImageUrl(__CString title,__CString text,__CString url,__CString imageUrl){
    LGWebPageShareRequest * request = [LGWebPageShareRequest RequestWithWebImageUrl:_StrC2OC(imageUrl) Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
    [[LGOpenSDK share] sendRequest:request];
}

/**
 检测是否安装了游戏互联

 @return 是否安装了游戏互联
 */
extern BOOL lgopensdk_isInstallYXHL(){
    return [[LGOpenSDK share]isInstallYXHL];
}


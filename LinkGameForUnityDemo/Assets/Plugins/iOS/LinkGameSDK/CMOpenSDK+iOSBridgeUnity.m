//
//  CMOpenSDK+iOSBridgeUnity.m
//  UnityPluginProject
//
//  Created by 刘万林 on 2018/4/16.
//

#import "CMOpenSDK+iOSBridgeUnity.h"
#import "UnityAppController.h"
#import <CMOpenSDK/CMOpenSDK.h>

#define _StrC2OC(__CStr) [NSString stringWithUTF8String:__CStr]

@interface CMOpenSDKUnityTool:NSObject<CMOpenSDKDelegate>
+(instancetype)share;
@property (copy, nonatomic) NSString * callBackObjectName;
@property (copy, nonatomic) NSString * callBackMethodName;
@end

@implementation CMOpenSDKUnityTool
+(instancetype)share{
    static CMOpenSDKUnityTool * tool = nil;
    if(!tool){
        tool = [[CMOpenSDKUnityTool alloc] init];
    }
    return tool;
}
#pragma mark - ● CMOpenSDKDelegate
- (void)didResponsed:(CMBaseResponse *)response {
    NSMutableDictionary * data = [NSMutableDictionary dictionary];
    data[@"type"] = @(response.ResultRequestType);
    if (response.ResultRequestType == CMRequestTypeAuthorize) {
        CMAuthorizeResponse * authResponse = (CMAuthorizeResponse *)response;
        data[@"refreshToken"] = authResponse.RefreshToken;
    }
    [CMOpenSDKUnityTool sendToUnityCode:response.ErrorCode Message:response.Message data:data];
}

+(void)sendToUnityCode:(NSInteger)code Message:(NSString *)message data:(NSDictionary *)dic{
    NSDictionary * dataDic = @{
                               @"code":@(code),
                               @"message":message,
                               @"data":dic?:[NSNull new]
                               };
    NSError * error = nil;
    NSData * jsonData = [NSJSONSerialization dataWithJSONObject:dataDic options:NSJSONWritingPrettyPrinted error:&error];
    if (!error) {
        NSString * messageStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage([CMOpenSDKUnityTool share].callBackObjectName.UTF8String, [CMOpenSDKUnityTool share].callBackMethodName.UTF8String, messageStr.UTF8String);
    }else{
        NSString * messageStr = @"{\"code\":20005,\"message\":\"json解析出错\",\"data\":[]}";
        UnitySendMessage([CMOpenSDKUnityTool share].callBackObjectName.UTF8String, [CMOpenSDKUnityTool share].callBackMethodName.UTF8String, messageStr.UTF8String);
    }
}

@end



/**
 初始化SDK

 @param appID APPID
 @param appSecret appSecret
 */
void cmopensdk_initCMOpenSDK(__CString appID, __CString appSecret){
    [[CMOpenSDK share] RegisterAppID:_StrC2OC(appID) AppSecret:_StrC2OC(appSecret) Delegate:[CMOpenSDKUnityTool share]];
}

/**
 注册回调

 @param callBackObjectName 接收回调的UnityObject名称
 @param callBackMethodName 接收回调的方法名
 */
void ckopensdk_registerCallBack(__CString callBackObjectName,__CString callBackMethodName){
    [CMOpenSDKUnityTool share].callBackObjectName = _StrC2OC(callBackObjectName);
    [CMOpenSDKUnityTool share].callBackMethodName = _StrC2OC(callBackMethodName);
}

/**
 请求使用游戏互联登录

 @return 发起请求是否成功
 */
void cmopensdk_requestLogin(void){
    CMAuthRequest * request = [CMAuthRequest new];
    [[CMOpenSDK share] sendRequest:request];
}

/**
 发起分享文字的请求

 @param text 要分享的文字
 @return 发起请求是否成功
 */
void cmopensdk_requestShareText(__CString text){
    if (text==NULL) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文本不能为空" data:nil];
        return;
    }
    CMTextShareRequest * request = [CMTextShareRequest RequestWithText:_StrC2OC(text)];
    [[CMOpenSDK share]sendRequest:request];
}

/**
 发起分享图片的请求

 @param filePath 图片文件的路径
 @return 发起请求是否成功
 */
void cmopensdk_requestShareImageWithPath(__CString filePath){
    if (filePath==NULL) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件路径不能为空" data:nil];
        return;
    }
    BOOL isDirectory = NO;
    if (![[NSFileManager defaultManager] fileExistsAtPath:_StrC2OC(filePath) isDirectory:&isDirectory]) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件不存在或不能访问" data:nil];
        return;
    }
    if (isDirectory) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件路径错误,指向的是文件夹" data:nil];
        return;
    }

    CMImageShareRequest * request = [CMImageShareRequest RequestWithImageData:[NSData dataWithContentsOfFile:_StrC2OC(filePath)]];
    [[CMOpenSDK share] sendRequest:request];
}

/**
 发起分享图片的请求

 @param imageData 图片的二进制数据
 @param lenth 数据的长度(单位:Byte)
 @return 发起请求是否成功
 */
void cmopensdk_requestShareImageWithData(const void * imageData,const int lenth){
    if (imageData==NULL) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"图片不能为空" data:nil];
        return;
    }
    if (lenth==0) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"图片数据长度不能为0" data:nil];
        return;
    }
    CMImageShareRequest * request = [CMImageShareRequest RequestWithImageData:[NSData dataWithBytes:imageData length:lenth]];
    [[CMOpenSDK share] sendRequest:request];
}

/**
 发起分享网页链接的请求

 @param title 标题
 @param text 内容
 @param url 网页链接
 @param imageFilePath 图片文件的路径(请保证图片不大于32KB)
 @return 发起请求是否成功
 */
void cmopensdk_requestShareWebLinkWithImageFilePath(__CString title,__CString text,__CString url,__CString imageFilePath){

    if (imageFilePath==NULL) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件路径不能为空" data:nil];
        return;
    }
    BOOL isDirectory = NO;
    if (![[NSFileManager defaultManager] fileExistsAtPath:_StrC2OC(imageFilePath) isDirectory:&isDirectory]) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件不存在或不能访问" data:nil];
        return;
    }
    if (isDirectory) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"文件路径错误,指向的是文件夹" data:nil];
        return;
    }
    if ([[[NSFileManager defaultManager] attributesOfItemAtPath:_StrC2OC(imageFilePath) error:nil] fileSize]>32*1024) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"分享网页链接的图片文件不能大于32KB" data:nil];
        return;
    }
    CMWebPageShareRequest * request = [CMWebPageShareRequest RequestWithImageFilePath:_StrC2OC(imageFilePath) Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString: _StrC2OC(url)]];
    [[CMOpenSDK share]sendRequest:request];
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
void cmopensdk_requestShareWebLinkWithImageFileData(__CString title,__CString text,__CString url,const void * fileData,const int lenth){
    if (lenth>32*1024) {
        [CMOpenSDKUnityTool sendToUnityCode:CMOpenSDKErrorCodeParematersError Message:@"图片不能大于32KB" data:nil];
        return;
    }
    if (fileData==NULL||lenth==0) {
        CMWebPageShareRequest * request = [CMWebPageShareRequest RequestWithImageData:nil Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
        [[CMOpenSDK share] sendRequest:request];
    }else{
    CMWebPageShareRequest * request = [CMWebPageShareRequest RequestWithImageData:[NSData dataWithBytes:fileData length:lenth] Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
        [[CMOpenSDK share] sendRequest:request];

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
void cmopensdk_requestShareWebLinkWithImageUrl(__CString title,__CString text,__CString url,__CString imageUrl){
    CMWebPageShareRequest * request = [CMWebPageShareRequest RequestWithWebImageUrl:_StrC2OC(imageUrl) Title:_StrC2OC(title) Text:_StrC2OC(text) linkUrl:[NSURL URLWithString:_StrC2OC(url)]];
    [[CMOpenSDK share] sendRequest:request];
}

/**
 检测是否安装了游戏互联

 @return 是否安装了游戏互联
 */
extern BOOL cmopensdk_isInstallYXHL(){
    return [[CMOpenSDK share]isInstallYXHL];
}


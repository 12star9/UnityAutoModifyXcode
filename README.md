

## UnityAutoModifyXcode是一个演示UnityXcodeAPI修改Xcode工程的示例工程.

**背景说明:**

Unity5.X开始针对Xcode工程提供了自动修改工程相关配置的API方法，但是官方文档说明和代码演示的不足，老版本Unity不能使用，只能借助第三方python等脚本实现这些问题，该示例工程为解决这些问题提供了一种参考方案。

**目前代码演示实现的需求有:**

1.修改工程Build Phases配置，比如添加Run Script,系统.framework和.dylib.

2.添加Embedded Framework.

4.修改Build Settings,比如签名证书，provisioning_profile,部署目标，entitlements,Other_Link_Flags.

5.修改Info.plist.

6.修改UnityAppController.mm.

7.修改Capabilities，比如PushNotification,InAppPurchase,BackgroundModes.

8.build后自动打开XCode工程.







**🙏   https://bitbucket.org/Unity-Technologies/xcodeapi/src/stable/**



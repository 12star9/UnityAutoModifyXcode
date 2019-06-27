

# UnityAutoModifyXcode是一个自动化修改Unity-iPhone.xcodeproj工程配置的解决方案.


## 背景说明:

Unity5.X开始针对Xcode工程提供了自动修改工程相关配置的API方法，但是官方文档说明和代码演示的不足，老版本Unity不能使用，只能借助第三方python等脚本实现这些问题，该示例工程为解决这些问题提供了一种参考方案。


## 环境兼容:

Unity4.x~Unity2018.3.11f1



## 目前代码演示实现的需求有:

1.修改工程Build Phases配置，比如添加Run Script,系统.framework和.dylib. （兼容任意路径）

2.添加Embedded Framework.（兼容任意路径）

4.修改Build Settings,比如签名证书，provisioning_profile,部署目标，entitlements,Other_Link_Flags.

5.修改Info.plist.

6.修改UnityAppController.mm.

7.修改Capabilities，比如PushNotification,InAppPurchase,BackgroundModes.

8.build后自动打开XCode工程.




## 如何使用？
1. 拷贝工程代码下Assets/Editor/Users路径下的文件到你Unity工程项目的对应文件夹下
2. 参考UserOnPostBuild.cs下的代码演示，进行你的自定义配置！（API方法定义在CustomXcodeAPI.cs下，你可以自行拓展和修改）



## 如何测试？

由于在Unity里打包Xcode工程耗时问题，不便于测试，这里提供了另一种测试方案，你可以用Visual Studio打开UnityAutoModifyXcode.sln,参考TestApplication项目下的Form1.cs类代码，这里附上XCode测试工程文件：https://pan.baidu.com/s/1k7zu1PD16uxm4m3UBy7yNg 提取码: vkd9， 以方便测试！



## 鸣谢 🙏  

**  https://bitbucket.org/Unity-Technologies/xcodeapi/src/stable/**



using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;



namespace Users.Custom
{

    public class UtilsPath
    {
        /*
          string path_2 = "E:/Users/star/Desktop/xcodeapi_test/Unity-Technologies-xcodeapi-fbd0cb184b43/Unity-Technologies-xcodeapi-fbd0cb184b43/test2222.png";

          string path_3 = "E:/QQMiniDL/test2222.png";

          string path_4 = "E:/Users/star/Desktop/xcodeapi_test/Unity_Project/build/ios/123/test2222.png";


          string result = GetRelativePath(path1,path_2);

          result = GetRelativePath(path1, path_3);

          result = GetRelativePath(path1, path_4);


          return;
          */
        public static string GetRelativePath(string path1, string path2)
        {
            string[] path1Array = path1.Split('/');
            string[] path2Array = path2.Split('/');
            //
            int s = path1Array.Length >= path2Array.Length ? path2Array.Length : path1Array.Length;
            //两个目录最底层的共用目录索引
            int closestRootIndex = -1;
            for (int i = 0; i < s; i++)
            {
                if (path1Array[i] == path2Array[i])
                {
                    closestRootIndex = i;
                }
                else
                {
                    break;
                }
            }
            //由path1计算 ‘../’部分
            string path1Depth = "";
            for (int i = 0; i < path1Array.Length; i++)
            {
                if (i > closestRootIndex + 1)
                {
                    path1Depth += "../";
                }
            }
            //由path2计算 ‘../’后面的目录
            string path2Depth = "";
            for (int i = closestRootIndex + 1; i < path2Array.Length; i++)
            {
                path2Depth += "/" + path2Array[i];
            }
            path2Depth = path2Depth.Substring(1);

            return path1Depth + path2Depth;
        }
    }

    public partial class XClass : System.IDisposable
    {

        private string filePath;

        public XClass(string fPath)
        {
            filePath = fPath;
            if (!System.IO.File.Exists(filePath))
            {
                
                return;
            }
        }
        public void Replace(string oldStr, string newStr, string method = "")
        {
            if (!File.Exists(filePath))
            {

                return;
            }
            bool getMethod = false;
            string[] codes = File.ReadAllLines(filePath);
            for (int i = 0; i < codes.Length; i++)
            {
                string str = codes[i].ToString();
                if (string.IsNullOrEmpty(method))
                {
                    if (str.Contains(oldStr)) codes.SetValue(newStr, i);
                }
                else
                {
                    if (!getMethod)
                    {
                        getMethod = str.Contains(method);
                    }
                    if (!getMethod) continue;
                    if (str.Contains(oldStr))
                    {
                        codes.SetValue(newStr, i);
                        break;
                    }
                }
            }
            File.WriteAllLines(filePath, codes);
        }


        public void WriteBelow(string below, string text)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1)
            {

                return;
            }

            int endIndex = text_all.LastIndexOf("\n", beginIndex + below.Length);

            text_all = text_all.Substring(0, endIndex) + "\n" + text + "\n" + text_all.Substring(endIndex);

            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }
        public void Dispose()
        {

        }
    }

    public class CustomXCodeAPI
    {

        private string pbxprojPath = "";

        private string xcodePath = "";

        private Users.Custom.PBXProject pbxProj = new Users.Custom.PBXProject();

        private string targetGuid = "";

        public void InitPbxProject(string pathToBuiltProject)
        {
            xcodePath = pathToBuiltProject;
            string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            pbxprojPath = projPath;
            pbxProj.ReadFromFile(projPath);
            targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");
        }

        public void UpdateBuildSettingsSample()
        {

            //Build settings
            pbxProj.SetBuildProperty(targetGuid, "IPHONEOS_DEPLOYMENT_TARGET", "9.0");
            pbxProj.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", "Unity-iPhone.entitlements");
            //pbxProj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            pbxProj.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
            pbxProj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
            ///Build settings SetTeamId
            pbxProj.SetTeamId(targetGuid, "73889W623Z");
            ///Build settings ProvisioningProfile
            IEnumerable<string> configNames = pbxProj.BuildConfigNames();
            foreach (var configName in configNames)
            {
                string configGuid = pbxProj.BuildConfigByName(targetGuid, configName);
                string code_sign_identity = "iPhone Developer: nanxing liao (H6KAK88X9G)";
                string provision_profile_name = "StarUnityTest_Dev_ProvisioningProfile";
                if (configName != "Debug")
                {
                    code_sign_identity = "iPhone Distribution: nanxing liao (73889W623Z)";
                    provision_profile_name = "StarUnityTest_Dis_ProvisioningProfile";
                }
                pbxProj.SetBuildPropertyForConfig(configGuid, "CODE_SIGN_STYLE", "Manual");
                pbxProj.SetBuildPropertyForConfig(configGuid, "PROVISIONING_PROFILE_SPECIFIER", provision_profile_name);
                pbxProj.SetBuildPropertyForConfig(configGuid, "CODE_SIGN_IDENTITY", code_sign_identity);
                pbxProj.SetBuildPropertyForConfig(configGuid, "CODE_SIGN_IDENTITY[sdk=iphoneos*]", code_sign_identity);
            }

        }

        public void UpdateCapabilitiesSample()
        {
            ///Build settings Capabilities
            var projCapability = new Users.Custom.ProjectCapabilityManager(pbxprojPath, "Unity-iPhone.entitlements", "Unity-iPhone");
            //bug: projCapability.AddGameCenter();
            //bug: projCapability.AddInAppPurchase();
            projCapability.AddPushNotifications(true);
            //bug:pbxProj.AddCapability (targetGuid, Users.Custom.PBXCapabilityType.GameCenter);
            pbxProj.AddCapability(targetGuid, Users.Custom.PBXCapabilityType.InAppPurchase);
            //bug: projCapability.AddAppGroups(null);
            //bug: projCapability.AddHealthKit();
            //bug: projCapability.AddiCloud(true, true, true, true, null);
            //bug:projCapability.AddKeychainSharing(new string[]{"com.star.StarUnityTest"});
            projCapability.AddBackgroundModes(Users.Custom.BackgroundModesOptions.BackgroundFetch);
            projCapability.WriteToFile();
        }

        public void UpdateSystemFrameworkSample()
        {
            //Build Phases
            pbxProj.AddFrameworkToProject(targetGuid, "CoreBluetooth.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "GLKit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "AudioToolbox.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreFoundation.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "ImageIO.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "AdSupport.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "AVFoundation.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreMedia.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "Foundation.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "Security.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "UIKit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreVideo.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CFNetwork.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "MobileCoreServices.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreData.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreMotion.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "EventKitUI.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "EventKit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "MessageUI.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "Social.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "Twitter.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreLocation.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "MediaPlayer.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "QuartzCore.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "StoreKit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "AdSupport.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "Mapkit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "Passkit.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "Webkit.framework", true);
            pbxProj.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "AVKit.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "JavaScriptCore.framework", false);
            pbxProj.AddFrameworkToProject(targetGuid, "WatchConnectivity.framework", false);
            ///Build Phases add .dylib
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libresolv.dylib", "Frameworks/libresolv.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libsqlite3.0.dylib", "Frameworks/libsqlite3.0.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libxml2.dylib", "Frameworks/libxml2.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libc++.dylib", "Frameworks/libc++.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", Users.Custom.PBXSourceTree.Sdk));
            pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libxml2.2.dylib", "Frameworks/libxml2.2.dylib", Users.Custom.PBXSourceTree.Sdk));
        }


        public void AddStaticLibraryFromCustomPath(string absolutePath)
        {

            //添加自定义路径的a
            string aReferencePath =UtilsPath.GetRelativePath(xcodePath, absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            string aReference = pbxProj.AddFile(aReferencePath, "Frameworks/" + fileName, PBXSourceTree.Group);
            pbxProj.AddFileToBuild(targetGuid, aReference);
            string path= PBXPath.GetDirectory(aReferencePath)+"/**";
            pbxProj.AddBuildProperty(targetGuid, "LIBRARY_SEARCH_PATHS", path);
            
        }

        public void AddDynamicFrameworkFromCustomPath(string absolutePath)
        {
            //添加自定义路径的EmbeddedFramework
            
            pbxProj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");

            string frameworkReferencePath =UtilsPath.GetRelativePath(xcodePath, absolutePath);
            string path = PBXPath.GetDirectory(frameworkReferencePath) + "/**";
            string fileName = Path.GetFileName(absolutePath);
            string frameworkReference = pbxProj.AddFile(frameworkReferencePath, "Frameworks/" + fileName, PBXSourceTree.Group);
            pbxProj.AddFileToBuild(targetGuid, frameworkReference);

            pbxProj.AddDynamicFrameworkToProject(targetGuid, frameworkReferencePath, "Frameworks/" + fileName);

            pbxProj.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", path);
            
        }

        public void AddFrameworkFromCustomPath(string absolutePath)
        {
            //添加自定义路径的framework
            string frameworkReferencePath =UtilsPath.GetRelativePath(xcodePath, absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            string path = PBXPath.GetDirectory(frameworkReferencePath) + "/**";
            string frameworkReference = pbxProj.AddFile(frameworkReferencePath, "Frameworks/"+fileName, PBXSourceTree.Group);
            pbxProj.AddFileToBuild(targetGuid, frameworkReference);
            pbxProj.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", path);
        }

        public void AddFolderResourceFromCustomPath(string folderAbsolutePath)
        {
            //添加文件夹到build phases
            string folderReferencePath =UtilsPath.GetRelativePath(xcodePath, folderAbsolutePath);
            string lastName = Path.GetFileName(folderAbsolutePath);
            string folderReference = pbxProj.AddFolderReference(folderReferencePath, lastName, PBXSourceTree.Group);
            pbxProj.AddFileToBuild(targetGuid, folderReference);
        }

        public void AddFileResourceFromCustomPath(string absolutePath)
        {
            //添加资源文件(.bundle,.png)到build phases
            string pngReferencePath =UtilsPath.GetRelativePath(xcodePath, absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            string pngReference = pbxProj.AddFile(pngReferencePath, fileName, PBXSourceTree.Source);
            pbxProj.AddFileToBuild(targetGuid, pngReference);
        }

        public void AddComplieFileFromCustomPath(string absolutePath,List<string> compileFlags)
        {
            //添加编译代码文件
            string compileFileReferencePath =UtilsPath. GetRelativePath(xcodePath, absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            string compileFileReference = pbxProj.AddFile(compileFileReferencePath, fileName, PBXSourceTree.Group);
            pbxProj.AddFileToBuild(targetGuid, compileFileReference);
            /*
            List<string> list = new List<string>();
            list.Add("-fno-objc-arc");
             * */
            pbxProj.SetCompileFlagsForFile(targetGuid, compileFileReference, compileFlags);
        }

        public void AddShellScriptFromCustomPath(string absolutePath)
        {
            //添加shell-script
            string shellPath = UtilsPath.GetRelativePath(xcodePath, absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            pbxProj.AppendShellScriptBuildPhase(targetGuid, "Run Script " + fileName, "/bin/sh", shellPath);

        }

      
        public void UpdateXcodeFile()
        {
            //completed!
            pbxProj.WriteToFile(pbxprojPath);

        }

        public void UpdateXcodeFile(string resultPath)
        {
            //completed!
            pbxProj.WriteToFile(resultPath);

        }


        private static void EditInfoPlistSample(string filePath)
        {
            string path = filePath + "/Info.plist";
            Users.Custom.PlistDocument plistDocument = new Users.Custom.PlistDocument();
            plistDocument.ReadFromFile(path);
            Users.Custom.PlistElementDict dict = plistDocument.root.AsDict();
            Users.Custom.PlistElementDict securityDic = dict.CreateDict("NSAppTransportSecurity");
            securityDic.SetBoolean("NSAllowsArbitraryLoads", true);
            dict.SetString("NSLocationAlwaysUsageDescription", "NSLocationAlwaysUsageDescription");
            dict.SetString("NSPhotoLibraryUsageDescription", "NSPhotoLibraryUsageDescription");
            dict.SetString("NSCameraUsageDescription", "NSCameraUsageDescription");
            dict.SetString("NSCalendarsUsageDescription", "NSCalendarsUsageDescription");
            plistDocument.WriteToFile(path);
        }

        private static void CopyDirectory(string srcPath, string dstPath, string[] excludeExtensions, bool overwrite = true)
        {
            if (!Directory.Exists(dstPath))
                Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath, "*.*", SearchOption.TopDirectoryOnly).Where(path => excludeExtensions == null || !excludeExtensions.Contains(Path.GetExtension(path))))
            {
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
            }

            foreach (var dir in Directory.GetDirectories(srcPath))
                CopyDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), excludeExtensions, overwrite);
        }

        private static void EditUnityAppControllerSample(string pathToBuiltProject)
        {
            //Edit UnityAppController.mm
            XClass UnityAppController = new XClass(pathToBuiltProject + "/Classes/UnityAppController.mm");
            //Refer to the header file of the third-party SDK
            UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import \"ThirdDK.h\"");
            //add code:  [[ThirdSDK sharedInstance] showSplash:@"appkey" withWindow:self.window blockid:@"blockid"]; return YES;
            string resultStr = "";
            string newCodeStr = "    [[ThirdSDK sharedInstance] showSplash:@\"{0}\" withWindow:self.window blockid:@\"{1}\"];\n\n    return YES;";
            resultStr = string.Format(newCodeStr, "appkey", "blockid");
            UnityAppController.Replace("return YES;", resultStr, "didFinishLaunchingWithOptions");
        }		


    }

	

}


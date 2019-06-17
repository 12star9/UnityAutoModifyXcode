using System;
using UnityEngine;
#if UNITY_EDITOR && UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
//using UnityEditor.iOS.Xcode;
using Users.Custom; //
#endif
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;



namespace ThirdSDKPostBuilds
{
	public class UserOnPostBuild: MonoBehaviour
	{
		#if UNITY_EDITOR && UNITY_IOS
		[PostProcessBuild( 900 )]
		static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
		{
			if (target.ToString()=="iPhone"||target.ToString()=="iOS") {
				Debug.Log("Start Xcode project related configuration of SDK......");
				Debug.Log(pathToBuiltProject);
				EditProj(pathToBuiltProject);
				Debug.Log("Complete the Xcode project configuration of the SDK！");
			}
		}
		#endif

		static void EditProj(string pathToBuiltProject)
		{
			string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
			Users.Custom.PBXProject pbxProj = new Users.Custom.PBXProject();
			pbxProj.ReadFromFile(projPath);
			string targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");

			//Build settings
			pbxProj.SetBuildProperty(targetGuid, "IPHONEOS_DEPLOYMENT_TARGET", "9.0");
			pbxProj.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", "Unity-iPhone/test.entitlements");
			//pbxProj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
			pbxProj.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
			pbxProj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
			///Build settings SetTeamId
			pbxProj.SetTeamId(targetGuid,"73889W623Z");
			///Build settings ProvisioningProfile
			IEnumerable<string> configNames=pbxProj.BuildConfigNames();
			foreach(var configName in configNames)
			{
				Debug.Log("configNames:"+configName);
				string configGuid=pbxProj.BuildConfigByName(targetGuid,configName);
				string code_sign_identity="iPhone Developer: nanxing liao (H6KAK88X9G)";
				string provision_profile_name = "StarUnityTest_Dev_ProvisioningProfile";
				if(configName!="Debug")
				{
					code_sign_identity="iPhone Distribution: nanxing liao (73889W623Z)";
					provision_profile_name="StarUnityTest_Dis_ProvisioningProfile";
				}
				pbxProj.SetBuildPropertyForConfig(configGuid,"CODE_SIGN_STYLE","Manual");
				pbxProj.SetBuildPropertyForConfig(configGuid,"PROVISIONING_PROFILE_SPECIFIER",provision_profile_name);
				pbxProj.SetBuildPropertyForConfig(configGuid,"CODE_SIGN_IDENTITY",code_sign_identity);
				pbxProj.SetBuildPropertyForConfig(configGuid,"CODE_SIGN_IDENTITY[sdk=iphoneos*]",code_sign_identity);
			}
			///Build settings Capabilities
			var projCapability = new Users.Custom.ProjectCapabilityManager(projPath, "Unity-iPhone/test.entitlements", "Unity-iPhone");
			//bug: projCapability.AddGameCenter();
			//bug: projCapability.AddInAppPurchase();
			projCapability.AddPushNotifications(true);
			//bug:pbxProj.AddCapability (targetGuid, Users.Custom.PBXCapabilityType.GameCenter);
   			pbxProj.AddCapability (targetGuid, Users.Custom.PBXCapabilityType.InAppPurchase);
			//bug: projCapability.AddAppGroups(null);
			//bug: projCapability.AddHealthKit();
            //bug: projCapability.AddiCloud(true, true, true, true, null);
			//bug:projCapability.AddKeychainSharing(new string[]{"com.star.StarUnityTest"});
			projCapability.AddBackgroundModes(Users.Custom.BackgroundModesOptions.BackgroundFetch);
            projCapability.WriteToFile();
			

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
			///Build Phases add embedded framework 
		    pbxProj.SetBuildProperty (targetGuid,"LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
			string defaultLocationInProj = "Frameworks/Plugins/iOS/EmbeddedFramework/";
			string relativeCoreFrameworkPath = "";
			string[] commonFrameworkNames=new string[]{"EmbeddedFramework1","EmbeddedFramework2"};
			foreach (var frameworkNameTemp in commonFrameworkNames) {
				string frameworkName = frameworkNameTemp+".framework";
				 relativeCoreFrameworkPath = Path.Combine(defaultLocationInProj, frameworkName);
				AddDynamicFrameworks (ref pbxProj,targetGuid,relativeCoreFrameworkPath);
			}
			///Build Phases add run script
			pbxProj.AppendShellScriptBuildPhase(targetGuid,"Run Script copy_test.sh","/bin/sh","./../Assets/Editor/copy_test.sh");
			Debug.Log("copy_test completed!");
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
            //completed!
			pbxProj.WriteToFile(projPath);
            Debug.Log(" OnPostProcessBuild success!");
		}

        static void EditInfoPlist(string filePath)
		{
			string path = filePath + "/Info.plist";
			Users.Custom.PlistDocument plistDocument = new Users.Custom.PlistDocument();
			plistDocument.ReadFromFile(path);
			Users.Custom.PlistElementDict dict = plistDocument.root.AsDict();
			Users.Custom.PlistElementDict securityDic = dict.CreateDict("NSAppTransportSecurity");
			securityDic.SetBoolean ("NSAllowsArbitraryLoads",true);
			dict.SetString ("NSLocationAlwaysUsageDescription", "NSLocationAlwaysUsageDescription");
			dict.SetString ("NSPhotoLibraryUsageDescription", "NSPhotoLibraryUsageDescription");
			dict.SetString ("NSCameraUsageDescription", "NSCameraUsageDescription");
			dict.SetString ("NSCalendarsUsageDescription", "NSCalendarsUsageDescription");
			plistDocument.WriteToFile(path);
		}

		public static void CopyDirectory(string srcPath, string dstPath, string[] excludeExtensions, bool overwrite = true)
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

		static void EditUnityAppController(string pathToBuiltProject)
		{
			//Edit UnityAppController.mm
			XClass UnityAppController = new XClass(pathToBuiltProject + "/Classes/UnityAppController.mm");
			//Refer to the header file of the third-party SDK
			UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import \"ThirdDK.h\"");
			//add code:  [[ThirdSDK sharedInstance] showSplash:@"appkey" withWindow:self.window blockid:@"blockid"]; return YES;
			string resultStr = "";
 			string newCodeStr = "    [[ThirdSDK sharedInstance] showSplash:@\"{0}\" withWindow:self.window blockid:@\"{1}\"];\n\n    return YES;";
 			resultStr = string.Format (newCodeStr,"appkey","blockid");
			UnityAppController.Replace ("return YES;",resultStr,"didFinishLaunchingWithOptions");
		}

		static void AddDynamicFrameworks(ref Users.Custom.PBXProject project, string target,string embeddedFrameworkRelativePath)
		{
			string relativeCoreFrameworkPath = embeddedFrameworkRelativePath;
			project.AddDynamicFrameworkToProject(target, relativeCoreFrameworkPath);
			Debug.Log("Dynamic Frameworks added to Embedded binaries.");
		}
		

	public partial class XClass : System.IDisposable
	{

		private string filePath;

		public XClass(string fPath)
		{
			filePath = fPath;
			if( !System.IO.File.Exists( filePath ) ) {
				Debug.LogError( filePath +"The file does not exist under the path!" );
				return;
			}
		}
		public void Replace(string oldStr,string newStr,string method="")  
		{  
			if (!File.Exists (filePath))   
			{  
				 
				return;  
			}  
			bool getMethod = false;  
			string[] codes = File.ReadAllLines (filePath);  
			for (int i=0; i<codes.Length; i++)   
			{  
				string str=codes[i].ToString();  
				if(string.IsNullOrEmpty(method))  
				{  
					if(str.Contains(oldStr))codes.SetValue(newStr,i);  
				}  
				else  
				{  
					if(!getMethod)  
					{  
						getMethod=str.Contains(method);  
					}  
					if(!getMethod)continue;  
					if(str.Contains(oldStr))  
					{  
						codes.SetValue(newStr,i);  
						break;  
					}  
				}  
			}  
			File.WriteAllLines (filePath, codes);  
		}  


		public void WriteBelow(string below, string text)
		{
			StreamReader streamReader = new StreamReader(filePath);
			string text_all = streamReader.ReadToEnd();
			streamReader.Close();

			int beginIndex = text_all.IndexOf(below);
			if(beginIndex == -1){
				
				return; 
			}

			int endIndex = text_all.LastIndexOf("\n", beginIndex + below.Length);

			text_all = text_all.Substring(0, endIndex) + "\n"+text+"\n" + text_all.Substring(endIndex);

			StreamWriter streamWriter = new StreamWriter(filePath);
			streamWriter.Write(text_all);
			streamWriter.Close();
		}
		public void Dispose()
		{

		}
	}
	
  }

}


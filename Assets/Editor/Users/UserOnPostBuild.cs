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
using System.Diagnostics;


// using System;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.Callbacks;
// using System.Collections.Generic;
// using System.Xml;
// using System.Linq;
// using System.IO;
// using System.Diagnostics;


namespace ThirdSDKPostBuilds
{
	public class UserOnPostBuild: MonoBehaviour
	{
        //Xcode工程的build全路径
        private string xcodePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/Build/iOS/";
        //Unity-iPhone.xcodeproj/project.pbxproj的全路径
        private string pbxProjectPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/Build/iOS/Unity-iPhone.xcodeproj/project.pbxproj";
        //需要添加到Build Phases/Copy Bundle Resources中的文件夹的全路径
        private string folderResourcePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/bundle";
        //需要添加到Build Phases/Link Binary With Libraries中的框架的全路径
        private string frameworkPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/EmbeddedFramework1.framework";
        //需要添加到Build Phases/Embed Frameworks中的框架的全路径
        private string dynamicFrameworkPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/EmbeddedFramework/EmbeddedFramework2.framework";
        //需要添加到Build Phases/Compile Sources中的代码文件的全路径
        private string compileFilePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/CDataScanner.m";
        //需要添加到Build Phases/Run Script中的脚本文件的全路径
        private string shellFilePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/copy_test.sh";
        //需要添加到Build Phases/Link Binary With Libraries中的静态库的全路径
        private string staticLibraryPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/test.a";
        //需要添加到Build Phases/Copy Bundle Resources中的资源图片的全路径
        private string fileResourcePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/test.png";

        private CustomXCodeAPI xcodeInstance = new CustomXCodeAPI();
		#if UNITY_EDITOR && UNITY_IOS
		[PostProcessBuild( 900 )]
		static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
		{
			if (target.ToString()=="iPhone"||target.ToString()=="iOS") {
				UnityEngine.Debug.Log("Start Xcode project related configuration of SDK......");
				UnityEngine.Debug.Log(pathToBuiltProject);
				EditProj(pathToBuiltProject);
				UnityEngine.Debug.Log("Complete the Xcode project configuration of the SDK！");
			}
		}
		#endif

		static void EditProj(string pathToBuiltProject)
		{

            xcodeInstance.InitPbxProject(pathToBuiltProject);

            xcodeInstance.UpdateBuildSettingsSample();
            xcodeInstance.UpdateCapabilitiesSample();
            xcodeInstance.UpdateSystemFrameworkSample();
            List<string> list = new List<string>();
            list.Add("-fno-objc-arc");
            xcodeInstance.AddComplieFileFromCustomPath(compileFilePath, list);
            xcodeInstance.AddDynamicFrameworkFromCustomPath(dynamicFrameworkPath);
            xcodeInstance.AddFileResourceFromCustomPath(fileResourcePath);
            xcodeInstance.AddFolderResourceFromCustomPath(folderResourcePath);
            xcodeInstance.AddFrameworkFromCustomPath(frameworkPath);
            xcodeInstance.AddShellScriptFromCustomPath(shellFilePath);
            xcodeInstance.AddStaticLibraryFromCustomPath(staticLibraryPath);

            xcodeInstance.UpdateXcodeFile();

			var scriptPath = Path.Combine( Application.dataPath, "Editor/OpenXcode.py" );
			// sanity check
			
			UnityEngine.Debug.Log(" scriptPath: "+scriptPath);
			if( !File.Exists( scriptPath ) ) {
				UnityEngine.Debug.LogError("OpenXcode.py not exist!");
				return;
			} else {
				var args = string.Format( "\"{0}\" \"{1}\"", scriptPath,pathToBuiltProject+"/Unity-iPhone.xcodeproj" );
				var proc = new System.Diagnostics.Process
				{
					StartInfo = new System.Diagnostics.ProcessStartInfo
					{
						FileName = "python",
						Arguments = args,
						UseShellExecute = false,
						RedirectStandardOutput = false,
						CreateNoWindow = true
					}
				};
				proc.Start();
				proc.WaitForExit();
				if (proc.ExitCode > 0) {
					UnityEngine.Debug.LogError(" post-build OpenXcode.py script had an error(code=" + proc.ExitCode);
				}
			}
            UnityEngine.Debug.Log(" OnPostProcessBuild success!");
		}
  }

}


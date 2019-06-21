using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Users.Custom;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        //这里测试输出的project_output.pbxproj文件覆盖到Xcode源工程文件，必须保证Xcode工程文件跟其他文件的相对路径跟下面定义的相对路径是一样的!!!!
        private string xcodePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/Build/iOS/";
        private string pbxProjectPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/Build/iOS/Unity-iPhone.xcodeproj/project.pbxproj";
        private string output_pbxProjectPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/Build/iOS/Unity-iPhone.xcodeproj/project_output.pbxproj";
        private string folderResourcePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/bundle";
        private string frameworkPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/EmbeddedFramework1.framework";
        private string dynamicFrameworkPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/EmbeddedFramework/EmbeddedFramework2.framework";
        private string compileFilePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/CDataScanner.m";
        private string shellFilePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/copy_test.sh";
        private string staticLibraryPath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/test.a";
        private string fileResourcePath = "E:/Users/star/Desktop/Git/UnityAutoModifyXcode/SDK_FILES/test.png";
        private CustomXCodeAPI xcodeInstance = new CustomXCodeAPI();

        public Form1()
        {
            InitializeComponent();
            this.testAPI();
        }

        private void testAPI()
        {

            xcodeInstance.InitPbxProject(xcodePath);

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


            xcodeInstance.UpdateXcodeFile(output_pbxProjectPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            


        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReFreeAdobe.src
{
    public class PatchInfo
    {
        private AdobeProduct product;
        private string productName;
        private string version;
        private string relInstallPath;
        private string relLaunchPath;
        private string fileName;
        private string targetByteStr;
        private string newByteStr;
        
        

        public AdobeProduct Product { get => product; set => product = value; }

        public string ProductName { get => productName; set => productName = value; }

        public string Version { get => version; set => version = value; }
        public string RelativeInstallPath { get => relInstallPath; set => relInstallPath = value; }


        public string FileName { get => fileName; set => fileName = value; }
        public string TargetByteStr { get => targetByteStr; set => targetByteStr = value; }
        public string NewByteStr { get => newByteStr; set => newByteStr = value; }
        public string RelativeLaunchPath { get => relLaunchPath; set => relLaunchPath = value; }

        public PatchInfo()
        {
        }

        public PatchInfo(AdobeProduct product,string productName, string version, string relInstallPath, string relLaunchPath, string fileName, string targetByteStr, string newByteStr)
        {
            this.Product = product;
            this.productName = productName;
            this.Version = version;
            this.relInstallPath = relInstallPath;
            this.relLaunchPath = relLaunchPath;
            this.FileName = fileName;
            this.TargetByteStr = targetByteStr;
            this.NewByteStr = newByteStr;
        }
    }
}

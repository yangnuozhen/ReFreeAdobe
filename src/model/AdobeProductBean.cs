using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReFreeAdobe.src.model
{
    public class AdobeProductBean
    {
        private AdobeProduct product;
        private string version;
        private string icon;
        private string name;
        private string desc;
        private string detail;
        private string relLaunchPath;

        public AdobeProductBean()
        {
        }

        public AdobeProductBean(AdobeProduct product, string version, string icon, string name, string desc, string detail,string relLaunchPath)
        {
            this.product = product;
            this.version = version;
            this.icon = icon;
            this.name = name;
            this.desc = desc;
            this.detail = detail;
            this.relLaunchPath = relLaunchPath;

            if(Name == "Photoshop 2023")
            {
                this.desc = "需要禁止联网";
            }
        }

        public AdobeProduct Product { get => product; set => product = value; }
        public string Version { get => version; set => version = value; }
        public string Icon { get => icon; set => icon = value; }
        public string Name { get => name; set => name = value; }
        public string Desc { get => desc; set => desc = value; }
        public string Detail { get => detail; set => detail = value; }
        public string LaunchPath
        {
            get
            {
                return AdobePatchUtil.AdobeInstallPath + "/" + relLaunchPath;
            }
        }
    }
}

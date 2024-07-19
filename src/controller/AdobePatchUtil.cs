using ReFreeAdobe.src.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ReFreeAdobe.src;

namespace ReFreeAdobe.src
{
    public class AdobePatchUtil
    {
        //2019\2020\2021系列通用特征码
        private static string targetByteStr= "BB 02 00 00 00 8B C3 87 05";
        private static string newByteStr = "BB 03 00 00 00 8B C3 87 05";

        private static string acrobatTargetByte = "33 C0 40 89 85 ?? ?? ?? ?? 8D 85";
        private static string acrobatNewByte = "40 40 40 89 85 ?? ?? ?? ?? 8D 85";

        public static readonly string AdobeDefaultPath = "C:/Program Files/Adobe";

        public static string AdobeInstallPath
        {
            get
            {
                if (Properties.Settings.Default.installPath.Length != 0)
                    return Properties.Settings.Default.installPath;
                return AdobeDefaultPath;
            }
            set
            {
                Properties.Settings.Default.installPath = value;
                Properties.Settings.Default.Save();
            }
        }

        private static List<PatchInfo> patchInfos;

        public static List<PatchInfo> loadAllProduct() {
            if (patchInfos == null)
            {
                patchInfos = new List<PatchInfo>();
                initPatchInfo("2024");
                initPatchInfo("2023");
                initPatchInfo("2022");
                initPatchInfo("2021");
                initPatchInfo("2020");
                initPatchInfo("2019");
            }
            return patchInfos;
        }


        public static List<PatchInfo> loadProductPatchInfo(AdobeProduct productName,string version) {
            if (patchInfos == null) {
                patchInfos = new List<PatchInfo>();
                initPatchInfo("2024");
                initPatchInfo("2023");
                initPatchInfo("2022");
                initPatchInfo("2021");
                initPatchInfo("2020");
                initPatchInfo("2019");
            }
            List<PatchInfo> productPatchInfo = new List<PatchInfo>();
            foreach(PatchInfo patchInfo in patchInfos) {
                if (patchInfo.Product == productName&&patchInfo.Version.Equals(version)) {
                    productPatchInfo.Add(patchInfo);
                }
            }
            return productPatchInfo;
        }

        public static void initPatchInfo(string version) {
            
            patchInfos.Add(new PatchInfo(AdobeProduct.AfterEffects, "AfterEffects", version, "Adobe After Effects "+version+"/Support Files/AfterFXLib.dll", "Adobe After Effects " + version + "/Support Files/AfterFX.exe", "AfterFXLib.dll", targetByteStr,newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Animate, "Animate",version, "Adobe Animate " + version + "/Animate.exe", "Adobe Animate " + version + "/Animate.exe", "Animate.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Audition, "Audition", version, "Adobe Audition " + version + "/AuUI.dll", "Adobe Audition " + version + "/Adobe Audition.exe", "AuUI.dll", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Bridge, "Bridge", version, "Adobe Bridge " + version + "/Bridge.exe", "Adobe Bridge " + version + "/Bridge.exe", "Bridge.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.CharacterAnimator, "CharacterAnimator", version, "Adobe Character Animator " + version + "/Support Files/Character Animator.exe", "Adobe Character Animator " + version + "/Support Files/Character Animator.exe", "Character Animator.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Dreamweaver, "Dreamweaver", "2021", "Adobe Dreamweaver " + "2021" + "/Dreamweaver.exe", "Adobe Dreamweaver " + "2021" + "/Dreamweaver.exe", "Dreamweaver.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Illustrator, "Illustrator", version, "Adobe Illustrator " + version + "/Support Files/Contents/Windows/Illustrator.exe", "Adobe Illustrator " + version + "/Support Files/Contents/Windows/Illustrator.exe", "Illustrator.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.LightroomClassic, "LightroomClassic", version, "Adobe Lightroom Classic/Lightroom.exe", "Adobe Lightroom Classic/Lightroom.exe", "Lightroom.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.MediaEncoder, "MediaEncoder", version, "Adobe Media Encoder " + version + "/Adobe Media Encoder.exe", "Adobe Media Encoder " + version + "/Adobe Media Encoder.exe", "Character Animator.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Photoshop, "Photoshop", version, "Adobe Photoshop " + version + "/Photoshop.exe", "Adobe Photoshop " + version + "/Photoshop.exe", "Photoshop.exe", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.Prelude, "Prelude", version, "Adobe Prelude " + version + "/Registration.dll", "Adobe Prelude " + version + "/Adobe Prelude.exe", "Registration.dll", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.PremierePro, "PremierePro", version, "Adobe Premiere Pro " + version + "/Registration.dll", "Adobe Premiere Pro " + version + "/Adobe Premiere Pro.exe", "Registration.dll", targetByteStr, newByteStr));

            patchInfos.Add(new PatchInfo(AdobeProduct.InCopy, "InCopy", version, "Adobe InCopy "+version+"/Public.dll", "Adobe InCopy " + version + "/InCopy.exe", "Public.dll", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.InDesign, "InDesign", version, "Adobe InDesign " + version + "/Public.dll", "Adobe InDesign " + version + "/InDesign.exe", "Public.dll", targetByteStr, newByteStr));
            patchInfos.Add(new PatchInfo(AdobeProduct.AcrobatDC, "AcrobatDC", version, "Acrobat DC/Acrobat/Acrobat.dll", "Acrobat DC/Acrobat/Acrobat.exe", "Acrobat.dll", acrobatTargetByte, acrobatNewByte));
        }

        public static bool patchProduct(AdobeProduct product, string version) {
            List<PatchInfo> productPatchInfo = loadProductPatchInfo(product,version);
            bool succ = true ;
            foreach (PatchInfo info in productPatchInfo) {
                string installPath = AdobeInstallPath + "/" + info.RelativeInstallPath;
                //succ&=ByteHelper.replaceByte(info.InstallPath,info.TargetByteStr,info.NewByteStr);
                succ &= StrongByteHelper.replaceByte(installPath, info.TargetByteStr, info.NewByteStr);
                
                }
                return succ;
        }
    }
}

/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：ImageConvertCDR.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/01/23 23:05:43 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/01/23 23:05:43 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using Aspose.Imaging;
using Aspose.Imaging.ImageOptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="ImageConvertCDR" langword="" />
    /// </summary>
    public class ImageConvertCDR
    {
        public static Dictionary<string, string> CDR2JPEG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                using (Aspose.Imaging.FileFormats.Cdr.CdrImage image = (Aspose.Imaging.FileFormats.Cdr.CdrImage)Image.Load(originImagePath))
                {
                    JpegOptions options = new Aspose.Imaging.ImageOptions.JpegOptions();
                    options.VectorRasterizationOptions = (Aspose.Imaging.ImageOptions.VectorRasterizationOptions)image.GetDefaultOptions(new object[] { Color.White, image.Width, image.Height });
                    options.VectorRasterizationOptions.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    options.VectorRasterizationOptions.SmoothingMode = SmoothingMode.None;
                    image.Save(targetImagePath, options);

                    keyValuePairs.Add("resolution", image.Size.Width.ToString() + "*" + image.Size.Height.ToString());
                }
                Console.WriteLine("YPN....CDR2PNG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("YPN....CDR2PNG....失败！耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
                return keyValuePairs;
            }
            return keyValuePairs;
        }

        public static Dictionary<string, string> CDR2PNG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                using (Aspose.Imaging.FileFormats.Cdr.CdrImage image = (Aspose.Imaging.FileFormats.Cdr.CdrImage)Image.Load(originImagePath))
                {
                    PngOptions options = new Aspose.Imaging.ImageOptions.PngOptions();
                    options.ColorType = Aspose.Imaging.FileFormats.Png.PngColorType.TruecolorWithAlpha;
                    options.VectorRasterizationOptions = (Aspose.Imaging.ImageOptions.VectorRasterizationOptions)image.GetDefaultOptions(new object[] { Color.White, image.Width, image.Height });
                    options.VectorRasterizationOptions.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    options.VectorRasterizationOptions.SmoothingMode = SmoothingMode.None;
                    image.Save(targetImagePath, options);

                    keyValuePairs.Add("size", image.Size.Width.ToString() + " X " + image.Size.Height.ToString());
                }
                Console.WriteLine("YPN....CDR2PNG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("YPN....CDR2PNG....失败！耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
                throw e;
            }
            return keyValuePairs;
        }

        public static void CDR2PSD(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Console.WriteLine("Running CDR2PSD");
            string inputFileName = originImagePath;

            using (Aspose.Imaging.FileFormats.Cdr.CdrImage image = (Aspose.Imaging.FileFormats.Cdr.CdrImage)Image.Load(inputFileName))
            {
                ImageOptionsBase options = new Aspose.Imaging.ImageOptions.PsdOptions();

                // By default if image is multipage image all pages exported
                options.MultiPageOptions = new Aspose.Imaging.ImageOptions.MultiPageOptions();

                // Optional parameter that indicates to export multipage image as one
                // layer (page) otherwise it will be exported page to page
                options.MultiPageOptions.MergeLayers = true;

                // Set rasterization options for fileformat
                options.VectorRasterizationOptions = (Aspose.Imaging.ImageOptions.VectorRasterizationOptions)image.GetDefaultOptions(new object[] { Color.White, image.Width, image.Height });
                options.VectorRasterizationOptions.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                options.VectorRasterizationOptions.SmoothingMode = SmoothingMode.None;

                image.Save(targetImagePath, options);
            }
            Console.WriteLine("Finished CDR2PSD");
            Console.WriteLine("....CDR2PSD，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
        }

        public static bool IsCDR(string originImagePath)
        {
            FileFormat expectedFileFormat = FileFormat.Cdr;
            using (Image image = Image.Load(originImagePath))
            {
                if (expectedFileFormat != image.FileFormat)
                {
                    return false;
                }
            }
            return true;
        }

        public static Dictionary<string, string> TIF2JPEG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                using (Image image = Image.Load(originImagePath))
                {
                    JpegOptions options = new Aspose.Imaging.ImageOptions.JpegOptions();
                    options.VectorRasterizationOptions = (Aspose.Imaging.ImageOptions.VectorRasterizationOptions)image.GetDefaultOptions(new object[] { Color.White, image.Width, image.Height });
                    options.VectorRasterizationOptions.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    options.VectorRasterizationOptions.SmoothingMode = SmoothingMode.None;
                    image.Save(targetImagePath, options);

                    keyValuePairs.Add("Size", image.Size.Width.ToString() + " X " + image.Size.Height.ToString());
                }
                Console.WriteLine("YPN....CDR2PNG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("YPN....CDR2PNG....失败！耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
                return keyValuePairs;
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 获取TIF图像的层数
        /// YPN 2021-02-05
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string GetTIFLayer(string imagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            int layer = 0;
            try
            {
                Aspose.Imaging.FileFormats.Tiff.TiffImage image = (Aspose.Imaging.FileFormats.Tiff.TiffImage)Aspose.Imaging.Image.Load(imagePath);
                layer = image.ExifData.ExifTags.Count();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("YPN....GetTIFLayer....失败！耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
            Console.WriteLine("YPN....GetTIFLayer....{0}，耗时 {1} 秒", layer, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return layer.ToString();
        }
    }
}

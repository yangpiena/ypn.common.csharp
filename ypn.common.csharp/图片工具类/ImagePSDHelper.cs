/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：ImageConvertPSD.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/01/23 22:48:29 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/01/23 22:48:29 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.PSD;
using Aspose.PSD.ImageOptions;
using Aspose.PSD.FileFormats.Png;
using Aspose.PSD.FileFormats.Ai;
using System.IO;

/**
 * https://github.com/aspose-psd/Aspose.PSD-for-.NET
 * https://github.com/aspose-imaging/Aspose.Imaging-for-.NET
 * */
namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="ImageConvertPSD" langword="" />
    /// </summary>
    public class ImageConvertPSD
    {
        /// <summary>
        /// 使用Aspose.PSD转换
        /// </summary>
        /// <param name="originImagePath"></param>
        /// <param name="targetImagePath"></param>
        /// <param name="fileInfos"></param>
        public static Dictionary<string, string> PSD2PNG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            // implement correct Crop method for PSD files.
            using (RasterImage image = Aspose.PSD.Image.Load(originImagePath) as RasterImage)
            {
                keyValuePairs.Add("层数", image.BitsPerPixel.ToString());
                Console.WriteLine(image.TransparentColor);
                keyValuePairs.Add("分辨率", image.HorizontalResolution.ToString() + "DPI");
                keyValuePairs.Add("图像尺寸", image.Size.Width.ToString() + " X " + image.Size.Height.ToString());

                //image.Crop(new Aspose.PSD.Rectangle(10, 30, 100, 100));
                image.Save(targetImagePath, new PngOptions() { ColorType = PngColorType.TruecolorWithAlpha });
            }

            Console.WriteLine("YPN....PSD2PNG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return keyValuePairs;
        }

        /// <summary>
        /// 使用SimplePsd.dll转换
        /// </summary>
        /// <param name="i_sourceFilePath"></param>
        /// <param name="i_targetFilePath"></param>
        /// <returns></returns>
        public static string PSD2PNG2(string i_sourceFilePath, string i_targetFilePath)
        {
            SimplePsd.CPSD psd = new SimplePsd.CPSD();
            string v_result = "";
            int nResult = psd.Load(i_sourceFilePath);
            if (nResult == 0)
            {
                int nCompression = psd.GetCompression();
                string strCompression = "Unknown";
                switch (nCompression)
                {
                    case 0:
                        strCompression = "Raw data";
                        break;
                    case 1:
                        strCompression = "RLE";
                        break;
                    case 2:
                        strCompression = "ZIP without prediction";
                        break;
                    case 3:
                        strCompression = "ZIP with prediction";
                        break;
                }
                v_result = string.Format("Image Width: {0}px\r\nImage Height: {1}px\r\n" +
                    "Image BitsPerPixel: {2}\r\n" +
                    "Resolution (pixels/inch): X={3} Y={4}\r\n",
                    psd.GetWidth(),
                    psd.GetHeight(),
                    psd.GetBitsPerPixel(),
                    psd.GetXResolution(),
                    psd.GetYResolution());
                v_result += "Compression: " + strCompression;
                //pictureBox1.Image = System.Drawing.Image.FromHbitmap(psd.GetHBitmap());
                System.Drawing.Image.FromHbitmap(psd.GetHBitmap()).Save(i_targetFilePath);
            }
            else if (nResult == -1)
                v_result = "Cannot open the File";
            else if (nResult == -2)
                v_result = "Invalid (or unknown) File Header";
            else if (nResult == -3)
                v_result = "Invalid (or unknown) ColourMode Data block";
            else if (nResult == -4)
                v_result = "Invalid (or unknown) Image Resource block";
            else if (nResult == -5)
                v_result = "Invalid (or unknown) Layer and Mask Information section";
            else if (nResult == -6)
                v_result = "Invalid (or unknown) Image Data block";

            return v_result;
        }

        /// <summary>
        /// 获取PSD图像的层数
        /// YPN 2021-02-05
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string GetPSDLayer(string imagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            int layer = 0;
            try
            {
                using (RasterImage image = Aspose.PSD.Image.Load(imagePath) as RasterImage)
                {
                    foreach (var item in ((Aspose.PSD.FileFormats.Psd.PsdImage)image).Layers)
                    {
                        if (item.GetType() == typeof(Aspose.PSD.FileFormats.Psd.Layers.Layer))
                        {
                            layer++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("YPN....GetPSDLayer....失败！，耗时 {1} 秒", layer, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
            Console.WriteLine("YPN....GetPSDLayer....{0}，耗时 {1} 秒", layer, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return layer.ToString();
        }

        public static Dictionary<string, string> AI2JPEG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            using (AiImage image = (AiImage)Image.Load(originImagePath))
            {
                ImageOptionsBase options = new JpegOptions();
                image.Save(targetImagePath, options);

                keyValuePairs.Add("resolution", image.Size.Width.ToString() + "*" + image.Size.Height.ToString());
            }
            Console.WriteLine("ypn....AI2JPEG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return keyValuePairs;
        }

        public static Dictionary<string, string> AI2PNG(string originImagePath, string targetImagePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            using (AiImage image = (AiImage)Image.Load(originImagePath))
            {
                ImageOptionsBase options = new PngOptions() { ColorType = PngColorType.TruecolorWithAlpha };
                image.Save(targetImagePath, options);

                keyValuePairs.Add("size", image.Size.Width.ToString() + " X " + image.Size.Height.ToString());
            }
            Console.WriteLine("YPN....AI2PNG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return keyValuePairs;
        }

        public static void AI2PSD(string originImagePath, string targetImagePath)
        {
            //new Aspose.PSD.License().SetLicense(new MemoryStream(Convert.FromBase64String("PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5TdXpob3UgQXVuYm94IFNvZnR3YXJlIENvLiwgTHRkLjwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPnNhbGVzQGF1bnRlYy5jb208L0VtYWlsVG8+CiAgICA8TGljZW5zZVR5cGU+RGV2ZWxvcGVyIE9FTTwvTGljZW5zZVR5cGU+CiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1pdGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+CiAgICA8T3JkZXJJRD4xOTA4MjYwODA3NTM8L09yZGVySUQ+CiAgICA8VXNlcklEPjEzNDk3NjAwNjwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+M2U0NGRlMzAtZmNkMi00MTA2LWIzNWQtNDZjNmEzNzE1ZmMyPC9TZXJpYWxOdW1iZXI+CiAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwMjAwODI3PC9TdWJzY3JpcHRpb25FeHBpcnk+CiAgICA8TGljZW5zZVZlcnNpb24+My4wPC9MaWNlbnNlVmVyc2lvbj4KICAgIDxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuYXNwb3NlLmNvbS9wb2xpY2llcy91c2UtbGljZW5zZTwvTGljZW5zZUluc3RydWN0aW9ucz4KICA8L0RhdGE+CiAgPFNpZ25hdHVyZT53UGJtNUt3ZTYvRFZXWFNIY1o4d2FiVEFQQXlSR0pEOGI3L00zVkV4YWZpQnd5U2h3YWtrNGI5N2c2eGtnTjhtbUFGY3J0c0cwd1ZDcnp6MytVYk9iQjRYUndTZWxsTFdXeXNDL0haTDNpN01SMC9jZUFxaVZFOU0rWndOQkR4RnlRbE9uYTFQajhQMzhzR1grQ3ZsemJLZFZPZXk1S3A2dDN5c0dqYWtaL1E9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4=")));
            DateTime v_StartDT = DateTime.Now;
            Console.WriteLine("Running AI2PSD");
            String sourceFileName = originImagePath;
            String outFileName = targetImagePath;

            using (AiImage image = (AiImage)Image.Load(sourceFileName))
            {

                ImageOptionsBase options = new PsdOptions();
                image.Save(outFileName, options);

            }
            Console.WriteLine("Finished AI2PSD");
            Console.WriteLine("....AI2PSD，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
        }
    }
}

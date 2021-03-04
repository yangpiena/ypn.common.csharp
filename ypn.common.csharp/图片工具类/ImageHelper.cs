using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;
using System.Collections.Generic;

namespace ypn.common.csharp
{
    /// <summary>   
    /// 图片工具类   
    /// </summary>   
    public class ImageHelper
    {
        private static bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>   
        /// 生成缩略图重载方法1，返回缩略图的Image对象   
        /// </summary>   
        /// <param name="Width">缩略图的宽度</param>   
        /// <param name="Height">缩略图的高度</param>   
        /// <returns>缩略图的Image对象</returns>   
        public static Image GetThumbImage(Image i_image, int i_width, int i_height)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ReducedImage = i_image.GetThumbnailImage(i_width, i_height, callb, IntPtr.Zero);
                return ReducedImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>   
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// </summary>   
        /// <param name="Width">缩略图的宽度</param>   
        /// <param name="Height">缩略图的高度</param>   
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>   
        /// <returns>成功返回true，否则返回false</returns>   
        public static bool GetThumbImage(Image i_image, int i_width, int i_height, string i_targetFilePath)
        {
            try
            {
                DateTime v_StartDT = DateTime.Now;
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ReducedImage = i_image.GetThumbnailImage(i_width, i_height, callb, IntPtr.Zero);
                ReducedImage.Save(@i_targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                Console.WriteLine("....GetThumbImage，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>   
        /// 生成缩略图重载方法3，返回缩略图的Image对象   
        /// </summary>   
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>     
        /// <returns>缩略图的Image对象</returns>   
        public static Image GetThumbImage(Image i_image, double i_percent)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int imageWidth = Convert.ToInt32(i_image.Width * i_percent);
                int imageHeight = Convert.ToInt32(i_image.Width * i_percent);
                ReducedImage = i_image.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);
                return ReducedImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>   
        /// 生成缩略图重载方法4，将缩略图文件保存到指定的路径，返回结果
        /// </summary>   
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>     
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>   
        /// <returns>成功返回true,否则返回false</returns>   
        public static bool GetThumbImage(Image i_image, double i_percent, string i_targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int imageWidth = Convert.ToInt32(i_image.Width * i_percent);
                int imageHeight = Convert.ToInt32(i_image.Width * i_percent);
                ReducedImage = i_image.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);
                ReducedImage.Save(@i_targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图,可缩放\可裁剪
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static void GetThumbImage(Image i_image, int i_width, int i_height, string i_mode, string i_targetFilePath)
        {
            Image originalImage = i_image;
            int towidth = i_width;
            int toheight = i_height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (i_mode)
            {
                case "HW":  //指定高宽缩放（可能变形）                
                    break;
                case "W":   //指定宽，高按比例
                    toheight = originalImage.Height * i_width / originalImage.Width;
                    break;
                case "H":   //指定高，宽按比例
                    towidth = originalImage.Width * i_height / originalImage.Height;
                    break;
                case "Cut": //指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * i_height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(i_targetFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// JPEG格式图片压缩
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="outPath"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool GetThumbJPEG(string sFile, string outPath, int flag)
        {
            DateTime v_StartDT = DateTime.Now;
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                Console.WriteLine("YPN....GetThumbJPEG，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            }
        }

        /// <summary>
        /// 使用MagickImage转换图像
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertImage(string sourceFilePath, string targetFilePath)
        {
            DateTime v_StartDT = DateTime.Now;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            using (MagickImage image = new MagickImage(sourceFilePath)) //获得一个图片对象
            {
                //keyValuePairs.Add("ColorSpace", image.ColorSpace.ToString());
                keyValuePairs.Add("resolution", image.Width.ToString() + "*" + image.Height.ToString());
                image.Write(targetFilePath); //以流的方式写入目标路径
                //image.Dispose(); //对象进行释放
            }

            Console.WriteLine("YPN....ConvertImage，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return keyValuePairs;
        }

        #region 从大图中截取一部分图片
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static void CutImage(Image i_image, int offsetX, int offsetY, string toImagePath, int width, int height)
        {
            //原图片文件
            //Image fromImage = Image.FromFile(fromImagePath);
            Image fromImage = i_image;
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            saveImage.Save(toImagePath, ImageFormat.Png);
            //释放资源   
            saveImage.Dispose();
            graphic.Dispose();
            bitmap.Dispose();
        }
        #endregion

        #region 图片压缩
        /// <summary>
        /// 按照图片质量压缩
        /// </summary>
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool CompressImageByQuality(String sourceFile, long quality, String outputFile)
        {
            bool flag = false;
            try
            {
                long imageQuality = quality;
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth), (int)(yWidth));
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(sourceImage, 0, 0, xWidth, yWidth);
                sourceImage.Dispose();
                g.Dispose();
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;

        }

        /// <summary>  
        /// 按图片尺寸收缩倍数压缩
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool CompressImageByMultiple(String sourceFile, int multiple, String outputFile)
        {
            try
            {
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / multiple), (int)(yWidth / multiple));
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
                sourceImage.Dispose();
                g.Dispose();
                newImage.Save(outputFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按图片质量和收缩倍数同时压缩
        /// </summary>
        /// <param name="sourceImage">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>
        public static bool CompressImage(Bitmap sourceImage, long quality, int multiple, String outputFile)
        {
            bool flag = false;
            try
            {
                long imageQuality = quality;
                //DateTime v_StartDT = DateTime.Now;
                //Bitmap sourceImage = new Bitmap(sourceFile);
                //Console.WriteLine("....读取图像文件，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / multiple), (int)(yWidth / multiple));
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
                //sourceImage.Dispose();
                g.Dispose();
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>  
        /// 按图片固定尺寸压缩
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool CompressFixSize(String sourceFile, int xWidth, int yWidth, String outputFile)
        {
            try
            {
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                Bitmap newImage = new Bitmap((int)(xWidth), (int)(yWidth));
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(sourceImage, 0, 0, xWidth, yWidth);
                sourceImage.Dispose();
                g.Dispose();
                newImage.Save(outputFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按图片固定大小压缩
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="targetFile"></param>
        /// <param name="maxLength"></param>
        /// <param name="quality"></param>
        /// <param name="multiple"></param>
        /// <returns></returns>
        public static bool CompressFixLength(Bitmap sourceImage, string targetFile, int maxLength, int quality, int multiple = 1)
        {
            DateTime v_StartDT = DateTime.Now;
            if (!CompressImage(sourceImage, quality, multiple, targetFile))
            {
                sourceImage.Dispose();
                return false;
            }
            long imageLength = new FileInfo(targetFile).Length;
            Console.WriteLine("YPN....FixLength...{0}....{1}，耗时 {2} 秒", multiple, imageLength / 1024, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            if (imageLength <= maxLength * 1024)
            {
                sourceImage.Dispose();
                return true;
            }
            if (multiple >= 5) maxLength = 200;
            return CompressFixLength(sourceImage, targetFile, maxLength, quality, multiple + 1);
        }

        /// <summary>  
        /// 获取图片编码信息  
        /// </summary>  
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        #endregion
    }
}

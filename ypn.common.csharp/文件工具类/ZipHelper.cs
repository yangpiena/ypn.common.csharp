/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：ZipHelper.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/02/06 23:18:44 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/02/06 23:18:44 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="打包压缩工具类" langword="" />
    /// </summary>
    public class ZipHelper
    {
        /// <summary>  
        /// 生成 ***.tar.gz 文件  
        /// </summary>  
        /// <param name="i_BasePath">文件基目录（源文件、生成文件所在目录）</param>  
        /// <param name="i_SourceFolderName">待压缩的源文件夹名</param>  
        public static string CreatTarGzArchive(string i_BasePath, string i_SourceFolderName, string i_TargetFolderName)
        {
            DateTime v_StartDT = DateTime.Now;
            if (string.IsNullOrEmpty(i_BasePath)
                || string.IsNullOrEmpty(i_SourceFolderName)
                || !System.IO.Directory.Exists(i_BasePath)
                || !System.IO.Directory.Exists(Path.Combine(i_BasePath, i_SourceFolderName)))
            {
                return string.Empty;
            }

            Environment.CurrentDirectory = i_BasePath;
            string strSourceFolderAllPath = Path.Combine(i_BasePath, i_SourceFolderName);
            string strOupFileAllPath = Path.Combine(i_BasePath, i_TargetFolderName + ".tar.gz");

            Stream outTmpStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

            //注意此处源文件大小大于4096KB  
            Stream outStream = new GZipOutputStream(outTmpStream);
            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
            archive.WriteEntry(entry, true);

            if (archive != null)
            {
                archive.Close();
            }

            outTmpStream.Close();
            outStream.Close();

            Console.WriteLine("ypn....CreatTarGzArchive，耗时 {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return strOupFileAllPath;
        }

        /// <summary>
        /// 生成 ***.tar 文件
        /// </summary>
        /// <param name="strBasePath">文件基目录（源文件、生成文件所在目录）</param>
        /// <param name="strSourceFolderName">待压缩的源文件夹名</param>
        public static bool CreatTarArchive(string strBasePath, string strSourceFolderName)
        {
            if (string.IsNullOrEmpty(strBasePath)
                || string.IsNullOrEmpty(strSourceFolderName)
                || !System.IO.Directory.Exists(strBasePath)
                || !System.IO.Directory.Exists(Path.Combine(strBasePath, strSourceFolderName)))
            {
                return false;
            }

            Environment.CurrentDirectory = strBasePath;
            string strSourceFolderAllPath = Path.Combine(strBasePath, strSourceFolderName);
            string strOupFileAllPath = Path.Combine(strBasePath, strSourceFolderName + ".tar");

            Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);

            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            TarEntry entry = TarEntry.CreateEntryFromFile(strSourceFolderAllPath);
            archive.WriteEntry(entry, true);

            if (archive != null)
            {
                archive.Close();
            }

            outStream.Close();

            return true;
        }

        /// <summary>
        /// tar包解压
        /// </summary>
        /// <param name="strFilePath">tar包路径</param>
        /// <param name="strUnpackDir">解压到的目录</param>
        /// <returns></returns>
        public static bool UnpackTarFiles(string strFilePath, string strUnpackDir)
        {
            try
            {
                if (!File.Exists(strFilePath))
                {
                    return false;
                }

                strUnpackDir = strUnpackDir.Replace("/", "\\");
                if (!strUnpackDir.EndsWith("\\"))
                {
                    strUnpackDir += "\\";
                }

                if (!Directory.Exists(strUnpackDir))
                {
                    Directory.CreateDirectory(strUnpackDir);
                }

                FileStream fr = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                TarInputStream s = new TarInputStream(fr);
                TarEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(strUnpackDir + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(strUnpackDir + theEntry.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();
                fr.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// zip压缩文件
        /// </summary>
        /// <param name="filename">filename生成的文件的名称，如：C\123\123.zip</param>
        /// <param name="directory">directory要压缩的文件夹路径</param>
        /// <returns></returns>
        public static bool PackFiles(string filename, string directory)
        {
            try
            {
                directory = directory.Replace("/", "\\");

                if (!directory.EndsWith("\\"))
                    directory += "\\";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filename, directory, true, "");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// zip解压文件
        /// </summary>
        /// <param name="file">压缩文件的名称，如：C:\123\123.zip</param>
        /// <param name="dir">dir要解压的文件夹路径</param>
        /// <returns></returns>
        public static bool UnpackFiles(string file, string dir)
        {
            try
            {
                if (!File.Exists(file))
                    return false;

                dir = dir.Replace("/", "\\");
                if (!dir.EndsWith("\\"))
                    dir += "\\";

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                FileStream fr = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ZipInputStream s = new ZipInputStream(fr);
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(dir + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(dir + theEntry.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();
                fr.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

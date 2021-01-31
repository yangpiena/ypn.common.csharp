using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileHelper
    {
        #region 获取文件后缀名
        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="i_filename">文件名</param>
        /// <returns>.doc|.html格式</returns>
        public static string GetPostfix(string i_filename)
        {
            int    v_start   = i_filename.LastIndexOf(".");
            int    v_length  = i_filename.Length;
            return i_filename.Substring(v_start, v_length - v_start);
        }
        #endregion

        #region 去掉文件后缀名
        /// <summary>
        /// 去掉文件后缀名
        /// </summary>
        /// <param name="i_filename"></param>
        /// <returns></returns>
        public static string RemovePostfix(string i_filename)
        {
            int v_start = i_filename.LastIndexOf(".");
            return i_filename.Substring(0, v_start);
        }
        #endregion

        #region 获取文件大小
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="i_filePath">文件路径</param>
        /// <returns>单位：B</returns>
        public static long GetFileSize(string i_filePath)
        {
            FileInfo v_FileInfo = new FileInfo(i_filePath);
            return v_FileInfo.Length;
        }
        #endregion

        #region 获取文件夹大小
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="i_DirPath">文件夹路径</param>
        /// <returns>单位：B</returns>
        public static long GetDirectorySize(string i_DirPath)
        {
            if (!Directory.Exists(i_DirPath))
            {
                return 0;
            }
            long          v_len           = 0;
            DirectoryInfo v_DirInfo = new DirectoryInfo(i_DirPath);
            foreach (FileInfo fi in v_DirInfo.GetFiles())
            {
                v_len += fi.Length;
            }
            DirectoryInfo[] v_DirInfos = v_DirInfo.GetDirectories();
            if (v_DirInfos.Length > 0)
            {
                for (int i = 0; i < v_DirInfos.Length; i++)
                {
                    v_len += GetDirectorySize(v_DirInfos[i].FullName);
                }
            }
            return v_len;
        }
        #endregion

        #region 将文件大小(字节)转换为最适合的显示方式
        /// <summary>
        /// 将文件大小(字节)转换为最适合的显示方式
        /// </summary>
        /// <param name="size">文件字节</param>
        /// <returns>返回转换后的字符串</returns>
        public static string ConvertFileSize(long size)
        {
            string result = "0KB";
            int filelength = size.ToString().Length;
            if (filelength < 4)
                result = size + "byte";
            else if (filelength < 7)
                result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
            else if (filelength < 10)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            else if (filelength < 13)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            else
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            return result;
        }
        #endregion

        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：当文件不存时，则创建文件，并追加文件
         * 参    数：Path:文件路径,Strings:文本内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "这是我写的内容啊";
         *           Common.Utility.FileOperate.WriteFile(Path,Strings);
        *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string Path, string Strings)
        {

            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();


        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = Common.Utility.FileOperate.ReadFile(Path);
        *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 追加文件
        /****************************************
         * 函数名称：FileAdd
         * 功能说明：追加文件内容
         * 参    数：Path:文件路径,strings:内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");     
         *           string Strings = "新追加内容";
         *           Common.Utility.FileOperate.FileAdd(Path, Strings);
        *****************************************/
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        #endregion

        #region 拷贝文件
        /****************************************
         * 函数名称：FileCoppy
         * 功能说明：拷贝文件
         * 参    数：OrignFile:原始文件,NewFile:新文件路径
         * 调用示列：
         *           string OrignFile = Server.MapPath("Default2.aspx");     
         *           string NewFile = Server.MapPath("Default3.aspx");
         *           Common.Utility.FileOperate.FileCoppy(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="OrignFile">原始文件</param>
        /// <param name="NewFile">新文件路径</param>
        public static void FileCoppy(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile, NewFile, true);
        }

        #endregion

        #region 删除文件
        /****************************************
         * 函数名称：FileDel
         * 功能说明：删除文件
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default3.aspx");    
         *           Common.Utility.FileOperate.FileDel(Path);
        *****************************************/
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">路径</param>
        public static void FileDel(string Path)
        {
            File.Delete(Path);
        }
        #endregion

        #region 移动文件
        /****************************************
         * 函数名称：FileMove
         * 功能说明：移动文件
         * 参    数：OrignFile:原始路径,NewFile:新文件路径
         * 调用示列：
         *            string OrignFile = Server.MapPath("../说明.txt");    
         *            string NewFile = Server.MapPath("../../说明.txt");
         *            Common.Utility.FileOperate.FileMove(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="OrignFile">原始路径</param>
        /// <param name="NewFile">新路径</param>
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile, NewFile);
        }
        #endregion

        #region 在当前目录下创建目录
        /****************************************
         * 函数名称：FolderCreate
         * 功能说明：在当前目录下创建目录
         * 参    数：OrignFolder:当前目录,NewFloder:新目录
         * 调用示列：
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           Common.Utility.FileOperate.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/
        /// <summary>
        /// 在当前目录下创建目录
        /// </summary>
        /// <param name="OrignFolder">当前目录</param>
        /// <param name="NewFloder">新目录</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }
        #endregion

        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="Path"></param>
        public static void FileCreate(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path); //创建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }
        #endregion

        #region 递归删除文件夹目录及文件
        /****************************************
         * 函数名称：DeleteFolder
         * 功能说明：递归删除文件夹目录及文件
         * 参    数：dir:文件夹路径
         * 调用示列：
         *           string dir = Server.MapPath("test/");  
         *           Common.Utility.FileOperate.DeleteFolder(dir);       
        *****************************************/
        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>  
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }
        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
        /****************************************
         * 函数名称：CopyDir
         * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
         * 参    数：srcPath:原始路径,aimPath:目标文件夹
         * 调用示列：
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           Common.Utility.FileOperate.CopyDir(srcPath,aimPath);   
        *****************************************/
        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(树形)
        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(树形)
         * 参    数：Path:详细路径
         * 调用示列：
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = Common.Utility.FileOperate.GetFoldAll(strDirlist);  
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件
        /// </summary>
        /// <param name="Path">详细路径</param>
        public static string GetFoldAll(string Path)
        {

            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str);
            return str;

        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件函数
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <returns></returns>
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录
            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "├";
                }
                Rn += "<b>" + dirinfo.Name.ToString() + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name.ToString() + " <br />";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn);


            }
            return Rn;
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(下拉框形)
        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(下拉框形)
         * 参    数：Path:详细路径
         * 调用示列：
         *            string strDirlist = Server.MapPath("templates");      
         *            this.Literal2.Text = Common.Utility.FileOperate.GetFoldAll(strDirlist,"tpl","");
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件(下拉框形)
        /// </summary>
        /// <param name="Path">详细路径</param>
        ///<param name="DropName">下拉列表名称</param>
        ///<param name="tplPath">默认选择模板名称</param>
        public static string GetFoldAll(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName + "\"><option value=\"\">--请选择详细模板--</option>";
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件函数
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <param name="tplPath">默认选择模板名称</param>
        /// <returns></returns>
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn, string tplPath)//递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录

            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                Rn += "<option value=\"" + dirinfo.Name.ToString() + "\"";
                if (tplPath.ToLower() == dirinfo.Name.ToString().ToLower())
                {
                    Rn += " selected ";
                }
                Rn += ">";

                if (nLevel == 0)
                {
                    Rn += "┣";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "┣";
                }
                Rn += "" + dirinfo.Name.ToString() + "</option>";


                FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    Rn += "<option value=\"" + dirinfo.Name.ToString() + "/" + fInfo.Name.ToString() + "\"";
                    if (tplPath.ToLower() == fInfo.Name.ToString().ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";

                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name.ToString() + "</option>";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn, tplPath);


            }
            return Rn;
        }
        #endregion

        #region 获取指定文件详细属性
        /****************************************
         * 函数名称：GetFileAttibe(string filePath)
         * 功能说明：获取指定文件详细属性
         * 参    数：filePath:文件详细路径
         * 调用示列：
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(Common.Utility.FileOperate.GetFileAttibe(file));         
        *****************************************/
        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "详细路径:" + objFI.FullName + "<br>文件名称:" + objFI.Name + "<br>文件长度:" + objFI.Length.ToString() + "字节<br>创建时间" + objFI.CreationTime.ToString() + "<br>最后访问时间:" + objFI.LastAccessTime.ToString() + "<br>修改时间:" + objFI.LastWriteTime.ToString() + "<br>所在目录:" + objFI.DirectoryName + "<br>扩展名:" + objFI.Extension;
            return str;
        }
        #endregion

        #region 将文件转换为byte数组
        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }
        #endregion

        #region 将byte数组转换为文件并保存到指定地址
        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }

        #endregion

        #region 根据键值获取对应资源文件的value值
        /// <summary>
        /// 根据键值获取对应资源文件的value值
        /// </summary>
        /// <param name="resxFileName">资源文件路径</param>
        /// <param name="sKey">key名称</param>
        /// <returns>string</returns>
        public static string GetResxValue(string resxFileName, string sKey)
        {

            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "zh-CN/xxOffline.resources.dll");
           // Type type = assembly.GetType("Resources." + "LogResource");
            //System.Reflection.PropertyInfo[] pa = type.GetProperties();
            ResourceManager re = new ResourceManager("resources", assembly);
        
            return re.GetString(sKey);
        }
        #endregion

        #region Exception新增
        /// <summary>
        /// 当前目录
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                return dir;
            }
        }

        /// <summary>
        /// 防止锁定文件导致其他进程不能写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadToEnd(string path)
        {
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 删除过期文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="dateTime">过期时间</param>
        public static void DelExpireFiles(string path, DateTime dateTime)
        {
            if (!Directory.Exists(path)) return;
            var files = GetAllFiles(path);
            for (int i = 0; i < files?.Count; i++)
            {
                try
                {
                    var item = files[i];
                    if (item.LastWriteTime < dateTime)
                        item.Delete();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 读取目录下的所有文件路径(不包含子目录)
        /// </summary>
        /// <returns></returns>
        public static List<FileInfo> GetAllFiles(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                var files = directoryInfo.GetFiles()?.ToList();
                return files;
            }
            catch (Exception)
            {
                return new List<FileInfo>();
            }
        }

        public static void DownloadOrUpdateFile(string localfileName, string serverFileName)
        {
            if (!File.Exists(localfileName))
            {
                using (var client = new WebClient())
                {
                    var bytes = client.DownloadData(serverFileName);
                    using (var ms = new MemoryStream(bytes))
                    {
                    }
                }
            }
        }

        public static bool TryCopy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overwrite);
                return true;
            }
            catch { }
            return false;
        }

        public static bool TryDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch { }
            return false;
        }
        #endregion 

    }
}
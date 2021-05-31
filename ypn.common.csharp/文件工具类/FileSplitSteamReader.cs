/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：FileSplitSteamReader.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/02/25 20:27:25 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/02/25 20:27:25 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 文件分块读
    /// <see cref="FileSplitSteamReader" langword="" />
    /// </summary>
    public class FileSplitSteamReader : FileStream
    {
        #region //文件分块读
        /// <summary>
        /// 用于普通文件读取
        /// </summary>
        /// <param name="sourceFileName">文件的路径</param>
        public FileSplitSteamReader(string sourceFileName)
        : base(sourceFileName, FileMode.Open, FileAccess.Read)
        {
            this.sourceFileName = sourceFileName;
        }
        /// <summary>
        /// 用于大文件分块读取
        /// </summary>
        /// <param name="sourceFileName">文件的路径</param>
        /// <param name="splitSize">切分大小</param>
        public FileSplitSteamReader(string sourceFileName, int splitSize)
        : base(sourceFileName, FileMode.Open, FileAccess.Read)
        {
            this.sourceFileName = sourceFileName;
            this.splitSize = splitSize;
        }
        private string sourceFileName;
        /// <summary>
        /// 获取文件的路径
        /// </summary>
        public string SourceFileName
        {
            get { return sourceFileName; }
        }
        private long splitSize;
        /// <summary>
        /// 每次切分文件大小splitSize
        /// </summary>
        public long SplitSize
        {
            get { return splitSize; }
        }
        /// <summary>
        /// 文件的大小
        /// </summary>
        public long FileSize
        {
            get { return this.Length; }
        }
        private long readTimes = 1;
        /// <summary>
        /// 当前读取次数块号
        /// </summary>
        public long ReadTimes
        {
            get { return readTimes; }
        }
        private bool judge = false;
        /// <summary>
        /// 用于判断是否执行到最后一块.读完为ture,未读完为false.
        /// </summary>
        public bool Judge
        {
            get { return judge; }
            set { judge = value; }
        }
        /// <summary>
        /// 最后一次读取文件的大小
        /// </summary>
        public long FinilReadSize
        {
            get
            {
                if (splitSize == 0)
                    return 1024 * 1024 * 3;
                else
                    return this.FileSize - (this.FileSize / (long)this.splitSize) * (long)this.splitSize;
            }
        }
        public int CurrentReadSize;
        /// <summary>
        /// 开始读取文件
        /// </summary>
        /// <returns>以Bitmap类型返回每次读取文件的内容</returns>
        public byte[] SpliteRead()
        {
            FileBlockSteamReaderEventArgs Fbsr = new FileBlockSteamReaderEventArgs();
            byte[] timeReadContect;
            this.Seek(splitSize * (readTimes - 1), SeekOrigin.Begin);
            if (readTimes < (this.FileSize / this.splitSize + 1))
            {
                Fbsr.ReadPercent = (int)(((float)this.Position / this.FileSize) * 100);
                timeReadContect = new byte[this.splitSize];
            }
            else
            {
                timeReadContect = new byte[this.FinilReadSize];
                judge = true;
                Fbsr.ReadPercent = 100;
            }
            CurrentReadSize = timeReadContect.Length;
            this.Read(timeReadContect, 0, timeReadContect.Length);
            FileABlockReadEndEvent(Fbsr);
            readTimes++;
            return timeReadContect;
        }
        public event EventHandler<FileBlockSteamReaderEventArgs> ABlockReadEndEvent;
        public event EventHandler FinishAllReadEvent;
        /// <summary>
        /// 文件读取完当前块当前事件
        /// </summary>
        /// <param name="e"></param>
        public void FileABlockReadEndEvent(FileBlockSteamReaderEventArgs e)
        {
            if (ABlockReadEndEvent != null)
            {
                this.ABlockReadEndEvent(this, e);
            }
        }
        /// <summary>
        /// 文件读取完最后一块的事件
        /// </summary>
        public void FileFinishAllReadEvent()
        {
            if (FinishAllReadEvent != null)
            {
                this.FinishAllReadEvent(this, new EventArgs());
            }
        }
        #endregion
    }

    public class FileBlockSteamReaderEventArgs : EventArgs
    {
        private int readPercent;
        public int ReadPercent
        {
            get { return readPercent; }
            set { readPercent = value; }
        }
    }
}

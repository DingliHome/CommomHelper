using System;
using System.Diagnostics;
using System.IO;

namespace ClassLibraryHelper.C_.Video
{
    public class VideoOperation
    {
        #region 字段

        /// <summary>
        ///     视频转换工具的路径
        /// </summary>
        private readonly string ffmpeg = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Tools\\ffmpeg.exe";

        /// <summary>
        ///     提过本地进程访问
        /// </summary>
        private readonly Process process;

        #endregion

        #region 属性

        /// <summary>
        ///     是否初始化成功
        /// </summary>
        public bool IsFinishInitial { get; set; }

        /// <summary>
        ///     During时长(D)、Width视频宽(W)、视频高(H)的联合字符串
        ///     例子：00:04:32.46x800x600
        /// </summary>
        public string DuringTime { get; set; }

        public string VideoWidth { get; set; }

        public string VideoHeight { get; set; }

        #endregion

        #region 构造

        public VideoOperation()
        {
            if (!File.Exists(ffmpeg))
            {
                //若不存在

                IsFinishInitial = false;

                return;
            }

            //初始化ffmpeg进程

            process = new Process();

            //不启动新窗口

            process.StartInfo.CreateNoWindow = true;

            //不用操作系统shell

            process.StartInfo.UseShellExecute = false;

            //设置启动程序路径

            process.StartInfo.FileName = ffmpeg;

            //初始化成功

            IsFinishInitial = true;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     截取视频截图
        ///     同时异步从视频读取信息During时长(D)、Width视频宽(W)、视频高(H)
        ///     存在字符串StrDWH
        /// </summary>
        /// <param name="fileName">需要截取图片的视频路径</param>
        /// <param name="imgFile">截取图片后保存的图片路径</param>
        /// <param name="imgSize">获取截取图片的大小,默认"100x100"</param>
        public void GetDWHFromVideo(string fileName, string imgFile = "", string imgSize = "100x100")
        {
            //就看视频

            //获取截图后保存的路径

            string flv_img = imgFile;

            // pss = new Process();

            //设置启动程序的路径

            process.StartInfo.Arguments = " -i \"" + fileName + "\" -y -f -ss"; //不生成图片
            //process.StartInfo.Arguments = " -i \"" + fileName + "\" -y -f image2 -ss 2 -vframes 1 -s " + imgSize + " " + flv_img; //生成图片

            process.StartInfo.RedirectStandardError = true; //输出信息第一步，基础输出重定向

            DataReceivedEventHandler handler = null;

            process.ErrorDataReceived += handler = (s, e) => //输出信息第二步，调用接收到信息时执行的函数
                {
                    if (String.IsNullOrEmpty(e.Data)) return;

                    string dataReceived = e.Data.Trim();

                    if (dataReceived.StartsWith("Duration:"))
                    {
                        //例子： Duration: 01:56:28.47, start: 0.000000, bitrate: 291 kb/s

                        DuringTime = dataReceived.Split(',')[0].Substring(9);
                    }

                    if (dataReceived.Contains("Video:"))
                    {
                        //StrDWH += "x"+dataReceived.Split(',')[2].Split('[')[0];

                        VideoWidth = dataReceived.Split(',')[2].Trim().Split('[')[0].Split('x')[0];
                        VideoHeight = dataReceived.Split(',')[2].Trim().Split('[')[0].Split('x')[1];

                        //不在监听数据接收

                        process.ErrorDataReceived -= handler;
                    }
                };

            //启动

            process.Start();

            process.BeginErrorReadLine(); //输出信息第三步，开始执行

            process.WaitForExit();
        }

        #endregion

        //顺便写个视频转换方法也贴上好了，

        /// <summary>
        ///     视频格式转换，转换成指定像素尺寸的FLV视频
        /// </summary>
        /// <param name="fileName">原视频</param>
        /// <param name="outPutPath">输出视频</param>
        /// <param name="size">尺寸,不赋值则用原来的</param>
        public void VideoFormatToFLV(string fileName, string outPutPath, string size = "")
        {
            //参数：-ab 比特率

            if (string.IsNullOrWhiteSpace(size))
            {
                process.StartInfo.Arguments = " -i " + fileName + " -ab 128 -ar 22050 -qscale 6 -r 29.97 " + outPutPath;
            }

            else
            {
                process.StartInfo.Arguments = " -i " + fileName + " -ab 128 -ar 22050 -qscale 6 -r 29.97 -s " + size +
                                              " " + outPutPath;
            }

            process.Start();

            process.WaitForExit();
        }
    }
}
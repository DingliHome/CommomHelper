using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ClassLibraryHelper.C_.FTP
{
    public class FTPHelper
    {
        /// <summary>
        /// FTP上传文件
        /// </summary>
        /// <param name="fromFileName">本地文件</param>
        /// <param name="toFileName">服务器文件</param>
        /// <param name="ftpServerIp">ftp IP地址</param>
        /// <param name="ftpUserId">用户名</param>
        /// <param name="ftpPassword">密码</param>
        /// <returns></returns>
        public static string Upload(string fromFileName, string toFileName, string ftpServerIp, string ftpUserId,
                                    string ftpPassword)
        {
            string msg = "uploaded success";
            //得到本地文件信息
            var fileInfo = new FileInfo(fromFileName);
            //获取ftp上传文件地址
            string uri = string.Format("ftp://{0}/{1}", ftpServerIp, toFileName);
            var request = (FtpWebRequest) WebRequest.Create(new Uri(uri));
            //用户名和密码
            request.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
            request.KeepAlive = false; //保持连接为false
            request.Method = WebRequestMethods.Ftp.UploadFile;
            //指定数据类型
            request.UseBinary = true;
            request.UsePassive = false;
            request.ContentLength = fileInfo.Length;

            //缓冲大小2kb
            int buffLength = 2048;
            var buff = new byte[buffLength];
            int contentLen;
            //打开文件流去读上传的文件
            FileStream fs = fileInfo.OpenRead();
            try
            {
                //上传的文件写入流
                Stream stream = request.GetRequestStream();
                //每次2kb的读文件流
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);
                }
                stream.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// Ftp 下载文件
        /// </summary>
        /// <param name="fromFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="ftpServerIp"></param>
        /// <param name="ftpUserId"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="termId"></param>
        /// <returns></returns>
        public static string DownLoad(string fromFilePath, string fileName, string ftpServerIp, string ftpUserId,
                                      string ftpPassword, string termId)
        {
            string msg = "OK";
            FtpWebRequest ftpWebRequest;
            try
            {
                string filePath = string.Format("{0}\\Data\\{1}", Environment.CurrentDirectory, termId);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = string.Format("{0}\\{1}", filePath, fileName);
                fromFilePath = string.Format("{0}\\{1}", fromFilePath, fileName);
                string uri = string.Format("ftp://{0}/{1}", ftpServerIp, fromFilePath);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri(uri));
                ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = false;
                var response = (FtpWebResponse) ftpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                int bufferSize = 2048;
                var buffer = new byte[bufferSize];
                int readCount;
                readCount = stream.Read(buffer, 0, bufferSize);
                var fileStream = new FileStream(filePath, FileMode.Create);
                while (readCount > 0)
                {
                    fileStream.Write(buffer, 0, readCount);
                    readCount = stream.Read(buffer, 0, bufferSize);
                }

                fileStream.Flush();
                fileStream.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// FTP 创建目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public static string MakeDir(string dirName, string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            string sRet = "OK";
            try
            {
                string uri = string.Format("ftp://{0}/{1}", ftpServerIP, dirName);
                FtpWebRequest reqFTP;

                // 根据uri创建FtpWebRequest对象 
                reqFTP = (FtpWebRequest) WebRequest.Create(new Uri(uri));

                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                // 指定数据传输类型
                reqFTP.UseBinary = true;

                var respFTP = (FtpWebResponse) reqFTP.GetResponse();
                respFTP.Close();
            }
            catch (Exception ex)
            {
                sRet = ex.Message;
            }
            return sRet;
        }

        /// <summary>
        /// FTP获取文件列表
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public static string[] GetFileList(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            string[] downloadFiles;
            var result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'        
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// FTP获取目录列表
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="dirName"></param>
        /// <param name="fileOrDir"></param>
        /// <returns></returns>
        public static List<string> GetDirList(string ftpServerIP, string ftpUserID, string ftpPassword, string dirName,
                                              string fileOrDir)
        {
            var downloadFiles = new List<string>();
            var result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + dirName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'        
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();

                string[] strsFileList = result.ToString().Split('\n');

                if (fileOrDir.ToLower() == "f")
                {
                    fileOrDir = "-rw-rw-rw";
                }
                else
                {
                    fileOrDir = "drw-rw-rw";
                }

                //解析文件夹
                foreach (string s in strsFileList)
                {
                    if (s.IndexOf(fileOrDir) >= 0)
                    {
                        string s2 = "";
                        string[] sl1 = s.Split(' ');
                        int j = 0;
                        for (int i = 0; i < sl1.Length; i++)
                        {
                            //非空项
                            if (sl1[i] != "")
                            {
                                j++;
                            }

                            //超过9以上开始系文件夹
                            if (j > 8)
                            {
                                s2 = sl1[i] + " ";
                            }
                        }
                        s2 = s2.Trim();

                        if (s2 != "." && s2 != "..")
                        {
                            downloadFiles.Add(s2);
                        }
                    }
                }

                return downloadFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public static string DeleteFileName(string fileName, string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            string sRet = "OK";
            string uri = "ftp://" + ftpServerIP + "/" + fileName;
            FtpWebRequest reqFTP;

            try
            {
                reqFTP = (FtpWebRequest) WebRequest.Create(new Uri(uri));
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                var response = (FtpWebResponse) reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                sRet = ex.Message;
            }

            return sRet;
        }
    }
}
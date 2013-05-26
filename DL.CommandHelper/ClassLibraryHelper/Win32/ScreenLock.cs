using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClassLibraryHelper.Win32
{
    public class ScreenLock
    {
        [DllImport("user32 ")]
        public static extern bool LockWorkStation();//这个是调用windows的系统锁定
        [DllImport("user32.dll")]
        static extern void BlockInput(bool Block);   


        public void LockScreen()
        {
            LockWorkStation();
        }

         //锁定鼠标及键盘 
        public void LockAll()
        {
            BlockInput(true);
        }

        //锁定任务管理器
        public void LockTaskManager()
        {
            FileStream fs = new FileStream(Environment.ExpandEnvironmentVariables("%windir%\\system32\\taskmgr.exe"), FileMode.Open,FileAccess.Read,FileShare.Read);
        }

        public void UnLock()
        {
            BlockInput(false);
        }
    }
}

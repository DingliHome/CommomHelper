using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ClassLibraryHelper.C_.Print
{
    public class PrintDialogHelper
    {
        private const string PrintServerName = "Lee-pc";
        private const string PrintName = "hp 打印机";

        public PrintDialogHelper()
        {

        }

        /// <summary>
        /// 打印控件
        /// </summary>
        /// <param name="element"></param>
        public void PrintVisual(FrameworkElement element)
        {
            var printDialog = new PrintDialog();
            SetPrintProperty(printDialog);
            var printQueue = SelectedPrintServer(PrintServerName, PrintName);
            if (printQueue != null) 
                printDialog.PrintQueue = printQueue;

            printDialog.PrintVisual(element,"");

        }

        /// <summary>
        /// 查找并获取打印机
        /// </summary>
        /// <param name="printerServerName">服务器名字： Lee-pc</param>
        /// <param name="printerName">打印机名字：Hp laserjet m1522 mfp series pcl 6 </param>
        /// <returns></returns>
        public PrintQueue SelectedPrintServer(string printerServerName, string printerName)
        {
            try
            {
                var printers = PrinterSettings.InstalledPrinters;//获取本机上的所有打印机
                PrintServer printServer = null;

                foreach (string printer in printers)
                {
                    if (printer.Contains(printerServerName) && printer.Contains("\\\\"))
                        printServer = new PrintServer("\\\\" + printerServerName);
                }

                if (printServer == null) return null;//没有找到打印机

                var printQueue = printServer.GetPrintQueue(printerName);
                return printQueue;
            }
            catch (Exception)
            {
                return null;//没有找到打印机
            }
        }

        /// <summary>
        /// 设置打印格式
        /// </summary>
        /// <param name="printDialog">打印文档</param>
        /// <param name="pageSize">打印纸张大小 a4</param>
        /// <param name="pageOrientation">打印方向 竖向</param>
        public void SetPrintProperty(PrintDialog printDialog, PageMediaSizeName pageSize = PageMediaSizeName.ISOA4, PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            var printTicket = printDialog.PrintTicket;
            printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
            printTicket.PageOrientation = pageOrientation;//默认竖向打印
        }
    }
}

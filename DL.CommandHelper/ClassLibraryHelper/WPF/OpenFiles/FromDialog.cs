using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ClassLibraryHelper.WPF.OpenFiles
{
    public class FromDialog : IDialog
    {
        private const string Filter = "XML(*.xml)|*.xml";

        public string GetFileNameForOpening(string filiter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filiter;
            openFileDialog.ShowDialog();

            return openFileDialog.FileName;
        }

        public string GetFileNameForSaving(string filiter)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filiter;
            saveFileDialog.ShowDialog();

            return saveFileDialog.FileName;
        }

        public string GetFolder(string folder)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = folder;
                folderBrowserDialog.ShowDialog();

                return folderBrowserDialog.SelectedPath;
            }
        }

        public MessageBoxResult ShowQuestion(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}

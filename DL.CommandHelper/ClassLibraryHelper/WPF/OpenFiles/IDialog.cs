using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ClassLibraryHelper.WPF.OpenFiles
{
    public interface IDialog
    {
        /// <summary>
        /// Gets the file name for opening.
        /// </summary>
        /// <param name="filiter">The filiter.</param>
        /// <returns></returns>
        string GetFileNameForOpening(string filiter);

        /// <summary>
        /// Gets the file name for saving.
        /// </summary>
        /// <param name="filiter">The filiter.</param>
        /// <returns></returns>
        string GetFileNameForSaving(string filiter);

        /// <summary>
        /// Gets the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        string GetFolder(string folder);

        /// <summary>
        /// Shows the question.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        MessageBoxResult ShowQuestion(string message, string caption);

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowMessage(string message);
    }
}

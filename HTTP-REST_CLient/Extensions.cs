using System.Windows;
using System.Windows.Controls;

namespace HTTP_REST_CLient
{
    public static class Extensions
    {
        /// <summary>
        /// Filling listbox by splitting input string
        /// </summary>
        /// <param name="filePaths">FileNames divided by \n.</param>
        public static void Fill(this ListBox listBox, string filePaths)
        {
            listBox.Items.Clear();
            if (filePaths.Length > 0)
            {
                string[] files = filePaths.Split('\n');
                for (int i = 0; i < files.Length; i++)
                {
                    listBox.Items.Add(System.IO.Path.GetFileName(files[i]));
                }
            }
        }
        
        public static string GetParentDir(string filename)
        {
            int index = 0;
            for (int i = filename.Length - 2; i >= 0; i--)
            {
                if (filename[i] == '\\')
                {
                    index = i;
                    break;
                }
            }

            string parentPath = filename.Substring(0, index);
            return parentPath;
        }
        
        public static void ShowMsgBox(string code)
        {
            MessageBox.Show(code, "Something went wrong", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
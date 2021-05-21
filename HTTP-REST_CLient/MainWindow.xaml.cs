using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HTTP_REST_CLient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string CurrentPath { get; set; } = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowStorageButton_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance().ShowStorage();
            }
            catch (HttpRequestException ex)
            {
                Extensions.ShowMsgBox(ex.Message);
                return;
            }

            if (ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                CurrentPath = "";
                CurrentPathTextBox.Content = "root\\";
                FilesListBox.Fill(await responseMessage.Content.ReadAsStringAsync());
                Console.WriteLine(msg);
            }
            else
            {
                Extensions.ShowMsgBox(msg);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (FilesListBox.SelectedItem != null)
            {
                string toDelete = CurrentPath + FilesListBox.SelectedItem;
                HttpResponseMessage responseMessage = null;
                try
                {
                    responseMessage = await ClientService.GetInstance().Delete(toDelete);
                }
                catch (HttpRequestException ex)
                {
                    Extensions.ShowMsgBox(ex.Message);
                    return;
                }

                if (ResponseChecker.CheckResponse(responseMessage, out string msg))
                {
                    await Update();
                    Console.WriteLine(msg);
                }
                else
                {
                    Extensions.ShowMsgBox(msg);
                }
            }
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            CreateFileWindow fileWindow = new CreateFileWindow();
            fileWindow.Owner = this;
            fileWindow.Show();
        }

        private void AppendButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilesListBox.SelectedItem != null)
            {
                AppendFileWindow window = new AppendFileWindow();
                string filename = FilesListBox.SelectedItem.ToString();
                window.Title = filename;
                window.Owner = this;
                window.Show();
            }
        }

        private async void Back_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentPath = Extensions.GetParentDir(CurrentPath);
            if (CurrentPath.Length > 0) CurrentPath += '\\';
            CurrentPathTextBox.Content = "root\\" + CurrentPath;
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance().Open(CurrentPath);
            }
            catch (HttpRequestException ex)
            {
                Extensions.ShowMsgBox(ex.Message);
                return;
            }

            if (ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                FilesListBox.Fill(await responseMessage.Content.ReadAsStringAsync());
                await Update();
            }
            else
            {
                Extensions.ShowMsgBox(msg);
            }
        }

        private async void FilesListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string filename = FilesListBox.SelectedItem?.ToString();

            if (filename == null) return;
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance().Open(CurrentPath + filename);
            } 
            catch (HttpRequestException ex)
            {
                Extensions.ShowMsgBox(ex.Message);
                return;
            }

            if (ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                if (responseMessage.Headers.TryGetValues("isDir", out var value))
                {
                    if (bool.TryParse(value.First(), out bool isDir))
                    {
                        if (isDir)
                        {
                            CurrentPath += filename + '\\';
                            CurrentPathTextBox.Content = "root\\" + CurrentPath;
                            FilesListBox.Fill(await responseMessage.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            OpenFileWindow window = new OpenFileWindow();
                            window.Title = filename;
                            window.ContentTextBox.Text = await responseMessage.Content.ReadAsStringAsync();
                            window.Show();

                            await Update();
                        }
                    }
                    else
                    {
                        Extensions.ShowMsgBox(msg);
                    }
                }
                else
                {
                    Extensions.ShowMsgBox("Header \'isDir\' was not found");
                }
            }
            else
            {
                Extensions.ShowMsgBox(msg);
            }
        }

        public async Task Update()
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance().Update();
            }
            catch (HttpRequestException ex)
            {
                Extensions.ShowMsgBox(ex.Message);
                return;
            }

            if (ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                FilesListBox.Fill(await responseMessage.Content.ReadAsStringAsync());
            }
            else
            {
                Extensions.ShowMsgBox(msg);
            }
        }


        private void CopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilesListBox.SelectedItem != null)
            {
                var window = new CopyFileWindow();
                string filename = FilesListBox.SelectedItem.ToString();
                window.Title = filename;
                window.Owner = this;
                window.Show();
            }
        }
    }
}
using System;
using System.Net.Http;
using System.Windows;

namespace HTTP_REST_CLient
{
    public partial class CopyFileWindow : Window
    {
        public CopyFileWindow()
        {
            InitializeComponent();
        }

        private async void CopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow) this.Owner;
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance()
                    .Copy(MainWindow.CurrentPath + Title, DstFileTextBox.Text, DeleteSrcCheckBox.IsChecked);
            }
            catch (HttpRequestException ex)
            {
                Extensions.ShowMsgBox(ex.Message);
                return;
            }

            Close();
            if (!ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                Extensions.ShowMsgBox(msg);
            }

            await main.Update();
        }
    }
}
using System;
using System.Net.Http;
using System.Windows;

namespace HTTP_REST_CLient
{
    public partial class AppendFileWindow : Window
    {
        public AppendFileWindow()
        {
            InitializeComponent();
        }

        private async void AppendButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow) this.Owner;
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await ClientService.GetInstance()
                    .AppendFile(MainWindow.CurrentPath + Title, new StringContent(ContentTextBox.Text));
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
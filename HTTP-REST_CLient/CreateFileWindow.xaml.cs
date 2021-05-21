using System.Net.Http;
using System.Text;
using System.Windows;
namespace HTTP_REST_CLient
{
    public partial class CreateFileWindow : Window
    {
        public CreateFileWindow()
        {
            InitializeComponent();
        }

        private async void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage responseMessage;
            if (IsFolderCheckBox.IsChecked == false)
            {
                responseMessage = await ClientService.GetInstance().AddFile(FileNameTextBox.Text,
                        new StringContent(ContentTextBox.Text, Encoding.UTF8));
                    
            }
            else
            {
                responseMessage = await ClientService.GetInstance().AddFolder(FileNameTextBox.Text);
            }
            Close();
            MainWindow main = (MainWindow) this.Owner;
            if (!ResponseChecker.CheckResponse(responseMessage, out string msg))
            {
                Extensions.ShowMsgBox(msg);
            }
            await main.Update();
        }

        private void IsFolderCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            ContentTextBox.IsEnabled = !ContentTextBox.IsEnabled;
        }
    }
}
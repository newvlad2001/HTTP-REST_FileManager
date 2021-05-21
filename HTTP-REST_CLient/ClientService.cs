using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_REST_CLient
{
    public class ClientService
    {
        private static ClientService _instance;
        private HttpClient _httpClient;

        public string _serverAddress = "http://192.168.43.254:8080/";

        private ClientService()
        {
            _httpClient = new HttpClient();
        }

        public static ClientService GetInstance()
        {
            if (_instance == null)
                _instance = new ClientService();
            return _instance;
        }

        public async Task<HttpResponseMessage> AddFile(string fileName, StringContent content)
        {
            return await _httpClient.PutAsync(_serverAddress + "createFile?file=" + MainWindow.CurrentPath + fileName,
                content);
        }

        public async Task<HttpResponseMessage> AddFolder(string fileName)
        {
            return await _httpClient.PutAsync(_serverAddress + "createDir?dirname=" + MainWindow.CurrentPath + fileName,
                new StringContent(""));
        }

        public async Task<HttpResponseMessage> AppendFile(string fileName, StringContent content)
        {
            return await _httpClient.PostAsync(_serverAddress + "appendFile?file=" + fileName, content);
        }

        public async Task<HttpResponseMessage> Open(string fileName)
        {
            return await _httpClient.GetAsync(_serverAddress + "open?file=" + fileName);
        }

        public async Task<HttpResponseMessage> Update()
        {
            return await _httpClient.GetAsync(_serverAddress + "open?file=" + MainWindow.CurrentPath);
        }

        public async Task<HttpResponseMessage> ShowStorage()
        {
            return await _httpClient.GetAsync(_serverAddress + "showStorage");
        }

        public async Task<HttpResponseMessage> Delete(string filename)
        {
            return await _httpClient.DeleteAsync(_serverAddress + "delete?file=" + filename);
        }

        public async Task<HttpResponseMessage> Copy(string srcFilename, string dstFilename, bool? deleteSrc)
        {
            return await _httpClient.PutAsync(_serverAddress + "copy?src=" + srcFilename + "&dst=" + dstFilename +
                                              "&deleteSrc=" + deleteSrc, new StringContent(""));
        }
    }
}
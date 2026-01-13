using Newtonsoft.Json;
using Planning_MIS.DocumentAPI.File;
using System.Text;

namespace Planning_MIS.API.DocumentAPI
{
    public class DocumentAPIHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _documentApiUrl;
    

        public DocumentAPIHelper(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _documentApiUrl = configuration["AppSettings:DocumentAPI"];
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_documentApiUrl);
        }

        public async Task<List<FileType>> GetFileTypes(string formType, string formSubType, bool showAll = false)
        {
            var response = await _httpClient.GetAsync($"/api/file/fileTypes?formType={formType}&formSubType={formSubType}&showAll={showAll}");
            if (!response.IsSuccessStatusCode) return new List<FileType>();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileType>>(respText);
        }


        public async Task<FileRecord> GetFileRecord(string fileId)
        {
            var response = await _httpClient.GetAsync($"/api/file/fileRecord?fileId={fileId}");
            if (!response.IsSuccessStatusCode) return null;

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileRecord>>(respText)?.FirstOrDefault();
        }


        public async Task<List<FileRecord>> GetFileRecords(string masterId, string dataId, int fileTypeId)
        {
            var response = await _httpClient.GetAsync($"/api/file/fileRecord?masterId={masterId}&dataId={dataId}&fileTypeId={fileTypeId}");
            if (!response.IsSuccessStatusCode) return new List<FileRecord>();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileRecord>>(respText);
        }


        public async Task<List<FileRecord>> GetFileRecords(string dataId)
        {
            var response = await _httpClient.GetAsync($"/api/file/fileRecords?dataId={dataId}");
            if (!response.IsSuccessStatusCode) return new List<FileRecord>();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileRecord>>(respText);
        }


        public async Task<Stream> DownloadFile(string fileId, string userId)
        {
            try
            {
                return await _httpClient.GetStreamAsync($"/api/file/download?fileId={fileId}&userId={userId}");
            }
            catch
            {
                return null;
            }
        }


        public async Task<FileUploadResult> UploadFile(FileUploadModel model)
        {
            if (model?.File == null || model.File.Length == 0)
                return new FileUploadResult();

            using var multiContent = new MultipartFormDataContent();
            using var ms = new MemoryStream();
            await model.File.CopyToAsync(ms);
            ms.Position = 0;

            multiContent.Add(new ByteArrayContent(ms.ToArray()), "file", model.File.FileName);
            multiContent.Add(new StringContent(model.MasterId ?? ""), "masterId");
            multiContent.Add(new StringContent(model.DataId), "dataId");
            multiContent.Add(new StringContent(model.FileTypeId.ToString()), "fileTypeId");
            multiContent.Add(new StringContent(model.CreatedBy.ToString()), "userId");

            var response = await _httpClient.PostAsync("/api/file/upload", multiContent);
            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FileUploadResult>(responseText);
        }


        public async Task<FileRemoveResult> RemoveFile(string fileId)
        {
            var model = new FileRemoveModel { FileId = fileId, UserId = "client" };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/file/remove", content);
            if (!response.IsSuccessStatusCode) return new FileRemoveResult();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FileRemoveResult>(respText);
        }


        public async Task<FileRemoveResult> UpdateMasterId(string dataId, string masterId)
        {
            var model = new FileUpdateMasterIdModel { DataId = dataId, MasterId = masterId };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/file/updatemasterId", content);
            if (!response.IsSuccessStatusCode) return new FileRemoveResult();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FileRemoveResult>(respText);
        }


        public async Task<FileValidationResult> ValidateDocuments(FileValidationModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/file/validate", content);
            if (!response.IsSuccessStatusCode) return new FileValidationResult();

            var respText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FileValidationResult>(respText);
        }

    }
}

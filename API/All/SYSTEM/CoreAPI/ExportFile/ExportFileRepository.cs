using API.All.DbContexts;
using API.DTO;
using CORE.DTO;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RegisterServicesWithReflection.Services.Base;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.Json.Nodes;

namespace API.All.SYSTEM.CoreAPI.ExportFile
{
    [ScopedRegistration]
    public class ExportFileRepository : IExportFileRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;

        public ExportFileRepository(FullDbContext context, IWebHostEnvironment env, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _env = env;
            _appSettings = options.Value;
        }

        public async Task<FileReturnDTO> ExportExel(ExportExelRequest request)
        {
            try
            {
                List<string> listHeader = new List<string>();
                foreach (var item in request.Headers)
                {
                    listHeader.Add(item.Caption);
                }
                string _sample = $"ExportExelSample.xlsx";
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                string _path_sample = Path.Combine(location, _sample);
                string destPath = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments, $"StorageFile");
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                FileInfo file = new FileInfo(_path_sample);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    if (package.Workbook.Worksheets.Count > 0)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet 1"];
                        // Open sheet1

                        DataTable data = JsonConvert.DeserializeObject<DataTable>(request.ListData.ToString());
                        int begin_row = 1;
                        int totalRows = data.Rows.Count;
                        // Add Row
                        if (totalRows != 0)
                        {
                            worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                        }
                        // Fill data
                        int idxHeader = begin_row;

                        for (int i = begin_row; i < listHeader.Count; i++)
                        {
                            worksheet.Column(i).Width = 100;
                            worksheet.Cells[1, i].Value = listHeader[i];
                        }



                        worksheet.Cells[idxHeader + 1, 1].LoadFromDataTable(data, false);

                        string fileName = $"ExportExel.xlsx";
                        string filePath = Path.Combine(destPath, fileName);
                        package.SaveAs(new FileInfo(filePath));
                        byte[] fileByte = File.ReadAllBytes(filePath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        return new FileReturnDTO
                        {
                            Bytes = fileByte,
                            ContentType = MimeTypes.GetMimeType(filePath),
                            FileName = Path.GetFileName(filePath)
                        };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}

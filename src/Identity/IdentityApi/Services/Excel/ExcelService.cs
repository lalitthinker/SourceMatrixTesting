using OfficeOpenXml;
using System.IO;

namespace IdentityApi.Services.Excel
{
    public class ExcelService : IExcelService
    {
        public MemoryStream CreateExcel(object list, MemoryStream stream, string FileName)
        {
            try
            {
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    switch (FileName)
                    {
                        case "Users List":

                            workSheet.Cells.LoadFromCollectionFiltered((List<UserProfileViewModel>)list);
                            break;
                    }
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    package.Save();
                }
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return stream;
        }

        public MemoryStream CreateExcelIntoExcelTemplate(object list, string path, string FileName)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                var package = new ExcelPackage(stream);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet workSheet = p.Workbook.Worksheets.Add("Sheet1");

                    switch (FileName)
                    {
                        case "Users List":

                            workSheet.Cells["A1"].LoadFromCollectionFiltered((List<UserProfile>)list);
                            break;
                    }
                    p.SaveAs(stream);
                    stream.Position = 0;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return stream;
        }
    }
}

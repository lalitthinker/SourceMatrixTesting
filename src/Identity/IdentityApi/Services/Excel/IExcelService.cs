using System.IO;

namespace IdentityApi.Services.Excel
{
    public interface IExcelService
    {
        MemoryStream CreateExcel(object list, MemoryStream stream, string FileName);

        MemoryStream CreateExcelIntoExcelTemplate(Object list, string path, string FileName);
    }
}

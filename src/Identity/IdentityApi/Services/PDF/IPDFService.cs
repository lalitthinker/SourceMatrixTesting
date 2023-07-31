using System.IO;

namespace IdentityApi.Services.PDF
{
    public interface IPDFService
    {
        void CreatePdf(Stream stream, List<UserProfile> users);
    }
}

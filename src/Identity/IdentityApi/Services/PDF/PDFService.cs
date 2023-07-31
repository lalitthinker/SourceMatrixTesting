
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;

namespace IdentityApi.Services.PDF
{
    public class PDFService : IPDFService
    {
        private readonly IPDFCommonSettings _pdfCommonSettings;

        public PDFService(IPDFCommonSettings pdfCommonSettings)
        {
            _pdfCommonSettings = pdfCommonSettings ?? throw new ArgumentNullException(nameof(pdfCommonSettings));
        }

        public void CreatePdf(Stream stream, List<UserProfile> users)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            //var pageSize = PageSize.A4_LANDSCAPE.Rotate;
            var doc = new Document(new Rectangle(200f, 400f), 20, 20, 20, 20);
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            var pdfWriter = PdfWriter.GetInstance(doc, stream);

            doc.Open();

            PrintHeader(doc, pdfWriter, "Users Report");

            PrintLine(doc);

            PrintUsersReportLists(doc, users);

            doc.Close();
        }

        private void PrintUsersReportLists(Document doc, List<UserProfile> users)
        {
            var table = new PdfPTable(new float[] {8f, 19F, 19F, 16F, 19F, 25f, 18f, 17f, 17f, 14f, 16F, 17F, 23F, 20f, 21f})
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };

            table.SpacingBefore = 2f;
            table.SpacingAfter = 2f;
            table.HeaderRows = 1;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Sr No",             columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "FullName",          columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "UserName",          columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Ph.No.",            columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "SaleQuota",         columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "PurchaseQuota",     columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Email",             columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "TimeZone",          columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Roles",             columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Desc",              columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Created Dt.",       columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "UserStatus",        columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Emerg.ContactName", columnType: ColumnType.Number,    IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Emergency No",      columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            table.AddCell(_pdfCommonSettings.GetBodyCell(text: "OfficePh.No.",      columnType: ColumnType.Number,  IsHeader: true, IsTableHeader: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "Id", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "FirstName", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "LastName", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "CoverUrl", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "ProfileUrl", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "DeviceToken", columnType: ColumnType.Number, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "StrCreated Dt.", columnType: ColumnType.Number, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "ProfileThumbUrl", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            //table.AddCell(_pdfCommonSettings.GetBodyCell(text: "CoverThumbUrl", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
            int i = 1;
            foreach (var item in users)
            {
                //var UserName =
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: i.ToString(),                                                                        columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.FullName != null) ? item.FullName.ToString() : "",                             columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.UserName != null) ? item.UserName.ToString() : "",                             columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.PhoneNumber != null) ? item.PhoneNumber.ToString() : "",                       columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.SaleQuota != null) ? item.SaleQuota.ToString() : "",                           columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.PurchaseQuota != null) ? item.PurchaseQuota.ToString() : "",                   columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.Email != null) ? item.Email.ToString() : "",                                   columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.TimeZone != null) ? item.TimeZone.ToString() : "",                             columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.Roles != null) ? item.Roles.ToString() : "",                                   columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.Description != null) ? item.Description.ToString() : "",                       columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.CreatedDate != null) ? item.CreatedDate.ToString() : "",                       columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.UserStatus != null) ? item.UserStatus.ToString() : "",                         columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.EmergencyContactName != null) ? item.EmergencyContactName.ToString() : "",     columnType: ColumnType.Number,    RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.EmergencyContactNumber != null) ? item.EmergencyContactNumber.ToString() : "", columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));
                table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.OfficePhoneNumber != null) ? item.OfficePhoneNumber.ToString() : "",           columnType: ColumnType.Number,  RemoveBorder: true, IsTableBody: true));

                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.Id != null) ? item.Id.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.FirstName != null) ? item.FirstName.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.LastName != null) ? item.LastName.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.CoverPictureUrl != null) ? item.CoverPictureUrl.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.ProfilePictureUrl != null) ? item.ProfilePictureUrl.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.DeviceToken != null) ? item.DeviceToken.ToString() : "", columnType: ColumnType.Number, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.StrCreatedDate != null) ? item.StrCreatedDate.ToString() : "", columnType: ColumnType.Number, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.ProfilePictureThumbUrl != null) ? item.ProfilePictureThumbUrl.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                //table.AddCell(_pdfCommonSettings.GetBodyCell(text: (item.CoverPictureThumbUrl != null) ? item.CoverPictureThumbUrl.ToString() : "", columnType: ColumnType.Text, RemoveBorder: true, IsTableBody: true));
                i++;
            }

            doc.Add(table);
        }


        private void PrintLine(Document doc)
        {
            LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
            doc.Add(line);
        }

        protected virtual void PrintHeader(Document doc, PdfWriter writer, string ReportName, string BarocdeID = null)
        {
            // Main table
            var mainTable = new PdfPTable(new float[] { 80F, 30F })
            {
                RunDirection = PdfWriter.RUN_DIRECTION_LTR,
                WidthPercentage = 100f
            };
            mainTable.SpacingBefore = 4f;
            mainTable.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Header Left
            PdfPCell leftTableCell = new PdfPCell();
            leftTableCell.Border = PdfPCell.NO_BORDER;
            //header
            var headerLeft = new PdfPTable(new float[] { 20F, 90F });
            headerLeft.WidthPercentage = 100f;
            headerLeft.SpacingAfter = 4f;
            headerLeft.DefaultCell.Border = Rectangle.NO_BORDER;

            //headerLeft.AddCell(_pdfCommonSettings.GetLogoCell());
            //doc.Add(headerLeft);

            var cellHeader = _pdfCommonSettings.GetHeaderCell("AEGIS IMPORT-EXPORT INC");
            cellHeader.MinimumHeight = 10;
            headerLeft.AddCell(cellHeader);

            var cellHeaderBody = _pdfCommonSettings.GetMainHeaderAddressCell("11305 N.W. 122nd st\nMEDLEY, FL 33178\nPhone:(305)883-1194/FAX:(305)883-5594\nEmail:markminors@dunblare.com");
            cellHeaderBody.Border = Rectangle.NO_BORDER;

            headerLeft.AddCell(cellHeaderBody);

            leftTableCell.AddElement(headerLeft);
            mainTable.AddCell(leftTableCell);
            #endregion

            #region Header Right
            PdfPCell rightTableCell = new PdfPCell();
            rightTableCell.Border = PdfPCell.NO_BORDER;
            rightTableCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            var headerRight = new PdfPTable(new float[] { 50F, 50F })
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                WidthPercentage = 100f
            };

            //headerRight.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headerRight.DefaultCell.VerticalAlignment = Element.ALIGN_RIGHT;
            //headerRight.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerRight.DefaultCell.Border = Rectangle.NO_BORDER;
            var ReportNameCell = _pdfCommonSettings.GetReportHeaderCell(ReportName);
            ReportNameCell.MinimumHeight = 25;
            headerRight.AddCell(ReportNameCell);
            if (!string.IsNullOrEmpty(BarocdeID))
            {
                headerRight.AddCell(_pdfCommonSettings.GetBarcodeCell(BarocdeID, writer));
                headerRight.AddCell(_pdfCommonSettings.GetReportHeaderCell("    "));
                headerRight.AddCell(_pdfCommonSettings.GetBodyCell(text: BarocdeID, columnType: ColumnType.Text, RemoveBorder: true, IsHeader: true, IsTransparentBackground: true));
                headerRight.AddCell(_pdfCommonSettings.GetReportHeaderCell("    "));
            }

            rightTableCell.AddElement(headerRight);
            mainTable.AddCell(rightTableCell);
            #endregion
            doc.Add(mainTable);
        }
    }
}

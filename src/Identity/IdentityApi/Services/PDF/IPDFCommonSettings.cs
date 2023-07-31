using iTextSharp.text;
using iTextSharp.text.pdf;

namespace IdentityApi.Services.PDF
{
    public interface IPDFCommonSettings
    {
        PdfPCell GetCell(string text, Font font);
        Phrase GetBodyCellItem(string text);
        PdfPCell GetMainHeaderAddressCell(string text);
        PdfPCell GetHeaderCell(string text);
        PdfPCell GetHeaderBodyCell(string text);
        PdfPCell GetBodyCell(string text, ColumnType columnType = ColumnType.Text, bool IsHeader = false, bool RemoveBorder = false, bool RemoveLeftBorder = false, bool RemoveRightBorder = false, bool RemoveBottomBorder = false, bool RemoveTopBorder = false, bool KeepTopBorder = false, bool IsTransparentBackground = false, bool IsTableHeader = false, bool IsDangerBody = false, bool IsTableBody = false, bool IsNormalBody = false);
        PdfPCell GetLogoCell();
        PdfPCell GetReportHeaderCell(string text, int Colspan = 2);
        iTextSharp.text.Image GetBarcodeCell(string Code, PdfWriter writer);


        PdfPCell GetCellWithColspan(string text, int columnSpan,

             bool IsDisclaimerText = false,
             bool RemoveBorder = true,
             bool KeepTopBorder = false,

              bool RemoveLeftBorder = false,
             bool RemoveRightBorder = false,
             bool RemoveBottomBorder = false,
             bool RemoveTopBorder = false,


             bool IsHeader = false,


            bool IsTransparentBackground = false,
             bool IsTableHeader = false,
             bool IsTableBody = false,
             ColumnType columnType = ColumnType.Text
            );

        PdfPCell GetCellWithRowspanAndColSpan(string text,
        int rowSpan,
        int colSpan,
        ColumnType columnType = ColumnType.Text,
        bool RemoveBorder = false,
        bool KeepTopBorder = false,
        //Bhawana (21/10/2019)
        bool IsHeader = false,
       bool IsTransparentBackground = false,
            bool IsTableHeader = false,
            bool IsTableBody = false);

        //PdfPCell GetCellWithColSpanAndRowSpan(string text);
        //PdfPCell GetCellWithColSpanAndRowSpan(string text, int colSpan, int rowSpan);

        //Task<PdfPTable> GetWarehouseReceiptInfo(WarehouseReceipt warehouseReceipt);

        #region Barcode Print
        PdfPCell GetCellForBarcodePrint(string text, int Size, int FontType, int Align = Element.ALIGN_LEFT, int colspan = 0, bool RemoveBorder = false,
             bool RemoveLeftBorder = false,
            bool RemoveRightBorder = false,
            bool RemoveBottomBorder = false,
            bool RemoveTopBorder = false,
            bool KeepTopBorder = false);
        #endregion

    }
}

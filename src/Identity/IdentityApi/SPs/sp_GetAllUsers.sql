CREATE PROCEDURE [dbo].[sp_GetAllUsers]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@Id NVARCHAR(MAX)
)
AS
BEGIN

			SELECT		
									ANU.Id									As Id,
									ANU.FirstName							As FirstName,
									ANU.LastName							As LastName,
									ANU.CoverPictureUrl				        As CoverPictureUrl,
									ANU.Title												As Title,
									ANU.City												As City,
									ANU.Email											As Email,
									ANU.OfficePhoneNumber			As OfficePhoneNumber,
									ANU.DirectPhoneNumber			As DirectPhoneNumber,
									ANU.NumberExtension					AS NumberExtension,
									ANU.PhoneNumber						As PhoneNumber,
									ANU.EmergencyContactName	As EmergencyContactName,
									ANU.EmergencyContactNumber	As EmergencyContactNumber,
									ANU.SaleCommissionRate				As SalesCommissionRate,
									ANU.PurchaseCommissionRate				As PurchaseCommissionRate,
									Convert(nvarchar,ANU.SaleQuota)											As SaleQuota,
									Convert(nvarchar,ANU.PurchaseQuota)									As PurchaseQuota

			FROM			[dbo].[AspNetUsers]				    As ANU
			Where			ANU.Id	= @Id

END
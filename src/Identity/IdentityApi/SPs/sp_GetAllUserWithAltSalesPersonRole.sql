CREATE PROCEDURE [dbo].[sp_GetAllUserWithAltSalesPersonRole]
AS
BEGIN

			SELECT				IsNull(ANU.Id,' ')							As Id,
										IsNull(Concat(ANU.FirstName, ' ' , ANU.LastName),' ')				    As Name,
										'AltSalesPerson'								as LabelName

			 FROM				[dbo].[AspNetUserRoles]				As ANUR
			INNER JOIN			AspNetRoles	As ANR on ANUR.RoleId = ANR.Id
			INNER JOIN			AspNetUsers	As ANU on ANU.Id = ANUR.UserId
			where				ANU.IsDeleted=0 and ANR.Name= 'SalesPerson' and ANU.UserStatusId=1
			 						
END
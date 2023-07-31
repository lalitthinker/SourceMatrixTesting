CREATE PROCEDURE [dbo].[sp_GetAllUserWithBuyersRole]
AS
BEGIN

			SELECT				IsNull(ANU.Id,' ')							As Id,
										IsNull(Concat(ANU.FirstName, ' ' , ANU.LastName),' ')				    As Name,
										'BuyerRep'								as LabelName

			 FROM				[dbo].[AspNetUserRoles]				As ANUR
			INNER JOIN			AspNetRoles	As ANR on ANUR.RoleId = ANR.Id
			INNER JOIN			AspNetUsers	As ANU on ANU.Id = ANUR.UserId
			where				ANU.IsDeleted=0 and ANR.Name= 'Buyers' and ANU.UserStatusId=1

END
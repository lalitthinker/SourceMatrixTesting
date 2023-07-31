CREATE OR ALTER PROCEDURE [dbo].[sp_GetUsersWithFullName]
AS
BEGIN

			SELECT				Distinct
										IsNull(ANU.Id,' ')							As Id,
										IsNull(ANU.FirstName,' ')							As FirstName,
										IsNull(ANU.LastName,' ')				As LastName

			 FROM				[dbo].[AspNetUserRoles]				As ANUR
			INNER JOIN			AspNetRoles	As ANR on ANUR.RoleId = ANR.Id
			INNER JOIN			AspNetUsers	As ANU on ANU.Id = ANUR.UserId
			where				ANU.IsDeleted=0 and ANU.UserStatusId=1
			 						
END
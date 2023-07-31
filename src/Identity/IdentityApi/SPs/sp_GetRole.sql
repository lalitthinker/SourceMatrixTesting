create PROCEDURE [dbo].[sp_GetRole]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@RoleName NVARCHAR(MAX) 
)
AS
BEGIN
	
			SELECT		
									ANR.Name									As RoleName,
									ANR.Id										As RoleId, 
									ANR.RoleColor								As RoleColor,
									ANUR.UserId									As UserId

			FROM				AspNetRoles					As ANR
			left JOIN			[dbo].[AspNetUserRoles]		As ANuR	on ANUR.RoleId = ANR.Id
			left JOIN			AspNetUsers					As AR	on AR.Id = ANuR.UserId
			

			WHERE				 (ANR.IsActive = 1)
			and (@RoleName = ANR.Name)

END
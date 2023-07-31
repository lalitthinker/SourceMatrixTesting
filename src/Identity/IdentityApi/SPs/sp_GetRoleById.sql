CREATE PROCEDURE [dbo].[sp_GetRoleById]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@RoleId NVARCHAR(MAX) 
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
			

			WHERE				(@RoleId = N'' OR @RoleId = ANR.Id)
								and (ANR.IsActive = 1)

END
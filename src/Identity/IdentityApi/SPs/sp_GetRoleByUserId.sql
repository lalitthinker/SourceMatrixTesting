CREATE PROCEDURE [dbo].[sp_GetRoleByUserId]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@UserId NVARCHAR(MAX) 
)
AS
BEGIN
	
			SELECT		ANR.Id										As RoleId

			FROM				AspNetRoles					As ANR
			left JOIN			[dbo].[AspNetUserRoles]		As ANUR	on ANUR.RoleId = ANR.Id
			left JOIN			AspNetUsers					As AR	on AR.Id = ANuR.UserId
			

			WHERE				(@UserId = N'' OR @UserId = AR.Id)
								and (ANR.IsActive = 1)

END
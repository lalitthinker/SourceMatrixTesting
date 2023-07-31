CREATE PROCEDURE [dbo].[sp_GetPrmissionNameById]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@RoleId NVARCHAR(MAX)
)
AS
BEGIN
	
			SELECT					CC.ClaimValue				As PermissionNamesList
									
			FROM				CustomClaims					As CC		
			full JOIN			MappingRoles					As MR	On  CC.Id = MR.CustomClaimsId
			
			WHERE				(@RoleId = MR.ApplicationRoleId)

END
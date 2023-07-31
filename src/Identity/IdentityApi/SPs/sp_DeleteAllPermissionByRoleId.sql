CREATE PROCEDURE [dbo].[sp_DeleteAllPermissionByRoleId]
(
@RoleId NVARCHAR(MAX)
)
AS
BEGIN

			Delete from [dbo].[MappingRoles]
			where ApplicationRoleId = @RoleId
			 						
END
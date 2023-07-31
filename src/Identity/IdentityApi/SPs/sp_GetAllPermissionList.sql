CREATE PROCEDURE [dbo].[sp_GetAllPermissionList]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@RoleId  nvarchar(max),
@SearchText NVARCHAR(MAX) = N'',
@GetAll BIT = 0
)
AS
BEGIN
		SET NOCOUNT ON
		SET TRANSACTION ISOLATION LEVEL READ COMMITTED		

		-- Set Constants
		DECLARE @ASC VARCHAR(8) = N'ASC';
		DECLARE @DESC VARCHAR(8) = N'DESC';

		-- Set default for SortDirection, if value is not either ASC or DESC:
		

		-- Create output table as Response of this stored procedure
		DECLARE @SpResponseData TABLE(
				Id											INT,
				ClaimTypeName				NVARCHAR(MAX),
				ClaimGroupName          NVARCHAR(MAX),
				ClaimValue						NVARCHAR(MAX),
		    	ClaimTypeId						INT,
			    ClaimGroupId					INT,
				IsAllowed							BIT,
				IsExpandable					BIT,
				IsEdited								BIT,
				PermissionSwitch			INT

		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			
			SELECT			CC.Id												As Id,
									CT.Name										As ClaimTypeName,
									CG.Name										AS ClaimGroupName,
									CC.ClaimValue							As ClaimValue,
									CC.ClaimTypeId							As ClaimTypeId,
									CC.ClaimGroupId						As ClaimGroupId,
									(select Count(*) from MappingRoles where  [CustomClaimsId] = CC.Id)    AS IsAllowed,
									(select Count(*) from MappingRoles where [CustomClaimsId] = CC.Id)    AS IsExpandable,
									(select Count(*) from MappingRoles where [CustomClaimsId] = CC.Id)    AS IsEdited,
									(select Count(*) from MappingRoles where [CustomClaimsId] = CC.Id)    AS PermissionSwitch


			FROM				CustomClaims 					As CC
								INNER JOIN ClaimTypes		    As CT					ON  CT.Id =CC.ClaimTypeId
								INNER JOIN ClaimGroups			As CG					ON  CG.Id= CC.ClaimGroupId
								INNER JOIN MappingRoles         As MR					ON  MR.CustomClaimsId = CC.Id		
			
			WHERE			
				 ((@SearchText = N'')		OR (CT.Name LIKE CONCAT('%',@SearchText ,'%'))	
											OR (CG.Name LIKE CONCAT('%',@SearchText ,'%'))
											OR (CC.ClaimValue LIKE CONCAT('%',@SearchText ,'%')))	
				and (@RoleId = ''			OR  @RoleId = MR.ApplicationRoleId)							
				
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination

		IF (@GetAll = 1)
			
		-- ordered result
		SELECT * FROM (SELECT * FROM @SpResponseData) AS Results

				order by Results.ClaimTypeName

END
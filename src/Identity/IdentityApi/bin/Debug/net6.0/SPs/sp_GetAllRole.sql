CREATE PROCEDURE [dbo].[sp_GetAllRole]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@Page INT = 1,
@PageSize INT = 10,
@SearchText NVARCHAR(MAX) = N'',
@GetAll BIT = 0,
--extra
@Userstatus    int=2
)
AS
BEGIN
		SET NOCOUNT ON
		SET TRANSACTION ISOLATION LEVEL READ COMMITTED		

		-- Set Constants
		DECLARE @ASC VARCHAR(8) = N'ASC';
		DECLARE @DESC VARCHAR(8) = N'DESC';

		-- Set default for SortDirection, if value is not either ASC or DESC:
		SET @SortDirection = UPPER(@SortDirection);
		--IF(@SortDirection NOT IN (@ASC, @DESC)) SET @SortDirection = @ASC

		-- Set defaults for PageSize and Page, if value is negative or zero:
		IF(@Page < 1) SET @Page = 1
		IF(@PageSize < 1) SET @PageSize = 10

		-- Create output table as Response of this stored procedure
		DECLARE @SpResponseData TABLE(
				RoleName					NVARCHAR(MAX),
				RoleId							NVARCHAR(MAX),
				RoleColor					NVARCHAR(MAX),
				IsSystemRole					BIT,
				IsActive						BIT,
				TotalUsers					NVARCHAR(MAX)
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
									ANR.Name								As RoleName,
									ANR.Id										As RoleId, 
									ANR.RoleColor						As RoleColor,
									ANR.IsSystemRole						As IsSystemRole,
									(Case  
							               When ANR.IsActive = 1 then(1)
										   When ANR.IsActive = 0 then(0)
			                               else (2) 
								           End)									As IsActive,
									COUNT(ANUR.UserId)			As TotalUsers 
									
																			

			FROM				AspNetRoles					As ANR
			left JOIN			AspNetUserRoles				As ANUR	on ANUR.RoleId = ANR.Id 
			left JOIN			AspNetUsers					As AR	on (AR.Id = ANuR.UserId and AR.IsDeleted = 0)
			
			WHERE		(@Userstatus = ANR.IsActive or @Userstatus = 2)
						--and AR.IsDeleted = 0
			and				((@SearchText = N'')		OR (ANR.Name LIKE CONCAT('%',@SearchText ,'%')))	
								--((@SearchText = N'')		OR (@SearchText = ANR.RoleColor))	
							

			GROUP BY	ANR.Name,ANR.Id,ANR.IsActive,ANR.RoleColor,ANR.IsSystemRole
			
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows			INT = 0;
		DECLARE @TotalPages		INT = 0;
		DECLARE @Offset					INT = 0;
		DECLARE @NextCount		INT = 0;
		DECLARE @AllUsersCount  INT = 0;
		
		SET @AllUsersCount		 = (Select sum(convert(int,TotalUsers)) from @SpResponseData);
		SET @TotalRows				 = (SELECT COUNT(1) from @SpResponseData);
		SET @TotalPages				 = IIF(@TotalRows % @PageSize = 0, (@TotalRows / @PageSize), (@TotalRows / @PageSize) + 1);

		IF (@GetAll = 0)
			BEGIN				
				SET @Offset = @PageSize * (@Page - 1);
				SET @NextCount = @PageSize;
			END
		ELSE
			BEGIN
				SET @Offset = 0;
				SET @NextCount = @TotalRows;
			END

		-- ordered result
		SELECT * FROM (SELECT *, @TotalRows as [TotalRecords], @AllUsersCount as [AllUsers] FROM @SpResponseData) AS Results

		ORDER BY Results.RoleName ASC
		
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName'		+ @ASC		THEN Results.RoleName			END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName '		+ @DESC	THEN Results.RoleName			END  DESC

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleColor'			+ @ASC		THEN Results.RoleColor			END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleColor'			+ @DESC	THEN Results.RoleColor			END	 DESC
							
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
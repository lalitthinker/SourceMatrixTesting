CREATE PROCEDURE [dbo].[sp_GetAllUserRoleCount]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@Page INT = 1,
@PageSize INT = 10,
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
		SET @SortDirection = UPPER(@SortDirection);
		IF(@SortDirection NOT IN (@ASC, @DESC)) SET @SortDirection = @ASC

		-- Set defaults for PageSize and Page, if value is negative or zero:
		IF(@Page < 1) SET @Page = 1
		IF(@PageSize < 1) SET @PageSize = 10

		-- Create output table as Response of this stored procedure
		DECLARE @SpResponseData TABLE(
				RoleName					NVARCHAR(MAX),
				RoleId						NVARCHAR(MAX),
				RoleColor					NVARCHAR(MAX),
				IsActive					BIT,
				IsSystemRole					BIT,
				TotalUsers					NVARCHAR(MAX)
				

		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
									ANR.NormalizedName					As RoleName,
									ANR.Id								As RoleId, 
									ANR.RoleColor						As RoleColor,
									ANR.IsActive						As IsActive,
									ANR.IsSystemRole					    As IsSystemRole,
									COUNT(ANUR.UserId)					As TotalUsers 



			FROM				AspNetRoles	As ANR
			left JOIN			[dbo].[AspNetUserRoles]				As ANuR on ANUR.RoleId = ANR.Id
			left JOIN			AspNetUsers	As AR on AR.Id = ANuR.UserId
			where				AR.IsDeleted=0 and  ANR.IsActive=1 
			GROUP BY			ANR.NormalizedName,ANR.Id, ANR.RoleColor, ANR.IsActive	, ANR.IsSystemRole
			

        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows		 INT = 0;
		DECLARE @TotalPages		 INT = 0;
		DECLARE @Offset				 INT = 0;
		DECLARE @NextCount		 INT = 0;
		DECLARE @AllUsersCount  INT = 0;
		
		--SET @AllUsersCount		 = (Select SUM(CONVERT(int,TotalUsers)) from @SpResponseData);
		SET @AllUsersCount		 = (Select Count(*) from AspNetUsers 
												left join AspNetUserRoles on AspNetUserRoles.UserId = AspNetUsers.Id
												left join AspNetRoles on AspNetUserRoles.RoleId = AspNetRoles.Id
												where IsDeleted = 0 and AspNetRoles.IsActive = 1);
		SET @TotalRows				 = (SELECT COUNT(1) from @SpResponseData);
		SET @TotalPages			 = IIF(@TotalRows % @PageSize = 0, (@TotalRows / @PageSize), (@TotalRows / @PageSize) + 1);

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

		ORDER BY
		
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName'			+ @ASC		THEN Results.RoleName			END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName '			+ @DESC		THEN Results.RoleName			END  DESC
							
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
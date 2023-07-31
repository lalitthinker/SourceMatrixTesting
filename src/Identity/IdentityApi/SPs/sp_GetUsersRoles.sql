Create PROCEDURE [dbo].[sp_GetUsersRoles]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@Page INT = 1,
@PageSize INT = 10,
@SearchText NVARCHAR(MAX) = N'',
@GetAll BIT = 0
--@UsersId NVARCHAR(MAX)
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
				UserId							NVARCHAR(MAX),			
				RoleName					NVARCHAR(MAX),
				RoleColor                      	NVARCHAR(MAX)
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		Distinct(ANUR.UserId)				As UserId,
								ANR.Name									As RoleName,
								ANR.RoleColor                      As RoleColor


			FROM			[dbo].[AspNetUserRoles]		As ANUR
			left JOIN		[dbo].[AspNetUsers]				As ANU on ANU.Id = ANUR.UserId
			full join		[dbo].[AspNetRoles]				As ANR on ANR.Id = ANUR.RoleId
			WHERE		((@SearchText = N'')				OR (ANR.Name like concat('%',@SearchText,'%')))
						--and (ANR.IsActive = 1)
			group by	ANUR.UserId , ANR.Name,	ANR.RoleColor 

        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows			INT = 0;
		DECLARE @TotalPages		INT = 0;
		DECLARE @Offset					INT = 0;
		DECLARE @NextCount		INT = 0;
		
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
		SELECT * FROM (SELECT *, @TotalRows as [TotalRecords] FROM @SpResponseData) AS Results

		ORDER BY
		
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName'		+ @ASC		THEN Results.RoleName			END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'RoleName'		+ @DESC	THEN Results.RoleName			END  DESC
							
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
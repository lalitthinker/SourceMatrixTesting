CREATE PROCEDURE [dbo].[sp_GetAllFavoriteDock]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@Page INT = 1,
@PageSize INT = 10,
@SearchText NVARCHAR(MAX) = N'',
@GetAll BIT = 0,
@UserId NVARCHAR(MAX)
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
				Id						BIGINT,
				UserId					NVARCHAR(MAX),
				IconId                    int,
				IsPinned                 bit,
				IsDeleted                bit,
				ExternalId				NVARCHAR(MAX)

		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
							FD.Id						As Id,
							FD.UserId					As UserId,
							FD.IconId                   As IconId,
							FD.IsPinned                 As IsPinned,
							FD.IsDeleted                As IsDeleted,
							ISNULL(FD.ExternalId,'')	as ExternalId

			FROM			[dbo].[FavoriteDocks]	As FD

			WHERE			(@UserId = ''				OR  @UserId = FD.UserId)	AND
			                FD.IsDeleted = 0 
							--((@SearchText = N'')		OR (@SearchText = TC.primaryMenus))				OR
							--((@SearchText = N'')		OR (@SearchText = TC.secondaryMenus))			OR
							--((@SearchText = N'')		OR (@SearchText = TC.favoritesDocks))			OR
							--((@SearchText = N'')		OR (@SearchText = TC.siteWides))		
							
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows		INT = 0;
		DECLARE @TotalPages		INT = 0;
		DECLARE @Offset			INT = 0;
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
		
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'IconId'		+ @ASC		THEN Results.IconId	END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'IconId'		+ @DESC		THEN Results.IconId	END  DESC
				
				
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
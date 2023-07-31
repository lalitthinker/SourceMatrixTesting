CREATE PROCEDURE [dbo].[sp_GetAllThemeCustomization]
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
				PrimaryMenu				NVARCHAR(MAX),
				SecondaryMenu			NVARCHAR(MAX),
				FavoriteDock			NVARCHAR(MAX),
				SiteWides				NVARCHAR(MAX),
				ExternalId				NVARCHAR(MAX)
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
							TC.Id						As Id,
							TC.UserId					As UserId,
							TC.PrimaryMenus				As PrimaryMenu,
							TC.SecondaryMenus			As SecondaryMenu,
							TC.FavoriteDocks			As FavoriteDock,
							TC.SiteWides				As SiteWides,
							isnull(TC.ExternalId,'')    as ExternalId
							

			FROM			[dbo].[ThemeCustomizations]	As TC

			WHERE			(@UserId = ''				OR  @UserId = TC.UserId)	
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
		
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'PrimaryMenu'		+ @ASC		THEN Results.PrimaryMenu	END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'PrimaryMenu'		+ @DESC		THEN Results.PrimaryMenu	END  DESC,
				
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'SecondaryMenu'		+ @ASC		THEN Results.SecondaryMenu	END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'SecondaryMenu'		+ @DESC		THEN Results.SecondaryMenu	END  DESC,

				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FavoriteDock'		+ @ASC		THEN Results.FavoriteDock	END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FavoriteDock'		+ @DESC		THEN Results.FavoriteDock	END  DESC,

				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'SiteWides'			+ @ASC		THEN Results.SiteWides			END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'SiteWides '		+ @DESC		THEN Results.SiteWides			END  DESC

		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
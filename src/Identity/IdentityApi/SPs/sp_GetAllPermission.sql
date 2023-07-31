CREATE PROCEDURE [dbo].[sp_GetAllPermission]
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
				Id									BIGINT,
				ClaimTypeName			NVARCHAR(MAX),
				ClaimGroupName			NVARCHAR(MAX),
				ClaimValue					NVARCHAR(MAX)
				);

		
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (
			SELECT			CC.Id										As Id,
									IsNull(CT.Name,0)				    As ClaimTypeName,
									CG.Name									As ClaimGroupName,
									CC.ClaimValue							As ClaimValue


			FROM				CustomClaims 							As CC
			INNER JOIN  ClaimTypes		     					As CT		ON  CT.Id =CC.ClaimTypeId
			INNER JOIN  ClaimGroups								As CG		ON  CG.Id= CC.ClaimGroupId
					    
					       -- 	Filter using search text
			WHERE		  ((@SearchText = N'')				OR (CC.ClaimValue like concat('%',@SearchText,'%')))
				
	     --   ORDER BY CG.Name ASC	
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows	INT = 0;
		DECLARE @TotalPages	INT = 0;
		DECLARE @Offset		INT = 0;
		DECLARE @NextCount	INT = 0;

		SET @TotalRows = (SELECT COUNT(1) from @SpResponseData);
		SET @TotalPages = IIF(@TotalRows % @PageSize = 0, (@TotalRows / @PageSize), (@TotalRows / @PageSize) + 1);

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
		Results.ClaimTypeName ASC,
		Results.ClaimGroupName ASC,
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'ClaimGroupName'		+ @ASC		THEN Results.ClaimGroupName			END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'ClaimGroupName '		+ @DESC		THEN Results.ClaimGroupName			END  DESC,
			
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Name '						+ @ASC		THEN Results.Id									END  ASC,
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Name '						+ @DESC		THEN Results.Id									END  DESC
							
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END

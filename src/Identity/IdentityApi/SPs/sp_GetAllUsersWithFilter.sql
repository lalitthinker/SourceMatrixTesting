CREATE PROCEDURE [dbo].[sp_GetAllUsersWithFilter]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@PageSize INT ,
@Page INT ,
@SearchText NVARCHAR(MAX) = N'',
@Date NVARCHAR(MAX) = N'',
@GetAll BIT = 0,
@Id NVARCHAR(MAX) = N'',
--extra
@Userstatus    int= 2
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
				Id									NVARCHAR(MAX),
				FirstName					NVARCHAR(MAX),
				LastName					NVARCHAR(MAX),
				UserName					NVARCHAR(MAX),
				CoverPictureUrl				NVARCHAR(MAX),
				ProfilePictureUrl			NVARCHAR(MAX),
				PhoneNumber					NVARCHAR(MAX),
				Email						NVARCHAR(MAX),
				TimeZone					NVARCHAR(MAX),
				DeviceToken					NVARCHAR(MAX),
				Description					NVARCHAR(MAX),
				SaleQuota					NVARCHAR(MAX),
				PurchaseQuota				NVARCHAR(MAX),
				CreatedDate					nvarchar(20),
				EmergencyContactNumber		NVARCHAR(MAX),
				EmergencyContactName		NVARCHAR(MAX),
				OfficePhoneNumber			NVARCHAR(MAX),
				IsChecked					BIT,
				Title								NVARCHAR(MAX),
				City								NVARCHAR(MAX),
				DirectPhoneNumber			NVARCHAR(MAX),
				NumberExtension					NVARCHAR(MAX),
				--UserStatusId				int,
				UserStatus					BIT,
				ImageZoomRatio	INT,
				IsDeleted                   BIT,
				SalesCommissionRate		NVARCHAR(MAX),
				PurchaseCommissionRate NVARCHAR(MAX)

		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		Distinct
									ANU.Id										As Id,
									ANU.FirstName								As FirstName,
									ANU.LastName								As LastName,
									ANU.UserName								As UserName,
									ANU.CoverPictureUrl							As CoverPictureUrl,
									ANU.ProfilePictureUrl						As ProfilePictureUrl,
									ANU.PhoneNumber								As PhoneNumber,
									ANU.Email									As Email,
									ANU.TimeZone								As TimeZone,
									ANU.DeviceToken								As DeviceToken,
									ANU.Description								As Description,
									Convert(nvarchar, ANU.SaleQuota)			As SaleQuota,
									Convert(nvarchar,ANU.PurchaseQuota)			As PurchaseQuota,
									convert(nvarchar, ANU.CreatedDate, 103)								As CreatedDate,
									ANU.EmergencyContactNumber					As EmergencyContactNumber,
									ANU.EmergencyContactName					As EmergencyContactName,
									ANU.OfficePhoneNumber						As OfficePhoneNumber,
									(select Count(*) from [dbo].[AspNetUsers] 
									where Id = @Id )							AS IsChecked,
									ANU.Title									As Title,
									ANU.City									As City,
									ANU.DirectPhoneNumber			As DirectPhoneNumber,
									ANU.NumberExtension					AS NumberExtension,
									(Case  
							               When ANU.UserStatusId = 1 then(1)
										   When ANU.UserStatusId = 0 then(0)
			                               else (2) 
								           End)                                  As UserStatus,
										   ANU.ImageZoomRatio					As ImageZoomRatio,
										   ANU.IsDeleted                         As IsDeleted,
										   ANU.SaleCommissionRate			As SaleCommissionRate,
										   ANU.PurchaseCommissionRate		As PurchaseCommissionRate
									

			FROM				[dbo].[AspNetUsers]				As ANU
			left JOIN		[dbo].[AspNetUserRoles]		As ANUR ON ANUR.UserId = ANU.Id
			FULL JOIN		[dbo].[AspNetRoles]				As ANR	ON ANR.Id = ANUR.RoleId

			WHERE			((@Date = N'')		OR (@Date = Convert(nvarchar ,ANU.CreatedDate ,103)))
			AND			(@SearchText = N''	OR	ANU.FirstName LIKE CONCAT('%',@SearchText ,'%') OR ANU.LastName LIKE CONCAT('%',@SearchText ,'%'))			
							AND ANU.IsDeleted = 0 
							and ANR.IsActive = 1
							AND (@Id = ANU.Id OR @Id = '')		
							AND (@Userstatus = ANU.UserStatusId or @Userstatus = 2)
							
        )
		INSERT INTO	@SpResponseData 	SELECT * FROM ListData;

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

		ORDER BY Results.UserName ASC
				
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FirstName'				+ @ASC			THEN Results.FirstName				END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FirstName'				+ @DESC		THEN Results.FirstName				END  DESC
				
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'LastName'				+ @ASC			THEN Results.LastName				END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'LastName'				+ @DESC		THEN Results.LastName				END  DESC

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'CoverPictureUrl'		+ @ASC			THEN Results.CoverPictureUrl	END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'CoverPictureUrl '		+ @DESC		THEN Results.CoverPictureUrl	END  DESC,
					
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'ProfilePictureUrl'		+ @ASC			THEN Results.ProfilePictureUrl	END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'ProfilePictureUrl '		+ @DESC		THEN Results.ProfilePictureUrl	END  DESC,

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'PhoneNumber'		+ @ASC			THEN Results.PhoneNumber		END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'PhoneNumber'		+ @DESC		THEN Results.PhoneNumber		END  DESC,

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Email'							+ @ASC			THEN Results.Email						END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Email'							+ @DESC		THEN Results.Email						END  DESC,

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'TimeZone'					+ @ASC			THEN Results.TimeZone				END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'TimeZone'					+ @DESC		THEN Results.TimeZone				END  DESC,

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'DeviceToken'			+ @ASC			THEN Results.DeviceToken			END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'DeviceToken'			+ @DESC		THEN Results.DeviceToken			END  DESC,

				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Description'				+ @ASC			THEN Results.Description			END  ASC, 
				--CASE WHEN @SortColumn + ' ' + @SortDirection	=	'Description'				+ @DESC		THEN Results.Description			END  DESC

						

		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
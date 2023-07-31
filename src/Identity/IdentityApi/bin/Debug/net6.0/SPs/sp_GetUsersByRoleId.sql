CREATE PROCEDURE [dbo].[sp_GetUsersByRoleId]
(
/**Declare common parameters for pagination and search filter and set defaults*/
@Id NVARCHAR(MAX) = N'',

@SortColumn VARCHAR(MAX) = N'Id',
@SortDirection VARCHAR(8) = N'DESC',
@Page INT = 1,
@PageSize INT = 10,
@SearchText NVARCHAR(MAX) = N'',
@GetAll BIT = 0,
@RoleId VARCHAR(MAX),
@Date NVARCHAR(MAX) = N'',
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
		IF(@SortDirection NOT IN (@ASC, @DESC)) SET @SortDirection = @ASC

		-- Set defaults for PageSize and Page, if value is negative or zero:
		IF(@Page < 1) SET @Page = 1
		IF(@PageSize < 1) SET @PageSize = 10

		-- Create output table as Response of this stored procedure
		DECLARE @SpResponseData TABLE(
				Id							NVARCHAR(MAX),
				--City								NVARCHAR(MAX),
				--State							NVARCHAR(MAX),
				--Country						NVARCHAR(MAX),
				--CreatedOn					DATETIME2,
				FirstName					NVARCHAR(MAX),
				LastName					NVARCHAR(MAX),
				UserName					NVARCHAR(MAX),
				CoverPictureUrl				NVARCHAR(MAX),
				PhoneNumber			NVARCHAR(MAX),
				CreatedDate			nvarchar(20),
				Description			NVARCHAR(MAX),
				DeviceToken			NVARCHAR(MAX),
				Email				NVARCHAR(MAX),
				ProfilePictureUrl	NVARCHAR(MAX),
				TimeZone			NVARCHAR(MAX),
				UserRole			NVARCHAR(MAX),
				--UserStatusId		INT,
				EmergencyContactNumber	NVARCHAR(MAX),
				EmergencyContactName	NVARCHAR(MAX),
				OfficePhoneNumber		NVARCHAR(MAX),
				PurchaseQuota			NVARCHAR(MAX),
				SaleQuota				NVARCHAR(MAX),
				IsChecked               bit,
				Title								NVARCHAR(MAX),
				City								NVARCHAR(MAX),
				DirectPhoneNumber			NVARCHAR(MAX),
				NumberExtension					NVARCHAR(MAX),
				UserStatus				bit,
				ImageZoomRatio	INT,
				IsDeleted               bit,
				SalesCommissionRate		NVARCHAR(MAX),
				PurchaseCommissionRate NVARCHAR(MAX)
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT			DISTINCT
									ANU.Id											As Id,
									--ANU.City										As City,
									--ANU.State									As State,
									--ANU.Country								As Country,
									--ANU.CreatedDate						As CreatedOn,
									ANU.FirstName							As FirstName,
									ANU.LastName							As LastName,
									ANU.UserName							As UserName,
									ANU.CoverPictureUrl						As CoverPictureUrl,
									ANU.PhoneNumber					As PhoneNumber,
									convert(nvarchar, ANU.CreatedDate, 103)					As CreatedDate,
									ANU.Description					As Description,
									ANU.DeviceToken					As DeviceToken,
									ANU.Email						As Email,
									ANU.ProfilePictureUrl			As ProfilePictureUrl,
									ANU.TimeZone					As TimeZone,
									ANU.UserRole					As UserRole,
									--ANU.UserStatusId				As UserStatusId,
									ANU.EmergencyContactName		As EmergencyContactName,
									ANU.EmergencyContactNumber		As EmergencyContactNumber,
									ANU.OfficePhoneNumber			As OfficePhoneNumber,
									Convert(nvarchar,ANU.PurchaseQuota)				As PurchaseQuota,
									Convert(nvarchar, ANU.SaleQuota)				As SaleQuota,
									(select Count(*) from [dbo].[AspNetUsers] where Id = @Id )    AS IsChecked,
									ANU.Title									As Title,
									ANU.City										As City,
									ANU.DirectPhoneNumber			As DirectPhoneNumber,
									ANU.NumberExtension					AS NumberExtension,
									(Case  
							               When ANU.UserStatusId = 1 then(1)
										   When ANU.UserStatusId = 0 then(0)
			                               else (2) 
								           End)                                          As UserStatus,
									ANU.ImageZoomRatio					As ImageZoomRatio,
									ANU.IsDeleted                                   As IsDeleted,
										   ANU.SaleCommissionRate			As SaleCommissionRate,
										   ANU.PurchaseCommissionRate		As PurchaseCommissionRate
									
									
			FROM				[dbo].[AspNetUsers]					As ANU 
			INNER JOIN			[dbo].[AspNetUserRoles]				As ANUR ON ANUR.RoleId = @RoleId
			WHERE				((@Date = N'')						OR (@Date = Convert(nvarchar ,ANU.CreatedDate ,103)))
								AND ((@SearchText = N'') OR (ANU.FirstName LIKE CONCAT('%',@SearchText,'%')) OR (ANU.LastName LIKE CONCAT('%',@SearchText,'%')))
								AND	ANU.Id = ANUR.UserId 
								AND ANU.IsDeleted = 0
								and (@Userstatus = ANU.UserStatusId or @Userstatus = 2)
								

        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		DECLARE @TotalRows		INT = 0;
		DECLARE @TotalPages		INT = 0;
		DECLARE @Offset			INT = 0;
		DECLARE @NextCount		INT = 0;
		
		SET @TotalRows			= (SELECT COUNT(1) from @SpResponseData);
		SET @TotalPages			= IIF(@TotalRows % @PageSize = 0, (@TotalRows / @PageSize), (@TotalRows / @PageSize) + 1);

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
		
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FirstName'		+ @ASC		THEN Results.FirstName		END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'FirstName '		+ @DESC		THEN Results.FirstName		END  DESC,

				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'LastName'		+ @ASC		THEN Results.LastName			END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'LastName '		+ @DESC		THEN Results.LastName			END  DESC,

				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'UserName'		+ @ASC		THEN Results.UserName		END  ASC, 
				CASE WHEN @SortColumn + ' ' + @SortDirection	=	'UserName '	+ @DESC		THEN Results.UserName		END  DESC
							
		OFFSET @Offset ROWS
			FETCH NEXT @NextCount ROWS ONLY OPTION (RECOMPILE)
		
END
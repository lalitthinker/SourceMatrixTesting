CREATE PROCEDURE [dbo].[sp_GetAllClaimType]
(
/**Declare common parameters for pagination and search filter and set defaults*/

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
				Id								INT,
				Name						NVARCHAR(MAX)
			
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
								CT.Id									As Id,
								CT.Name							As Name

			FROM			[dbo].[ClaimTypes]		As CT
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		IF (@GetAll = 1)
			
		-- ordered result
		SELECT * FROM (SELECT * FROM @SpResponseData) AS Results

		ORDER BY
		
			Results.Name
					
END

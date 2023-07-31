CREATE PROCEDURE [dbo].[sp_GetAllClaimValues]
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
				Id											INT,
				ClaimGroupId					INT,
				ClaimValue						NVARCHAR(MAX)
			
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			SELECT		
								CC.Id										As   Id,
								CC.ClaimGroupId				As   ClaimGroupId,
								CC.ClaimValue					As   ClaimValue

			FROM			[dbo].[CustomClaims]		As CC
			
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination

		IF (@GetAll = 1)
			
		-- ordered result
		SELECT * FROM (SELECT * FROM @SpResponseData) AS Results

		ORDER BY
		
			Results.ClaimGroupId
		
END

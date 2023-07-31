CREATE PROCEDURE [dbo].[sp_GetAllClaimGroups]
(
/**Declare common parameters for pagination and search filter and set defaults*/

@GetAll BIT = 0
)
AS
BEGIN
		SET NOCOUNT ON
		SET TRANSACTION ISOLATION LEVEL READ COMMITTED		

		-- Set Constants
		DECLARE @ASC VARCHAR(8)		= N'ASC';
		DECLARE @DESC VARCHAR(8)		= N'DESC';

		-- Set default for SortDirection, if value is not either ASC or DESC:
		-- Create output table as Response of this stored procedure
		DECLARE @SpResponseData TABLE(
				Id											INT,
				ClaimTypeName				NVARCHAR(MAX),
				ClaimGroupName			NVARCHAR(MAX),
				ClaimValue						NVARCHAR(MAX)
			
		);
        -- Get data from database table and then insert that data into @SpResponseData table
		WITH ListData AS (

			
			SELECT			CC.Id												As Id,
									CT.Name										As ClaimTypeName,
									CG.Name										AS ClaimGroupName,
									CC.ClaimValue							As ClaimValue


			FROM				CustomClaims 							As CC
									INNER JOIN ClaimTypes		     	As CT		ON  CT.Id =CC.ClaimTypeId
									INNER JOIN ClaimGroups			As CG		ON  CG.Id= CC.ClaimGroupId
					    
        )
		INSERT INTO	@SpResponseData	SELECT * FROM ListData;

		-- pagination
		IF (@GetAll = 1)
			
		-- ordered result
		SELECT * FROM (SELECT * FROM @SpResponseData) AS Results

END

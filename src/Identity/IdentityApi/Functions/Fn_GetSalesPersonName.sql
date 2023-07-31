CREATE Function [dbo].[GetSalesPersonName]  
(	
	 @UserId  varchar(max) =''
)
RETURNS TABLE 
AS
RETURN 
(
	(select     
                      U.FirstName	as FirstName,
					  U.LastName	as LastName
						
                      from AspNetUsers U 
where 	U.Id=  @Userid)
)
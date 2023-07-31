CREATE PROCEDURE [dbo].[sp_SetSaleCommission]
	@SaleCommissionId varchar(50),
	@UserIds varchar(1024)
AS
BEGIN
	
	--First, take the string of company Ids and parse them into a table

	select ListItem into #tmp from [dbo].[GetValueListTable](@UserIds,',')

	if exists(select * from #tmp)
	begin
		
		update AspNetUsers set [SaleCommissionRate] = @SaleCommissionId
		from AspNetUsers 
		inner join #tmp on AspNetUsers.Id = #tmp.ListItem

	end

END

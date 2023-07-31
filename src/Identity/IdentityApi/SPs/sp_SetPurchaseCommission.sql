CREATE PROCEDURE [dbo].[sp_SetPurchaseCommission]
	@PurchaseCommissionId varchar(50),
	@UserIds varchar(1024)
AS
BEGIN
	
	--First, take the string of company Ids and parse them into a table

	select ListItem into #tmp from [dbo].[GetValueListTable](@UserIds,',')

	if exists(select * from #tmp)
	begin
		
		update AspNetUsers set [PurchaseCommissionRate] = @PurchaseCommissionId
		from AspNetUsers 
		inner join #tmp on AspNetUsers.Id = #tmp.ListItem

	end

END

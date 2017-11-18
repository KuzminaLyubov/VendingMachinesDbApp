
declare @year int = 2017, @month int = 11 

-- terminals with one product missingTerminals with one product missing
select                     
t.Location,
p.Name,
tp.Quantity
 from vm.TerminalsProducts tp
 inner join vm.Terminals t on t.Id = tp.TerminalId
 inner join vm.Products p on p.Id = tp.ProductId
where Quantity = 0

-- 5 least sold products
select top 5 p.Name, Count(*) from vm.SoldProducts s
 inner join vm.Products p on p.Id = s.ProductId
where year(s.Date) = @year and (@month = 0 or month(s.Date) = @month)
group by p.Name
order by Count(*)
                
-- check profits
;with cte as (
select 
	t.Location, 
	profit = (select top 1 SellingPrice-PurchasePrice 
	from vm.ProductPrices p 
	where p.ProductId = s.ProductId 
	and p.Date<=s.Date order by p.Date desc)
from vm.SoldProducts s
inner join vm.Terminals t on s.TerminalId = t.Id
where year(s.Date) = @year and (@month = 0 or month(s.Date) = @month)
)
select Location, Sum(profit) from cte
group by Location
order by Sum(profit) desc
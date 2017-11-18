declare @FromDate date = '2016-01-01'
declare @ToDate date = '2017-11-14'

declare @id1 int, @id2 int, @i int = 20

declare vendor_cursor cursor for   

select ProductId, TerminalId, Quantity from vm.TerminalsProducts

open vendor_cursor  

fetch next from vendor_cursor   
into @id1, @id2, @i  

while @@fetch_status = 0  
begin  

    while (0 <= (select 20 - @i))
    begin
		--- generate transactions for random dates
		insert into VM.SoldProducts (ProductId, TerminalId, Date)
		values  (@id1,@id2,dateadd(day, rand(checksum(newid()))*(1+datediff(day, @FromDate, @ToDate)), @FromDate) )
        set @i = @i + 1

    end
    fetch next from vendor_cursor   
    into @id1, @id2, @i
end   
close vendor_cursor;  
deallocate vendor_cursor;  

select * from vm.SoldProducts

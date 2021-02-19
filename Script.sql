Create Database MedixTestt
Go
Use MedixTestt
go
Create Table Clients (
	ClientId int primary key identity (1,1),
	Name nvarchar(50),
	Phone nvarchar(50)
)
go
Create Table States (
	StateId int primary key identity (1,1),
	Code nvarchar(5),
	Name nvarchar(50)
)
go
Create Table Status (
	StatusId int primary key identity (1,1),
	Name nvarchar(50)
)
go

Create Table WorkOrders (
	WorkOrderId int primary key identity (1,1),
	WorkOrderNumber nvarchar(50),
	CreatedDate smalldatetime default(GetDAte()),
	ClientId int references Clients(ClientId),
	StateId int references States(StateId),
	StatusId int references Status(StatusId),
	ETA smalldatetime
)

insert into Clients values ('Client1', 'Phone1')
go
insert into Clients values ('Client2', 'Phone2')
go
insert into States values ('St1', 'Code1')
go
insert into States values ('St2', 'Code2')
go
insert into States values ('St3', 'Code3')
go
insert into Status values ('Status1')
go
insert into Status values ('Status2')
go
insert into Status values ('Status3')
go

Create Procedure Sp_GetClients
as
begin
	Select ClientId, Name, Phone From Clients Order By Name
end

go

Create Procedure Sp_GetStates
as
begin
	Select StateId, Code, Name From States Order By Code
end

go

Create Procedure Sp_GetStatus
as
begin
	Select StatusId, Name From Status Order By Name
end

go

Create Procedure Sp_GetWorkOrders
as
begin
	Select WorkOrderId, WorkOrderNumber, CreatedDate, ETA, C.Name as ClientName, C.ClientId, S.StateId, St.StatusId, St.Name as StatusName
	From WorkOrders Wo Join Clients C On Wo.ClientId = C.ClientId
	Join States S On S.StateId = Wo.StateId
	Join Status ST On St.StatusId = Wo.StatusId
	Order By ETA desc
end

go

Create Procedure Sp_GetWorkOrderItem
(
	@WorkOrderId int
)
as
begin
	Select WorkOrderId, WorkOrderNumber, CreatedDate, ETA, C.Name as ClientName, C.ClientId, S.StateId, St.StatusId, St.Name as StatusName
	From WorkOrders Wo Join Clients C On Wo.ClientId = C.ClientId
	Join States S On S.StateId = Wo.StateId
	Join Status ST On St.StatusId = Wo.StatusId
	Where WorkOrderId=@WorkOrderId
	Order By ETA desc
end

Go

Create Procedure Sp_InsertWorkOrder
(
	@WorkOrderNumber nvarchar(50),
	@ClientId int,
	@StateId int,
	@StatusId int,
	@ETA smalldatetime
)
as
begin
	Insert into WorkOrders (WorkOrderNumber, ClientId, StateId, StatusId, ETA) values (@WorkOrderNumber, 
	@ClientId, @StateId, @StatusId, @ETA)
end	

Go

Create Procedure Sp_GetWorkOrderCount
as
begin
	Select COUNT(*) From WorkOrders
end

Go

Create Procedure Sp_UpdateWorkOrderOrder
(
	@WorkOrderId int,	
	@ClientId int,
	@StateId int,
	@StatusId int,
	@ETA smalldatetime
)
as
begin
	Update WorkOrders 
	Set ClientId = @ClientId, StateId = @StateId, StatusId = @StatusId
	Where WorkOrderId = @WorkOrderId
end

Go

Create Procedure Sp_DeleteWorkOrderOrder
(
	@WorkOrderId int
)
as
begin
	Delete From WorkOrders Where WorkOrderId = @WorkOrderId
end
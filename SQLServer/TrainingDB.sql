create database Training
go

use Training
go

create table tStudents(
	aStudentID int primary key identity(1,1),
	aUsername varchar(100),
	aFullName nvarchar(255),
	aEmail varchar(100),
	aCreatedDate datetime,
	aUpdatedDate datetime
)
go

create table tSchedule(
	aScheduleID int primary key identity(1,1),
	aStudentID int foreign key references tStudents(aStudentID),
	aAttendedDate date,
	aSession int,
	aCreatedDate datetime,
	aUpdateDate datetime
)
go

insert into tStudents values('fitthuctap20',N'Trần Quốc Đạt','tranquocdataa@gmail.com',CURRENT_TIMESTAMP,null)
go

--tạo index cho 3 cột aUsername(tStudents), aAttendedDate, aSession (tSchedule)

if exists (select name from sys.indexes where name = N'IX_aUsername')
drop index IX_aUsername on tStudents
go
create nonclustered index IX_aUsername on tStudents(aUsername)
go

if exists (select name from sys.indexes where name = N'IX_aAttendedDate')
drop index IX_aAttendedDate on tSchedule
go
create nonclustered index IX_aAttendedDate on tSchedule(aAttendedDate)
go

if exists (select name from sys.indexes where name = N'IX_aSession')
drop index IX_aSession on tSchedule
go
create nonclustered index IX_aSession on tSchedule(aSession)
go


--tạo thủ tục lỗi khi thực thi câu truy vấn
if exists (select * from sys.objects where type = 'P' and name = 'spErrorExecuteQuery')
drop procedure spErrorExecuteQuery
go
create procedure spErrorExecuteQuery
as 
begin
		--lấy ra các lỗi rồi in ra thành 1 bảng
		SELECT  
            ERROR_NUMBER() AS ErrorNumber  
            ,ERROR_SEVERITY() AS ErrorSeverity  
            ,ERROR_STATE() AS ErrorState  
            ,ERROR_PROCEDURE() AS ErrorProcedure  
            ,ERROR_LINE() AS ErrorLine  
            ,ERROR_MESSAGE() AS ErrorMessage;  
end
go

--tạo mới 1 thủ tục có tên spStudentUpdate thêm mới student nếu chưa có, cập nhật student nếu đã tồn tại
if exists (select * from sys.objects where type = 'P' and name = 'spStudentUpdate')
drop procedure spStudentUpdate
go
create procedure spStudentUpdate 
@aUsername varchar(100),
@aFullName nvarchar(255),
@aEmail varchar(100)
as 
set nocount on
begin
		
	if((select COUNT(aUsername) from tStudents where aUsername = @aUsername) > 0)
	begin
		begin try
			declare @aStudentID as int;
			select @aStudentID = aStudentID from tStudents where aUsername = @aUsername
			update tStudents set aUsername = @aUsername, aFullName = @aFullName,aEmail = @aEmail,aUpdatedDate = CURRENT_TIMESTAMP where aStudentID = @aStudentID
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	end
	else
	begin 
		begin try
			insert into tStudents(aUsername,aFullName,aEmail,aCreatedDate,aUpdatedDate) values(@aUsername,@aFullName,@aEmail,CURRENT_TIMESTAMP,null) 
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	end
end
go

--tạo mới 1 thủ tục có tên spScheduleUpdate thêm mới schedule nếu chưa có, cập nhật schedule nếu đã tồn tại
if exists (select * from sys.objects where type = 'P' and name = 'spScheduleUpdate')
drop procedure spScheduleUpdate
go
create procedure spScheduleUpdate
@aStudentID int,
@aAttendedDate date,
@aSession int
as 
set nocount on
begin
	if ((select COUNT(aScheduleID) from tSchedule where aStudentID = @aStudentID) > 0)
	 begin
		begin try
			update tSchedule set aAttendedDate = @aAttendedDate, aSession = @aSession,aUpdateDate = CURRENT_TIMESTAMP where aStudentID = @aStudentID
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	 end
	else
	 begin
		begin try
			insert into tSchedule(aStudentID, aAttendedDate, aSession,aCreatedDate,aUpdateDate) values(@aStudentID,@aAttendedDate,@aSession,CURRENT_TIMESTAMP,null)
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	 end
end
go

--tạo mới 1 thủ tục có tên spStudentSelect tìm kiếm student theo username hoặc name hoặc email
if exists (select * from sys.objects where type = 'P' and name = 'spStudentSelect')
drop procedure spStudentSelect
go
create procedure spStudentSelect
@aUsername varchar(100) = null,
@aEmail varchar(100) = null,
@aFullName nvarchar(255) = null
as
begin
	select aStudentID,aUsername,aFullName,aEmail from tStudents where aUsername = @aUsername or aEmail = @aEmail or aFullName = @aFullName
end
go

--tạo mới thủ tục spStudentDelete xóa student theo aStudentID
if exists (select * from sys.objects where type = 'P' and name = 'spStudentDelete ')
drop procedure spStudentDelete 
go
create procedure spStudentDelete 
@aStudentID int
as
set nocount on
begin
if((select count(aStudentID) from tStudents where aStudentID = @aStudentID) > 0)
begin
	begin try
		delete from tStudents where aStudentID = @aStudentID
	end try
	begin catch
		execute spErrorExecuteQuery
	end catch
end
else
begin
	print 'aStudentID not exist in tStudents table'
end
end
go

--tạo thủ tục spScheduleSelect tìm kiếm schedule theo username hoặc theo khoảng time hoặc buổi
if exists (select * from sys.objects where type = 'P' and name = 'spScheduleSelect ')
drop procedure spScheduleSelect 
go
create procedure spScheduleSelect
--tên người dùng để lọc
@aUsername varchar(100) = null,
--@from ngày bắt đầu
--@to ngày kết thúc
@from date = null,
@to date = null,
--biến buổi để lọc
@aSession int = null
as
set nocount on
begin
	select aScheduleID,schedule.aStudentID,students.aFullName,students.aEmail,aAttendedDate,aSession from tSchedule schedule
	inner join tStudents students on students.aStudentID = schedule.aStudentID and (aUsername = @aUsername or (schedule.aAttendedDate between @from and @to) or aSession = @aSession)
end
go

--tạo thủ tục spScheduleDelete xóa schedule theo aScheduleID
if exists (select * from sys.objects where type = 'P' and name = 'spScheduleDelete ')
drop procedure spScheduleDelete 
go
create procedure spScheduleDelete 
@aScheduleID int
as
set nocount on
begin
if((select COUNT(aScheduleID) from tSchedule where aScheduleID = @aScheduleID) > 0)
	begin
		begin try
			delete from tSchedule where aScheduleID = @aScheduleID
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	end
	else
	begin 
		print 'aScheduleID not exist in tSchedule table'
	end
end
go


--thực thi thủ tục spStudentUpdate
-- @aUsername varchar(100)
-- @aFullName nvarchar(255)
-- @aEmail varchar(100)
execute spStudentUpdate 'fitthuctap21',N'Tran Hong Ngoc','ngoc@gmail.com'
go
select * from tStudents 
go

--thực thi thủ tục spScheduleUpdate
-- @aStudentID int
-- @aAttendedDate date
-- @aSession int
execute spScheduleUpdate 11,'2021-3-11',2
go
select * from tSchedule
go

--thực thi thủ tục spStudentSelect
-- @aUsername varchar(100) 
-- @aFullName nvarchar(255)
-- @aEmail varchar(100) 
execute spStudentSelect @aFullName = N'Trần Quốc Đạt'
go

--thực thi thủ tục spStudentDelete
-- @aStudentID int
execute spStudentDelete 11
go
select * from tStudents
go

--thực thi thủ tục spScheduleSelect
-- @aUsername varchar(100)
-- @from date, @to date
-- @aSession int
execute spScheduleSelect @aSession = 1
go

--thực thi thủ tục spScheduleDelete
-- @aScheduleID int
execute spScheduleDelete 19
go

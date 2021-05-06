use master
go

drop database Training
go

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
insert into tStudents values('fitthuctap22',N'Lường Quang Trung','trunglq@gmail.com',CURRENT_TIMESTAMP,null)
go

insert into tSchedule values(1,'2021-3-15',1,CURRENT_TIMESTAMP,NULL)
insert into tSchedule values(2,'2021-3-17',1,CURRENT_TIMESTAMP,NULL)
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
@pUsername varchar(100),
@pFullName nvarchar(255),
@pEmail varchar(100)
as 
set nocount on
begin
		
	if((select COUNT(aUsername) from tStudents where aUsername = @pUsername) > 0)
	begin
		begin try
			declare @aStudentID as int;
			select @aStudentID = aStudentID from tStudents where aUsername = @pUsername
			update tStudents set aUsername = @pUsername, aFullName = @pFullName,aEmail = @pEmail,aUpdatedDate = CURRENT_TIMESTAMP where aStudentID = @aStudentID
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	end
	else
	begin 
		begin try
			insert into tStudents(aUsername,aFullName,aEmail,aCreatedDate,aUpdatedDate) values(@pUsername,@pFullName,@pEmail,CURRENT_TIMESTAMP,null) 
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
@pStudentID int,
@pAttendedDate date,
@pSession int
as 
set nocount on
begin
	if ((select COUNT(aScheduleID) from tSchedule where aStudentID = @pStudentID and aAttendedDate = @pAttendedDate) > 0)
	 begin
		declare @pscheduleID int;
		select @pscheduleID = aScheduleID from tSchedule where aStudentID = @pStudentID and aAttendedDate = @pAttendedDate
		begin try
			update tSchedule set aAttendedDate = @pAttendedDate, aSession = @pSession,aUpdateDate = CURRENT_TIMESTAMP where aScheduleID = @pscheduleID
		end try
		begin catch
			execute spErrorExecuteQuery
		end catch
	 end
	else
	 begin
		begin try
			insert into tSchedule(aStudentID, aAttendedDate, aSession,aCreatedDate,aUpdateDate) values(@pStudentID,@pAttendedDate,@pSession,CURRENT_TIMESTAMP,null)
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
@pUsername varchar(100) = null,
@pEmail varchar(100) = null,
@pFullName nvarchar(255) = null
as
begin
	select aStudentID,aUsername,aFullName,aEmail,aCreatedDate,aUpdatedDate from tStudents where aUsername = @pUsername or aEmail = @pEmail or aFullName = @pFullName
end
go

--tạo mới thủ tục spStudentDelete xóa student theo aStudentID
if exists (select * from sys.objects where type = 'P' and name = 'spStudentDelete ')
drop procedure spStudentDelete 
go
create procedure spStudentDelete 
@pStudentID int
as
set nocount on
begin
if((select count(aStudentID) from tStudents where aStudentID = @pStudentID) > 0)
begin
	begin try
		delete from tStudents where aStudentID = @pStudentID
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
@pUsername varchar(100) = null,
--@from ngày bắt đầu
--@to ngày kết thúc
@pFrom date = null,
@pTo date = null,
--biến buổi để lọc
@pSession int = null
as
set nocount on
begin
	select aScheduleID,schedule.aStudentID,schedule.aCreatedDate,schedule.aUpdateDate,students.aUsername,students.aFullName,students.aEmail,aAttendedDate,aSession from tSchedule schedule
	inner join tStudents students on students.aStudentID = schedule.aStudentID and (aUsername = @pUsername or (schedule.aAttendedDate between @pFrom and @pTo) or aSession = @pSession)
end
go

--tạo thủ tục spScheduleDelete xóa schedule theo aScheduleID
if exists (select * from sys.objects where type = 'P' and name = 'spScheduleDelete ')
drop procedure spScheduleDelete 
go
create procedure spScheduleDelete 
@pScheduleID int
as
set nocount on
begin
if((select COUNT(aScheduleID) from tSchedule where aScheduleID = @pScheduleID) > 0)
	begin
		begin try
			delete from tSchedule where aScheduleID = @pScheduleID
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
-- @pUsername varchar(100)
-- @pFullName nvarchar(255)
-- @pEmail varchar(100)
execute spStudentUpdate 'fitthuctap21',N'Trần Hồng Ngọc','ngocth@gmail.com'
go
select * from tStudents 
go

--thực thi thủ tục spScheduleUpdate
-- @pStudentID int
-- @pAttendedDate date
-- @pSession int
execute spScheduleUpdate 1,'2021-5-4',1
go
select * from tSchedule
go

--thực thi thủ tục spStudentSelect
-- @pUsername varchar(100) 
-- @pFullName nvarchar(255)
-- @pEmail varchar(100) 
execute spStudentSelect @pFullName = N'Trần Quốc Đạt'
go

--thực thi thủ tục spStudentDelete
-- @pStudentID int
execute spStudentDelete 11
go
select * from tStudents
go

--thực thi thủ tục spScheduleSelect
-- @pUsername varchar(100)
-- @pfrom date, @to date
-- @paSession int
execute spScheduleSelect @pUsername = N'fitthuctap20'
go

--thực thi thủ tục spScheduleDelete
-- @pScheduleID int
execute spScheduleDelete 4
go
select * from tSchedule
go

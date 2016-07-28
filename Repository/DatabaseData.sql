
-- 插入基础数据：
INSERT INTO [Region]([Name]) VALUES(N'华北区'),(N'华中区'),(N'西北区'),(N'华东区'),(N'华南区')
GO
INSERT INTO [Priority]([Name]) VALUES(N'紧急'),(N'重要'),(N'一般'),(N'较低')
GO
INSERT INTO [LoginUser]([Name],[IsEnabled],[HourlyWage],[JobTitle]) VALUES
(N'User1',1,30,N'Administrator'),
(N'User2',1,20,N'Test'),
(N'User3',1,22,N'Test'),
(N'User4',1,35,N'Administrator'),
(N'User5',1,30,N'Common User')
GO

-- 插入200万主表Customer数据：
DECLARE @i INT
DECLARE @regionId INT
DECLARE @priorityId INT
DECLARE @phone VARCHAR(100)
DECLARE @index VARCHAR(50)
DECLARE @hours INT
SET @i=1

WHILE @i<=2000000
BEGIN
SET @index = CAST(@i AS VARCHAR)
SET @regionId = CASE @i%5 WHEN 0 THEN 5 ELSE @i%5 END
SET @priorityId = CASE @i%4 WHEN 0 THEN 4 ELSE @i%4 END
SET @phone ='029' + RIGHT('00000000' + CAST(@index AS VARCHAR), 8)
SET @hours = CASE @i%4 WHEN 0 THEN 4 ELSE @i%4 END

INSERT INTO [Customer]([Name],[RegionId],[PriorityId],[Type],[Cost],[Hours],[Address],[Phone])
VALUES('Name' + @index, @regionId, @priorityId, @i%2,@hours*30,@hours, 'Address' + @index, @phone)
SET @i=@i+1
End

-- 为每个Customer插入2个负责人：
DECLARE @userId INT
DECLARE @userId2 INT
SET @i=1

WHILE @i<=2000000
BEGIN
SET @userId = CASE @i%5 WHEN 0 THEN 5 ELSE @i%5 END
SET @userId2 = CASE @userId+1 WHEN 6 THEN 1 ELSE @userId+1 END
SET @hours = CASE @i%4 WHEN 0 THEN 4 ELSE @i%4 END

INSERT INTO [CustomerResponsibleUser]([CustomerId],[UserId],[Cost],[Hours]) VALUES
(@i, @userId, @hours*20, @hours),
(@i, @userId2, @hours*30, (@hours+1)%4)
SET @i=@i+1
End
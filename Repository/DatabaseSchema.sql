USE [PerformanceTest]
GO

-- check if schema exists --
IF EXISTS(SELECT * FROM sys.schemas WHERE name = 'dbo')
BEGIN
	DECLARE @ProcedureName VARCHAR(MAX)
	DECLARE @FunctionName VARCHAR(MAX)
	DECLARE @TriggerName VARCHAR(MAX)
	DECLARE @IndexName VARCHAR(MAX)
	DECLARE @ViewName VARCHAR(MAX)
	DECLARE @TypeName VARCHAR(MAX)
	DECLARE @ConstraintName VARCHAR(MAX)
	DECLARE @TableName VARCHAR(MAX)

	-- drop procedures --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT so.[name] 
		FROM sys.objects AS so INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'P' AND ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @ProcedureName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP PROCEDURE [dbo].[' + @ProcedureName + ']')
		FETCH NEXT FROM CurrentCursor INTO @ProcedureName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop functions --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT so.[name] 
		FROM sys.objects AS so INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE (so.[type] = 'FN' OR so.[type] = 'IF') AND
			ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @FunctionName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP FUNCTION [dbo].[' + @FunctionName + ']')
		FETCH NEXT FROM CurrentCursor INTO @FunctionName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop triggers --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT so.[name] 
		FROM sys.objects AS so INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'TR' AND ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @TriggerName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP TRIGGER [dbo].[' + @TriggerName + ']')
		FETCH NEXT FROM CurrentCursor INTO @TriggerName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop indexes --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT si.[name], so.[name] 
		FROM sys.indexes AS si INNER JOIN sys.objects AS so 
			ON si.[object_id] = so.[object_id] INNER JOIN sys.schemas AS ss 
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'U' AND si.[is_primary_key] = 0 AND si.[is_unique_constraint] = 0	AND 
			ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @IndexName, @TableName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP INDEX [' + @IndexName + '] ON [dbo].[' + @TableName + ']')
		FETCH NEXT FROM CurrentCursor INTO @IndexName, @TableName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop views --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT so.[name] 
		FROM sys.objects AS so INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'V' AND ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @ViewName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP VIEW [dbo].[' + @ViewName + ']')
		FETCH NEXT FROM CurrentCursor INTO @ViewName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop types --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT st.[name] 
		FROM sys.types AS st INNER JOIN sys.schemas AS ss
			ON st.[schema_id] = ss.[schema_id]
		WHERE ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @TypeName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP TYPE [dbo].[' + @TypeName + ']')
		FETCH NEXT FROM CurrentCursor INTO @TypeName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop constraints --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT sc.[name], so.[name] 
		FROM sys.foreign_keys AS sc INNER JOIN sys.objects AS so 
			ON sc.[parent_object_id] = so.[object_id] INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'U' AND ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @ConstraintName, @TableName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('ALTER TABLE [dbo].[' + @TableName + '] DROP CONSTRAINT [' + @ConstraintName + ']')
		FETCH NEXT FROM CurrentCursor INTO @ConstraintName, @TableName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor

	-- drop tables --
	DECLARE CurrentCursor CURSOR 

	FOR SELECT so.[name] 
		FROM sys.objects AS so INNER JOIN sys.schemas AS ss
			ON so.[schema_id] = ss.[schema_id]
		WHERE so.[type] = 'U' AND ss.[name] = 'dbo'
	OPEN CurrentCursor
	FETCH NEXT FROM CurrentCursor INTO @TableName
	WHILE @@fetch_status = 0
	BEGIN
		EXEC('DROP TABLE [dbo].[' + @TableName + ']')
		FETCH NEXT FROM CurrentCursor INTO @TableName
	END

	CLOSE CurrentCursor
	DEALLOCATE CurrentCursor
END
GO

-- create tables --
Create Table [Region]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Region PRIMARY KEY([Id])
)
GO

Create Table [Priority]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Priority PRIMARY KEY([Id])
)
GO

Create Table [Customer]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[RegionId] INT NOT NULL,
	[PriorityId] INT NULL,
	[Type] TINYINT NOT NULL,
	[Cost] FLOAT NOT NULL,
	[Hours] FLOAT NOT NULL,
	[Address] NVARCHAR(100) NULL,
	[Phone] NVARCHAR(100) NULL,

	CONSTRAINT PK_Customer PRIMARY KEY([Id]),
	CONSTRAINT FK_Customer_RegionId_Region_Id FOREIGN KEY([RegionId]) REFERENCES [Region]([Id]) ON DELETE NO ACTION,
	CONSTRAINT FK_Customer_PriorityId_Priority_Id FOREIGN KEY([PriorityId]) REFERENCES [Priority]([Id]) ON DELETE SET NULL
)
GO

Create Table [LoginUser]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[IsEnabled] [BIT] NOT NULL,
	[HourlyWage] [FLOAT] NOT NULL,
	[JobTitle] [NVARCHAR](50) NULL,
	[CompanyName] [NVARCHAR](50) NULL,

	CONSTRAINT PK_User PRIMARY KEY([Id])
)
GO

Create Table [CustomerResponsibleUser]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CustomerId] INT NOT NULL,
	[UserId] INT NOT NULL,
	[Cost] FLOAT NOT NULL,
	[Hours] FLOAT NOT NULL,

	CONSTRAINT PK_CustomerResponsibleUser PRIMARY KEY([Id]),
	CONSTRAINT UX_CustomerResponsibleUser_CustomerId_UserId UNIQUE([CustomerId], [UserId]),
	CONSTRAINT FK_CustomerResponsibleUser_CustomerId_Customer_Id FOREIGN KEY([CustomerId])
		 REFERENCES [Customer]([Id]) ON DELETE CASCADE,
	CONSTRAINT FK_CustomerResponsibleUser_UserId_User_Id FOREIGN KEY(UserId)
		 REFERENCES [LoginUser]([Id]) ON DELETE CASCADE
)
GO

-- Create functions --
CREATE FUNCTION GetCustomers
()
RETURNS TABLE
AS
RETURN
(
	SELECT c.[Id],
		c.[Name], 
		c.[Address], 
		c.[Phone], 
		c.[PriorityId], 
		p.[Name] AS [PriorityName],
		c.[RegionId],
		r.[Name] AS [RegionName], 
		c.[Type], 
		c.[Cost], 
		c.[Hours],
		cuv.*
	FROM [Customer] AS c
		INNER JOIN [Region] AS r ON c.PriorityId = r.Id
		LEFT JOIN [Priority] AS p ON c.PriorityId = p.Id
		OUTER APPLY(
			SELECT TOP 1
			    lu.[CompanyName],
				lu.[HourlyWage],
				lu.[IsEnabled],
				lu.[JobTitle],
				lu.[Name] AS [UserName]
			FROM [CustomerResponsibleUser] AS cu
				INNER JOIN [LoginUser] AS lu ON cu.[UserId] = lu.[Id]
			WHERE c.[Id] = cu.[CustomerId]
			ORDER BY cu.[Id]
		) AS cuv
)
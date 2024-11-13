CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL
)
GO

CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@FirstName nvarchar(50),
	@LastName nvarchar(50)
AS
begin
	update dbo.[User]
	set FirstName = @FirstName, LastName = @LastName
	where Id = @Id;
end
GO 

CREATE PROCEDURE [dbo].[spUser_Insert]
	@FirstName nvarchar(50),
	@LastName nvarchar(50)
AS
begin
	insert into dbo.[User] (FirstName, LastName)
	values (@FirstName, @LastName);
end
GO 

CREATE PROCEDURE [dbo].[spUser_Get]
	@Id int
AS
begin
	select Id, FirstName, LastName
	from dbo.[User]
	where Id = @Id;
end
GO

CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
begin
	delete
	from dbo.[User]
	where Id = @Id;
end
GO

CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
begin
	select Id, FirstName, LastName
	from dbo.[User];
end
GO


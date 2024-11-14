CREATE TABLE [dbo].[Person]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [CellPhoneId] INT NULL
)
GO

CREATE TABLE [dbo].[Phone]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [PhoneNumber] VARCHAR(20) NOT NULL
)
GO

CREATE TYPE [dbo].[BasicUDT] AS TABLE
(
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50)
)
GO


CREATE PROCEDURE [dbo].[spPerson_InsertSet]
@people BasicUDT readonly
AS
BEGIN
    INSERT INTO dbo.Person(FirstName, LastName)
    SELECT [FirstName], [LastName]
    FROM @people;
end
GO

CREATE PROCEDURE [dbo].[spPerson_Search]
@searchTerm VARCHAR(50)
AS
begin
    set nocount on;

    select [Id], [FirstName], [LastName]
    from dbo.Person
    WHERE FirstName LIKE '%' + @searchTerm + '%'
       OR LastName LIKE '%' + @searchTerm + '%';
end
GO






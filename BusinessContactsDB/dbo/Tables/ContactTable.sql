CREATE TABLE [dbo].[ContactTable]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [Company] NVARCHAR(50) NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(200) NULL
)

-- Create the database
CREATE DATABASE squirrelDb
GO

USE squirrelDb
GO

-- Create the stored procedure
CREATE PROCEDURE dbo.pCountAcorns
    @squirrelId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        @squirrelId squirrelId,
        ceiling(rand()*100) acornCount
END
GO

-- Test the stored procedure
DECLARE @squirrelId UNIQUEIDENTIFIER = NEWID()
EXEC dbo.pCountAcorns @squirrelId
GO

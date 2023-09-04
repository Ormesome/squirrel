CREATE DATABASE squirrelDb
GO

USE squirrelDb
GO

CREATE PROCEDURE dbo.pCountAcorns
    @squirrelId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        @squirrelId squirrelId,
        ceiling(rand()*100) acornCount
END
GO

DECLARE @squirrelId UNIQUEIDENTIFIER = NEWID()
EXEC dbo.pCountAcorns @squirrelId
GO

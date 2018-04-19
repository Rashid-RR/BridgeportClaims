sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'Ole Automation Procedures', 1;
GO
RECONFIGURE;
GO
--	DATE	:	23-Nov
--	AUTHOR	:	Jitendra Zaa

CREATE PROCEDURE [dbo].[CreateFolder] (@newfolder varchar(1000)) AS  
BEGIN  
DECLARE @OLEfolder   INT  
DECLARE @OLEsource   VARCHAR(255)  
DECLARE @OLEdescription  VARCHAR(255) 
DECLARE @init   INT  
DECLARE @OLEfilesytemobject INT  
 
-- it will fail if OLE automation not enabled
EXEC @init=sp_OACreate 'Scripting.FileSystemObject', @OLEfilesytemobject OUT  
IF @init <> 0  
BEGIN  
	EXEC sp_OAGetErrorInfo @OLEfilesytemobject  
	RETURN  
END  
-- check if folder exists  
EXEC @init=sp_OAMethod @OLEfilesytemobject, 'FolderExists', @OLEfolder OUT, @newfolder  
-- if folder doesnt exist, create it  
IF @OLEfolder=0  
	BEGIN  
	EXEC @init=sp_OAMethod @OLEfilesytemobject, 'CreateFolder', @OLEfolder OUT, @newfolder  
END  
-- in case of error, raise it   
IF @init <> 0  
	BEGIN  
		EXEC sp_OAGetErrorInfo @OLEfilesytemobject, @OLEsource OUT, @OLEdescription OUT  
		SELECT @OLEdescription='Could not create folder: ' + @OLEdescription  
		RAISERROR (@OLEdescription, 16, 1)   
	END  
EXECUTE @init = sp_OADestroy @OLEfilesytemobject  
END         

   --Prerequisite , make sure that @path exists on drive
DECLARE @path varchar(80) = 'C:\Development\SQL\ExportedFiles',
 @folderName varchar(100)= '',
 @fullPath varchar(500),
 @progressivePath varchar(500)

SET @fullPath = @path +'\'+@folderName
 
DECLARE @pos INT
DECLARE @len INT
DECLARE @curentFolder varchar(8000)
 
set @pos = 0
set @len = 0
SET @progressivePath = @path

--Loop through path and create folder iteratively
WHILE CHARINDEX('\', @folderName, @pos+1) > 0
BEGIN
    set @len = CHARINDEX('\', @folderName, @pos+1) - @pos 
    set @curentFolder = SUBSTRING(@folderName, @pos, @len) 
	SET @progressivePath = @progressivePath + '\'+@curentFolder
    PRINT @progressivePath 
	EXEC CreateFolder @progressivePath
    set @pos = CHARINDEX('\', @folderName, @pos+@len) +1
END
GO

DECLARE @outPutPath varchar(50) = 'C:\Development\SQL\ExportedFiles'
, @i bigint
, @init int
, @data varbinary(max) 
, @fPath varchar(max)  
, @folderPath  varchar(max) 
 
--Get Data into temp Table variable so that we can iterate over it 
DECLARE @Doctable TABLE (id int identity(1,1), [Doc_Num]  varchar(100) , [FileName]  varchar(100), [Doc_Content] varBinary(max) )
 
INSERT INTO @Doctable([Doc_Num] , [FileName],[Doc_Content])
Select [i].[ImportFileID] , i.[FileName],i.[FileBytes] FROM  [util].[ImportFile] AS [i]
WHERE [i].[ImportFileID] = 283
 
--SELECT * FROM @table

SELECT @i = COUNT(1) FROM @Doctable
 
WHILE @i >= 1
BEGIN 

	SELECT 
	 @data = [Doc_Content],
	 @fPath = @outPutPath + '\'+ [Doc_Num] + '\' +[FileName],
	 @folderPath = @outPutPath + '\'+ [Doc_Num]
	FROM @Doctable WHERE id = @i
 
  --Create folder first
  EXEC  [dbo].[CreateFolder]  @folderPath
  
  EXEC sp_OACreate 'ADODB.Stream', @init OUTPUT; -- An instace created
  EXEC sp_OASetProperty @init, 'Type', 1;  
  EXEC sp_OAMethod @init, 'Open'; -- Calling a method
  EXEC sp_OAMethod @init, 'Write', NULL, @data; -- Calling a method
  EXEC sp_OAMethod @init, 'SaveToFile', NULL, @fPath, 2; -- Calling a method
  EXEC sp_OAMethod @init, 'Close'; -- Calling a method
  EXEC sp_OADestroy @init; -- Closed the resources
 
  print 'Document Generated at - '+  @fPath   

--Reset the variables for next use
SELECT @data = NULL  
, @init = NULL
, @fPath = NULL  
, @folderPath = NULL
SET @i -= 1
END
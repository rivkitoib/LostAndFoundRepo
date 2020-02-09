
INSERT INTO [dbo].[Archives]
           ([description]
           ,[picture]
           ,[dateFound]
           ,[notes]
           ,[finderName]
           ,[cellphone]
           ,[email]
           ,[status]
           ,[dateStatus]
           ,[location_Id]
           ,[subCategory_id])
     VALUES
           ('טבעת כסופה'
           ,''
           ,GETDATE()
           ,'להתקשר בשעות הערב'
           ,'רחל כהן'
           ,'0548765324'
           ,'escfg@gmail.com'
           ,'נמסר לבעליו'
           ,GETDATE()
           ,4
           ,1)
GO



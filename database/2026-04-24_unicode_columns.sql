USE [football_booking];
GO

/* Preserve Vietnamese characters in UI-facing text columns */
ALTER TABLE [dbo].[users] ALTER COLUMN [name] NVARCHAR(100) NOT NULL;
GO
ALTER TABLE [dbo].[users] ALTER COLUMN [avt] NVARCHAR(255) NULL;
GO

ALTER TABLE [dbo].[fields] ALTER COLUMN [name] NVARCHAR(100) NOT NULL;
GO
ALTER TABLE [dbo].[fields] ALTER COLUMN [location] NVARCHAR(255) NOT NULL;
GO
ALTER TABLE [dbo].[fields] ALTER COLUMN [description] NVARCHAR(MAX) NULL;
GO
ALTER TABLE [dbo].[fields] ALTER COLUMN [image] NVARCHAR(255) NULL;
GO

ALTER TABLE [dbo].[services] ALTER COLUMN [name] NVARCHAR(100) NOT NULL;
GO
ALTER TABLE [dbo].[services] ALTER COLUMN [description] NVARCHAR(MAX) NULL;
GO
ALTER TABLE [dbo].[services] ALTER COLUMN [image] NVARCHAR(250) NULL;
GO

ALTER TABLE [dbo].[bookings] ALTER COLUMN [note] NVARCHAR(MAX) NULL;
GO

ALTER TABLE [dbo].[feedbacks] ALTER COLUMN [message] NVARCHAR(MAX) NOT NULL;
GO

/* Existing corrupted data (for example: 'm? dinh') cannot be auto-recovered.
   Re-enter or re-import affected records after this migration. */

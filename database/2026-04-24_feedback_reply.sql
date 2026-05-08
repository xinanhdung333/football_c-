USE [football_booking];
GO

IF COL_LENGTH('dbo.feedbacks', 'reply_message') IS NULL
BEGIN
    ALTER TABLE [dbo].[feedbacks]
        ADD [reply_message] NVARCHAR(MAX) NULL;
END
GO

IF COL_LENGTH('dbo.feedbacks', 'replied_by') IS NULL
BEGIN
    ALTER TABLE [dbo].[feedbacks]
        ADD [replied_by] INT NULL;
END
GO

IF COL_LENGTH('dbo.feedbacks', 'replied_at') IS NULL
BEGIN
    ALTER TABLE [dbo].[feedbacks]
        ADD [replied_at] DATETIME NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'fk_feedbacks_replied_by'
)
BEGIN
    ALTER TABLE [dbo].[feedbacks] WITH CHECK
    ADD CONSTRAINT [fk_feedbacks_replied_by]
        FOREIGN KEY ([replied_by]) REFERENCES [dbo].[users]([id]);
END
GO

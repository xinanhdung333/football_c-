IF COL_LENGTH('fields', 'peak_start_time') IS NULL
BEGIN
    ALTER TABLE fields ADD peak_start_time TIME NULL;
END
GO

IF COL_LENGTH('fields', 'peak_end_time') IS NULL
BEGIN
    ALTER TABLE fields ADD peak_end_time TIME NULL;
END
GO

IF COL_LENGTH('fields', 'peak_price_per_hour') IS NULL
BEGIN
    ALTER TABLE fields ADD peak_price_per_hour DECIMAL(18, 2) NULL;
END
GO

    IF OBJECT_ID('booking_payments', 'U') IS NULL
    BEGIN
        CREATE TABLE booking_payments (
            id INT IDENTITY(1,1) PRIMARY KEY,
            booking_id INT NOT NULL,
            payment_method NVARCHAR(50) NOT NULL,
            momo_order_id NVARCHAR(100) NULL,
            momo_trans_id NVARCHAR(100) NULL,
            amount DECIMAL(18, 2) NOT NULL,
            status NVARCHAR(20) NOT NULL DEFAULT 'pending',
            paid_at DATETIME NULL,
            created_at DATETIME NOT NULL DEFAULT GETDATE(),
            updated_at DATETIME NULL,
            CONSTRAINT fk_booking_payments_booking FOREIGN KEY (booking_id) REFERENCES bookings(id)
        );
    END
    GO

    IF OBJECT_ID('service_discounts', 'U') IS NULL
    BEGIN
        CREATE TABLE service_discounts (
            id INT IDENTITY(1,1) PRIMARY KEY,
            service_id INT NULL,
            start_time TIME NOT NULL,
            end_time TIME NOT NULL,
            multiplier DECIMAL(5, 2) NOT NULL,
            note NVARCHAR(255) NULL,
            is_active BIT NOT NULL DEFAULT 1,
            created_at DATETIME NOT NULL DEFAULT GETDATE(),
            updated_at DATETIME NULL,
            CONSTRAINT fk_service_discounts_service FOREIGN KEY (service_id) REFERENCES services(id),
            CONSTRAINT ck_service_discounts_multiplier CHECK (multiplier > 0 AND multiplier <= 1),
            CONSTRAINT ck_service_discounts_time CHECK (end_time > start_time)
        );
    END
    GO

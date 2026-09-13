CREATE TABLE [dbo].[OrderItems]
(
		OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
		OrderID INT NOT NULL REFERENCES [dbo].[Orders](OrderID) ON DELETE CASCADE,
		ProductID INT NOT NULL REFERENCES [dbo].[Products](ProductID) ON DELETE NO ACTION,
		Quantity INT NOT NULL DEFAULT 1,
		UnitPrice DECIMAL(18,4) NOT NULL,
		LineTotal AS (Quantity * UnitPrice) PERSISTED
)

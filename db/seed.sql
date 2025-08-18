INSERT INTO Categories (Name, Active) VALUES ('Whisky', 1), ('Ron',1), ('Vodka',1);

INSERT INTO Products (SKU, Name, CategoryId, Brand, Barcode, AlcoholVolumePct, VolumeMl, Cost, Price, Active)
VALUES
 ('WHI-001', 'Whisky 12 años', 1, 'Glen XX', '1234567890123', 40, 750, 80.00, 120.00, 1),
 ('RON-001', 'Ron Añejo', 2, 'Caribeño', '1234567890456', 38, 750, 25.00, 40.00, 1),
 ('VOD-001', 'Vodka Premium', 3, 'Nordic', '1234567890789', 40, 700, 18.00, 30.00, 1);

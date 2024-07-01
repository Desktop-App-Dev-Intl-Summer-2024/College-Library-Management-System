CREATE DATABASE FarmersMarket;
USE FarmersMarket;

CREATE TABLE Products (
    ProductName VARCHAR(255),
    ProductID INT PRIMARY KEY,
    Amount DECIMAL,
    Price DECIMAL
);

INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES ('Apple', 124567, 23, 2.10);
INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES ('Orange', 345678, 20, 2.49);
INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES ('Raspberry', 125678, 25, 2.35);
INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES ('Blueberry', 456732, 29, 1.45);
INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES ('Cauliflower', 238901, 24, 2.22);
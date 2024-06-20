Create database CollegeLibraryManagementSystem

CREATE TABLE Members (
    MemberID INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100) UNIQUE,
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    RegistrationDate DATE,
    Role NVARCHAR(20) -- 'Student', 'Librarian', 'Administrator'
);

CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200),
    Author NVARCHAR(100),
    Genre NVARCHAR(50),
    ISBN NVARCHAR(20) UNIQUE,
    Publisher NVARCHAR(100),
    YearPublished INT,
    CopiesAvailable INT
);

CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY,
    MemberID INT FOREIGN KEY REFERENCES Members(MemberID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
    IssueDate DATE,
    DueDate DATE,
    ReturnDate DATE,
    FineAmount DECIMAL(5, 2) DEFAULT 0.00
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    MemberID INT FOREIGN KEY REFERENCES Members(MemberID),
    Username NVARCHAR(50) UNIQUE,
    PasswordHash NVARCHAR(256),
    Role NVARCHAR(20) -- 'Librarian', 'Administrator'
);

CREATE TABLE Fines (
    FineID INT PRIMARY KEY IDENTITY,
    TransactionID INT FOREIGN KEY REFERENCES Transactions(TransactionID),
    FineAmount DECIMAL(5, 2),
    FinePaid BIT DEFAULT 0,
    PaymentDate DATE
);

CREATE TABLE Reports (
    ReportID INT PRIMARY KEY IDENTITY,
    ReportType NVARCHAR(50),
    GeneratedDate DATE,
    ReportData NVARCHAR(MAX)
);

INSERT INTO Members (FirstName, LastName, Email, Phone, Address, RegistrationDate, Role)
VALUES ('John', 'Doe', 'john.doe@example.com', '1234567890', '123 Main St', GETDATE(), 'Student');


INSERT INTO Members (FirstName, LastName, Email, Phone, Address, RegistrationDate, Role)
VALUES 
('Alice', 'Johnson', 'alice.johnson@example.com', '2345678901', '456 Elm St', GETDATE(), 'Student'),
('Bob', 'Smith', 'bob.smith@example.com', '3456789012', '789 Pine St', GETDATE(), 'Student'),
('Charlie', 'Brown', 'charlie.brown@example.com', '4567890123', '123 Oak St', GETDATE(), 'Student'),
('Daisy', 'Miller', 'daisy.miller@example.com', '5678901234', '234 Maple St', GETDATE(), 'Student'),
('Eve', 'Davis', 'eve.davis@example.com', '6789012345', '345 Birch St', GETDATE(), 'Student'),
('Frank', 'Wilson', 'frank.wilson@example.com', '7890123456', '456 Cedar St', GETDATE(), 'Student'),
('Grace', 'Taylor', 'grace.taylor@example.com', '8901234567', '567 Ash St', GETDATE(), 'Librarian'),
('Hank', 'Anderson', 'hank.anderson@example.com', '9012345678', '678 Fir St', GETDATE(), 'Librarian'),
('Ivy', 'Martin', 'ivy.martin@example.com', '0123456789', '789 Spruce St', GETDATE(), 'Administrator');


INSERT INTO Books (Title, Author, Genre, ISBN, Publisher, YearPublished, CopiesAvailable)
VALUES ('C# Programming', 'Jane Smith', 'Programming', '1234567890123', 'Tech Publishers', 2023, 10);

INSERT INTO Books (Title, Author, Genre, ISBN, Publisher, YearPublished, CopiesAvailable)
VALUES 
('The Great Gatsby', 'F. Scott Fitzgerald', 'Fiction', '9780743273565', 'Charles Scribner\' , 1925, 5),
('To Kill a Mockingbird', 'Harper Lee', 'Fiction', '9780061120084', 'J.B. Lippincott & Co.', 1960, 3),
('1984', 'George Orwell', 'Dystopian', '9780451524935', 'Secker & Warburg', 1949, 4),
('Pride and Prejudice', 'Jane Austen', 'Romance', '9781503290563', 'T. Egerton', 1813, 6),
('The Catcher in the Rye', 'J.D. Salinger', 'Fiction', '9780316769488', 'Little, Brown and Company', 1951, 2),
('The Hobbit', 'J.R.R. Tolkien', 'Fantasy', '9780547928227', 'George Allen & Unwin', 1937, 8),
('Moby Dick', 'Herman Melville', 'Adventure', '9781503280786', 'Harper & Brothers', 1851, 7),
('War and Peace', 'Leo Tolstoy', 'Historical Fiction', '9780199232765', 'The Russian Messenger', 1869, 4),
('The Odyssey', 'Homer', 'Epic', '9780140268867', 'Penguin Classics', 1800, 5);

INSERT INTO Transactions (MemberID, BookID, IssueDate, DueDate)
VALUES 
(2, 2, GETDATE(), DATEADD(DAY, 14, GETDATE())), -- Alice Johnson issues 'The Great Gatsby'
(3, 3, GETDATE(), DATEADD(DAY, 14, GETDATE())), -- Bob Smith issues 'To Kill a Mockingbird'
(4, 4, GETDATE(), DATEADD(DAY, 14, GETDATE())), -- Charlie Brown issues '1984'
(5, 5, GETDATE(), DATEADD(DAY, 14, GETDATE())); -- Daisy Miller issues 'Pride and Prejudice'


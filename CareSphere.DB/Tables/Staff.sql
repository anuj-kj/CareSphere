CREATE TABLE Staff (
    StaffId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Role NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) NULL,
    Email NVARCHAR(255) NULL,
    WorkingHoursStart TIME NULL,
    WorkingHoursEnd TIME NULL,
    OrganizationId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    CONSTRAINT FK_Staff_Organization FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId)
);

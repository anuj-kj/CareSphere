CREATE TABLE Doctor (
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Specialization NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(15) NULL,
    Email NVARCHAR(255) NULL,
    AvailableFrom TIME NULL,
    AvailableTo TIME NULL,
    OrganizationId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    CONSTRAINT FK_Doctor_Organization FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId) ON DELETE CASCADE
);



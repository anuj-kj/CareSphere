﻿CREATE TABLE [User] (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    Username NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NULL,
    DoctorId INT NULL,
    PatientId INT NULL,
    StaffId INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,

    CONSTRAINT FK_User_Doctor FOREIGN KEY (DoctorId) REFERENCES Doctor(DoctorId) ON DELETE SET NULL,
    CONSTRAINT FK_User_Patient FOREIGN KEY (PatientId) REFERENCES Patient(PatientId) ON DELETE SET NULL,
    CONSTRAINT FK_User_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId) ON DELETE SET NULL
);

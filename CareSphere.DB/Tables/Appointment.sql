CREATE TABLE Appointment (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL,
    PatientId INT NOT NULL,
    StaffId INT NULL,
    OrganizationId INT NOT NULL,
    Date DATE NOT NULL,
    Time TIME NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    CONSTRAINT FK_Appointment_Doctor FOREIGN KEY (DoctorId) REFERENCES Doctor(DoctorId) ON DELETE CASCADE,
    CONSTRAINT FK_Appointment_Patient FOREIGN KEY (PatientId) REFERENCES Patient(PatientId) ON DELETE CASCADE,
    CONSTRAINT FK_Appointment_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId) ON DELETE SET NULL,
    CONSTRAINT FK_Appointment_Organization FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId) ON DELETE CASCADE
);

CREATE TABLE PatientOrganization (
    PatientId INT NOT NULL,
    OrganizationId INT NOT NULL,
    CONSTRAINT PK_PatientOrganization PRIMARY KEY (PatientId, OrganizationId),
    CONSTRAINT FK_PatientOrganization_Patient FOREIGN KEY (PatientId) REFERENCES Patient(PatientId) ON DELETE CASCADE,
    CONSTRAINT FK_PatientOrganization_Organization FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId) ON DELETE CASCADE
);

-- Script 1: Crear Base de Datos y Tablas
-- ======================================

-- 1. Crear la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ExamenRPP')
BEGIN
    CREATE DATABASE ExamenRPP;
END
GO

USE ExamenRPP;
GO

-- 2. Crear tabla Persona
CREATE TABLE Persona (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ApPaterno NVARCHAR(50) NOT NULL,
    ApMaterno NVARCHAR(50) NOT NULL,
    Nombres NVARCHAR(50) NOT NULL,
    Genero CHAR(1) NOT NULL,
    FechaNacimiento DATETIME NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL
);
GO

-- 3. Crear tabla Trabajador
CREATE TABLE Trabajador (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdPersona INT NOT NULL,
    FechaIngreso DATETIME NOT NULL,
    Estado BIT NOT NULL,
    CONSTRAINT FK_Trabajador_Persona FOREIGN KEY (IdPersona) REFERENCES Persona(Id)
);
GO

-- 4. Crear tabla Hijos
CREATE TABLE Hijos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdTrabajador INT NOT NULL,
    IdHijo INT NOT NULL,
    Estado BIT NOT NULL,
    CONSTRAINT FK_Hijos_Trabajador FOREIGN KEY (IdTrabajador) REFERENCES Trabajador(Id),
    CONSTRAINT FK_Hijos_Persona FOREIGN KEY (IdHijo) REFERENCES Persona(Id)
);
GO

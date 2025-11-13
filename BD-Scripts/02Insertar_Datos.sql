-- Script 2: Insertar Datos (Fechas como DATETIME)
-- ================================================

USE ExamenRPP;
GO

-- Insertar Personas
INSERT INTO Persona (ApPaterno, ApMaterno, Nombres, Genero, FechaNacimiento, FechaRegistro, Estado)
VALUES 
('Calla','Bruna', 'Juan Junior', 'M', CONVERT(DATETIME, '1987-07-31', 120), GETDATE(),1), 
('Perez','Garcia', 'Georgina', 'F', CONVERT(DATETIME, '1990-07-31', 120), GETDATE(),1),
('Calla','Paredes', 'Russell', 'M', CONVERT(DATETIME, '2017-07-31', 120), GETDATE(),1),
('Calla','Paredes', 'Axell', 'M', CONVERT(DATETIME, '2022-10-31', 120), GETDATE(),1),
('Chavez','Castillo', 'Camila', 'F', CONVERT(DATETIME, '2004-07-31', 120), GETDATE(),1),
('Castillo','Terrones', 'Lilia', 'F', CONVERT(DATETIME, '1987-01-01', 120), GETDATE(),1),
('Chavez','Guerrero', 'Carlos', 'M', CONVERT(DATETIME, '1999-07-31', 120), GETDATE(),1),
('Pariona','Colca', 'Edmer', 'M', CONVERT(DATETIME, '1960-06-11', 120), GETDATE(),1),
('Zegarra','Garcia', 'Jose Luis', 'M', CONVERT(DATETIME, '1980-04-12', 120), GETDATE(),1),
('Chavez','Castillo', 'Francisco', 'M', CONVERT(DATETIME, '2006-03-11', 120), GETDATE(),1);
GO

-- Insertar Trabajadores
INSERT INTO Trabajador (IdPersona, FechaIngreso, Estado)
VALUES 
(1, DATEADD(DAY, -720, GETDATE()), 1), 
(9, CONVERT(DATETIME, '2020-04-08', 120), 1), 
(6, CONVERT(DATETIME, '1999-01-22', 120), 1),
(2, CONVERT(DATETIME, '2001-12-01', 120), 1),
(7, CONVERT(DATETIME, '1988-01-22', 120), 1);
GO

-- Insertar Hijos
INSERT INTO Hijos (IdTrabajador, IdHijo, Estado)
VALUES 
(1,4,1), 
(1,3,1),
(3,5,1), 
(5,5,1), 
(3,10,1);
GO

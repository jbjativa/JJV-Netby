CREATE DATABASE JAIMEJATIVA
GO
USE JAIMEJATIVA
GO
CREATE TABLE Tarea (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaCreacion DATETIME NOT NULL,
    FechaVencimiento DATETIME,
    Completada BIT NOT NULL DEFAULT 0
);

ALTER TABLE Tarea
ADD Estado BIT NOT NULL DEFAULT 1;


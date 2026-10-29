-- =====================================================================
-- Biblioteca Virtual SENA - Base de datos (MySQL / MariaDB - XAMPP)
-- Crea la base de datos completa con el estado final de las migraciones.
-- Uso: importar este archivo en phpMyAdmin o ejecutar:
--   mysql -u root < database/BibliotecaVirtualDB.sql
-- =====================================================================

CREATE DATABASE IF NOT EXISTS `BibliotecaVirtualDB` CHARACTER SET utf8mb4;
USE `BibliotecaVirtualDB`;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

-- Usuarios del sistema (Admin / Usuario)
CREATE TABLE IF NOT EXISTS `Usuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Correo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Rol` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

-- Catalogo de libros
CREATE TABLE IF NOT EXISTS `Libros` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Titulo` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Autor` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Categoria` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Descripcion` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UrlPortada` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UrlArchivo` longtext CHARACTER SET utf8mb4 NULL,
    `UsuarioId` int NULL,
    CONSTRAINT `PK_Libros` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Libros_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Libros_UsuarioId` ON `Libros` (`UsuarioId`);

-- Libros favoritos de cada usuario
CREATE TABLE IF NOT EXISTS `Favoritos` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UsuarioId` int NOT NULL,
    `LibroId` int NOT NULL,
    CONSTRAINT `PK_Favoritos` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Favoritos_Libros_LibroId` FOREIGN KEY (`LibroId`) REFERENCES `Libros` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Favoritos_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Favoritos_LibroId` ON `Favoritos` (`LibroId`);
CREATE INDEX `IX_Favoritos_UsuarioId` ON `Favoritos` (`UsuarioId`);

-- Usuario administrador inicial (cambiar la clave despues de entrar)
INSERT INTO `Usuarios` (`Id`, `Correo`, `Nombre`, `Password`, `Rol`)
VALUES (1, 'admin@biblioteca.com', 'Administrador Principal', 'admin123', 'Admin');

-- Marca las migraciones de Entity Framework como aplicadas
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20250927214224_InitialCreate', '9.0.9'),
('20250927223607_SeedAdminUser', '9.0.9'),
('20250928172701_AddFavoritosTable', '9.0.9'),
('20250928190233_AddUserToBookRelationship', '9.0.9');

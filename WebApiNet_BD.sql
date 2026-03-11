-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         9.6.0 - MySQL Community Server - GPL
-- SO del servidor:              Linux
-- HeidiSQL Versión:             12.15.0.7171
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para WebApiNet
CREATE DATABASE IF NOT EXISTS `WebApiNet` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `WebApiNet`;

-- Volcando estructura para tabla WebApiNet.__EFMigrationsHistory
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla WebApiNet.__EFMigrationsHistory: ~0 rows (aproximadamente)
REPLACE INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
	('20260227083330_IntialCreate', '9.0.2');

-- Volcando estructura para tabla WebApiNet.Alquileres
CREATE TABLE IF NOT EXISTS `Alquileres` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FechaAlquiler` date NOT NULL,
  `FechaDevolucionPrevista` date NOT NULL,
  `FechaDevolucionReal` date DEFAULT NULL,
  `Precio` decimal(18,2) NOT NULL,
  `ClienteDni` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `VehiculoMatricula` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Alquileres_ClienteDni` (`ClienteDni`),
  KEY `IX_Alquileres_VehiculoMatricula` (`VehiculoMatricula`),
  CONSTRAINT `FK_Alquileres_Clientes_ClienteDni` FOREIGN KEY (`ClienteDni`) REFERENCES `Clientes` (`Dni`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Alquileres_Vehiculos_VehiculoMatricula` FOREIGN KEY (`VehiculoMatricula`) REFERENCES `Vehiculos` (`Matricula`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla WebApiNet.Alquileres: ~2 rows (aproximadamente)
REPLACE INTO `Alquileres` (`Id`, `FechaAlquiler`, `FechaDevolucionPrevista`, `FechaDevolucionReal`, `Precio`, `ClienteDni`, `VehiculoMatricula`) VALUES
	(1, '2026-03-11', '2026-03-15', '2026-03-11', 120.00, '01248796T', '4578JKL'),
	(2, '2026-03-07', '2026-03-10', '2026-03-11', 150.00, '98547541P', '9999SPD');

-- Volcando estructura para tabla WebApiNet.Clientes
CREATE TABLE IF NOT EXISTS `Clientes` (
  `Dni` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Role` int NOT NULL,
  PRIMARY KEY (`Dni`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla WebApiNet.Clientes: ~9 rows (aproximadamente)
REPLACE INTO `Clientes` (`Dni`, `Email`, `PasswordHash`, `Nombre`, `Role`) VALUES
	('01248796T', 'rodrigo@test.com', '$2a$11$OJuWzUW6MnBIcvyCqxWZb.H9b/3pfH23Jpiq9WieaOZ6T99zh0Uf6', 'Rodrigo', 1),
	('06241547K', 'maria@test.com', '$2a$11$B0bRVnFscJZDD1eaDVm4Keff7viP2Pa6NfkR.zcsrMHhaQg6qsBiu', 'Maria', 1),
	('06280146L', 'miguel@test.com', '1234', 'Miguel', 0),
	('06280457K', 'dolores@test.com', '$2a$11$eU5F./P2xQxvD0zn3iKolOxeOlAZmplCz35aaqRgCKKAkmlb4mGOi', 'Dolores', 1),
	('54682485O', 'alberto@test.com', '$2a$11$5pHPfJ1/w9cIJlUAPBSi8ucDDKf3ajW4halLbwNrxreVx0X7lJUnW', 'Alberto', 1),
	('87454787L', 'rufino@test.com', '$2a$11$o7A91mrG2u8oTOhJOfWjRu57XNtAGQa6LcnO/pJI2c.8V5sRlVR2.', 'Rufino', 1),
	('87548745H', 'raul@test.com', '$2a$11$pPS.Wh21AIDlFRCjxpScWegsGTcZP9FYTT6fukzjtEFuTPH03RyLK', 'Raul', 1),
	('98547541P', 'Carlos@test.com', '$2a$11$JCbvyW5WeBjWscaWs08KpuAI0TotcPZP2.GZaoIo30PMLmH.bIkIu', 'Carlos', 1);

-- Volcando estructura para procedimiento WebApiNet.sp_delete_alquileres
DELIMITER //
CREATE PROCEDURE `sp_delete_alquileres`(
	IN `p_id` INT
)
BEGIN
	DELETE FROM Alquileres
	WHERE Id = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_delete_cliente
DELIMITER //
CREATE PROCEDURE `sp_delete_cliente`(
	IN `p_dni` VARCHAR(50)
)
BEGIN
	DELETE FROM Clientes
	WHERE Dni = p_dni;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_delete_vehiculo
DELIMITER //
CREATE PROCEDURE `sp_delete_vehiculo`(
	IN `p_matricula` VARCHAR(50)
)
BEGIN
	DELETE FROM Vehiculos
	WHERE Matricula = p_matricula;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_finalizar_alquiler
DELIMITER //
CREATE PROCEDURE `sp_finalizar_alquiler`(
	IN `p_id` INT,
	IN `p_fecha_devolucion_real` VARCHAR(50)
)
BEGIN
	UPDATE Alquileres 
	SET 
		FechaDevolucionReal = p_fecha_devolucion_real
	WHERE Id = p_id;

END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_insert_alquiler
DELIMITER //
CREATE PROCEDURE `sp_insert_alquiler`(
	IN `p_dni_cliente` VARCHAR(50),
	IN `p_matricula_vehiculo` VARCHAR(50),
	IN `p_fecha_alquiler` DATE,
	IN `p_fecha_devolucion_prevista` DATE,
	IN `p_fecha_devolucion_real` DATE,
	IN `p_precio` DECIMAL(18,2)
)
BEGIN
	INSERT INTO Alquileres
	(
		FechaAlquiler,
		FechaDevolucionPrevista,
		FechaDevolucionReal,
		Precio,
		ClienteDni,
		VehiculoMatricula
	)
	VALUES 
	(
		p_fecha_alquiler,
		p_fecha_devolucion_prevista,
		p_fecha_devolucion_real,
		p_precio,
		p_dni_cliente,
		p_matricula_vehiculo
	);
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_insertar_cliente
DELIMITER //
CREATE PROCEDURE `sp_insertar_cliente`(
	IN `p_dni` VARCHAR(50),
	IN `p_email` VARCHAR(50),
	IN `p_password_hash` VARCHAR(255),
	IN `p_nombre` VARCHAR(50),
	IN `p_role` INT
)
BEGIN
INSERT INTO Clientes (
    Dni,
    Email,
    PasswordHash,
    Nombre,
    Role
)
VALUES (
	p_dni,
	p_email,
	p_password_hash,
	p_nombre,
	p_role
);
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_insertar_vehiculo
DELIMITER //
CREATE PROCEDURE `sp_insertar_vehiculo`(
	IN `p_matricula` VARCHAR(255),
	IN `p_tipo_vehiculo` INT,
	IN `P_kilometraje` INT,
	IN `p_marca` VARCHAR(50),
	IN `p_modelo` VARCHAR(50),
	IN `p_precio` DECIMAL(18,2),
	IN `p_litros_tanque` DOUBLE,
	IN `p_estado` INT
)
BEGIN
INSERT INTO Vehiculos (
    Matricula, 
    TipoVehiculo, 
    Kilometraje, 
    Marca, 
    Modelo, 
    Precio, 
    LitrosTanque, 
    Estado
)
VALUES (
    p_matricula, 
    p_tipo_vehiculo, 
    p_kilometraje, 
    p_marca, 
    p_modelo, 
    p_precio, 
    p_litros_tanque, 
    p_estado
);
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_alquiler_activo
DELIMITER //
CREATE PROCEDURE `sp_obtener_alquiler_activo`(
	IN `p_dni_cliente` VARCHAR(50),
	IN `p_matricula_vehiculo` VARCHAR(50)
)
BEGIN
	SELECT * FROM Alquileres
	WHERE ClienteDni = p_dni_cliente
	AND VehiculoMatricula = p_matricula_vehiculo
	AND FechaDevolucionReal IS NULL;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_alquiler_id
DELIMITER //
CREATE PROCEDURE `sp_obtener_alquiler_id`(
	IN `p_id` INT
)
BEGIN
	SELECT * FROM Alquileres
	WHERE Id = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_alquileres
DELIMITER //
CREATE PROCEDURE `sp_obtener_alquileres`(
	IN `p_page_number` INT,
	IN `p_page_size` INT
)
BEGIN
	DECLARE v_offset INT;
	    SET v_offset = (p_page_number - 1) * p_page_size;
	
	    -- Primera consulta: Total de registros (para que el front sepa cuántas páginas hay)
	    SELECT COUNT(*) FROM Alquileres;
	
	    -- Segunda consulta: Los datos paginados
	    SELECT * FROM Alquileres 
	    ORDER BY Id 
	    LIMIT p_page_size OFFSET v_offset;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_cliente_dni
DELIMITER //
CREATE PROCEDURE `sp_obtener_cliente_dni`(
	IN `p_dni` VARCHAR(50)
)
BEGIN
	SELECT * FROM Clientes
	WHERE Dni = p_dni;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_cliente_email
DELIMITER //
CREATE PROCEDURE `sp_obtener_cliente_email`(
	IN `p_email_cliente` VARCHAR(50)
)
BEGIN
SELECT * FROM Clientes
WHERE Email = p_email_cliente;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_clientes
DELIMITER //
CREATE PROCEDURE `sp_obtener_clientes`(
	IN `p_page_number` INT,
	IN `p_page_size` INT
)
BEGIN
	DECLARE v_offset INT;
	    SET v_offset = (p_page_number - 1) * p_page_size;
	
	    -- Primera consulta: Total de registros (para que el front sepa cuántas páginas hay)
	    SELECT COUNT(*) FROM Clientes;
	
	    -- Segunda consulta: Los datos paginados
	    SELECT * FROM Clientes 
	    ORDER BY Dni 
	    LIMIT p_page_size OFFSET v_offset;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_vehiculo_por_matricula
DELIMITER //
CREATE PROCEDURE `sp_obtener_vehiculo_por_matricula`(
	IN `matricula_vehiculo` VARCHAR(50)
)
BEGIN
SELECT * FROM Vehiculos
WHERE Matricula = matricula_vehiculo;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_obtener_vehiculos
DELIMITER //
CREATE PROCEDURE `sp_obtener_vehiculos`(
	IN `p_page_number` INT,
	IN `p_page_size` INT
)
BEGIN
	DECLARE v_offset INT;
	    SET v_offset = (p_page_number - 1) * p_page_size;
	
	    -- Primera consulta: Total de registros (para que el front sepa cuántas páginas hay)
	    SELECT COUNT(*) FROM Vehiculos;
	
	    -- Segunda consulta: Los datos paginados
	    SELECT * FROM Vehiculos 
	    ORDER BY Matricula
	    LIMIT p_page_size OFFSET v_offset;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_update_alquiler
DELIMITER //
CREATE PROCEDURE `sp_update_alquiler`(
	IN `p_id` INT,
	IN `p_fecha_alquiler` DATE,
	IN `p_fecha_devolucion_prevista` DATE,
	IN `p_precio` DECIMAL(18,2)
)
BEGIN

UPDATE Alquileres 
	SET 
		FechaAlquiler = p_fecha_alquiler,
		FechaDevolucionPrevista = p_fecha_devolucion_prevista,
		Precio = p_precio
	WHERE Id = p_id;

END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_update_cliente
DELIMITER //
CREATE PROCEDURE `sp_update_cliente`(
	IN `p_email` VARCHAR(50),
	IN `p_password` VARCHAR(50),
	IN `p_dni` VARCHAR(50),
	IN `p_nombre` VARCHAR(50)
)
BEGIN
UPDATE Clientes 
	SET 
		Email = p_email,
		PasswordHas = p_password,
		Nombre = p_nombre
	WHERE Dni = p_dni;
END//
DELIMITER ;

-- Volcando estructura para procedimiento WebApiNet.sp_update_vehiculo
DELIMITER //
CREATE PROCEDURE `sp_update_vehiculo`(
	IN `p_matricula` VARCHAR(50),
	IN `p_tipo_vehiculo` INT,
	IN `p_kilometraje` INT,
	IN `p_marca` VARCHAR(50),
	IN `p_modelo` VARCHAR(50),
	IN `p_precio` DECIMAL(18,2),
	IN `p_litros_tanque` DOUBLE,
	IN `p_estado` INT
)
BEGIN
UPDATE Vehiculos 
	SET 
		TipoVehiculo = p_tipo_vehiculo,
		Kilometraje = p_kilometraje,
		Marca = p_marca,
		Modelo = p_modelo,
		Precio = p_precio,
		LitrosTanque = p_litros_tanque,
		Estado = p_estado
	WHERE Matricula = p_matricula;
END//
DELIMITER ;

-- Volcando estructura para tabla WebApiNet.Vehiculos
CREATE TABLE IF NOT EXISTS `Vehiculos` (
  `Matricula` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TipoVehiculo` int NOT NULL,
  `Kilometraje` int NOT NULL,
  `Marca` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Modelo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Precio` decimal(18,2) NOT NULL,
  `LitrosTanque` double NOT NULL,
  `Estado` int NOT NULL,
  PRIMARY KEY (`Matricula`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla WebApiNet.Vehiculos: ~53 rows (aproximadamente)
REPLACE INTO `Vehiculos` (`Matricula`, `TipoVehiculo`, `Kilometraje`, `Marca`, `Modelo`, `Precio`, `LitrosTanque`, `Estado`) VALUES
	('0001FST', 0, 555000, 'Porsche', '911 Carrera', 200.00, 70, 0),
	('0007BND', 3, 1200, 'Aston Martin', 'Vantage', 155000.00, 73, 0),
	('1111CAM', 5, 25000, 'Fiat', 'Ducato Camper', 55000.00, 90, 0),
	('1111LPK', 2, 100000, 'Audi', 'A5', 150.00, 64, 0),
	('1122QWE', 0, 205000, 'Mercedes', 'Vito', 15000.00, 70, 0),
	('1234BCN', 2, 45000, 'Seat', 'Leon', 18500.50, 50, 0),
	('2222RVL', 5, 40000, 'Hymer', 'B-Class', 89000.00, 100, 0),
	('2233XCV', 2, 15000, 'Mazda', '3', 24000.00, 51, 0),
	('2564UYK', 2, 100000, 'Dacia', 'Sandero', 120.00, 64, 0),
	('3344ASD', 1, 500000, 'MAN', 'TGX', 45000.00, 500, 0),
	('4433FAM', 4, 65000, 'Chrysler', 'Pacifica', 35000.00, 70, 0),
	('4455VBN', 0, 12000, 'Peugeot', 'Partner', 21500.00, 60, 0),
	('4578JKL', 1, 120000, 'Toyota', 'Corolla', 120.00, 70, 0),
	('4612GJX', 2, 100000, 'Opel', 'Astra', 145.00, 55, 0),
	('5544PLK', 2, 89000, 'Volkswagen', 'Golf', 12000.00, 55, 0),
	('5566GHT', 4, 98000, 'Ford', 'S-Max', 14000.00, 65, 0),
	('6677RTY', 1, 350000, 'Volvo', 'FH16', 85000.00, 400, 0),
	('7845JKS', 2, 12500, 'Toyota', 'Corolla', 22000.00, 50, 0),
	('7845POL', 2, 200000, 'Toyota', 'Auris', 130.00, 68, 0),
	('8812LMN', 0, 30000, 'Citroen', 'Berlingo', 19000.00, 60, 0),
	('9001AAA', 2, 1000, 'Ford', 'Fiesta', 17000.00, 42, 0),
	('9002BBB', 2, 2000, 'Kia', 'Ceed', 19500.00, 50, 0),
	('9003CCC', 0, 85000, 'Opel', 'Combo', 11000.00, 55, 0),
	('9004DDD', 1, 150000, 'Iveco', 'Stralis', 38000.00, 300, 0),
	('9005EEE', 3, 200, 'Ferrari', 'F8', 280000.00, 78, 0),
	('9006FFF', 5, 12000, 'Benimar', 'Tessoro', 62000.00, 90, 0),
	('9007GGG', 2, 45600, 'Honda', 'Civic', 21000.00, 46, 0),
	('9008HHH', 0, 67000, 'Fiat', 'Doblo', 13000.00, 60, 0),
	('9009III', 4, 34000, 'Renault', 'Espace', 29000.00, 68, 0),
	('9011KKK', 1, 220000, 'DAF', 'XF', 54000.00, 420, 0),
	('9012LLL', 3, 4000, 'Nissan', 'GT-R', 105000.00, 74, 0),
	('9013MMM', 2, 15000, 'Skoda', 'Octavia', 26000.00, 50, 0),
	('9014NNN', 0, 99000, 'Ford', 'Transit', 18000.00, 80, 0),
	('9015OOO', 5, 500, 'Volkswagen', 'California', 75000.00, 70, 0),
	('9016PPP', 2, 23000, 'BMW', 'Serie 3', 39000.00, 59, 0),
	('9017QQQ', 4, 120000, 'Citroen', 'Grand C4', 9000.00, 60, 0),
	('9018RRR', 1, 400000, 'Mercedes', 'Actros', 67000.00, 480, 0),
	('9019SSS', 3, 1000, 'Lamborghini', 'Huracán', 240000.00, 80, 0),
	('9020TTT', 2, 54000, 'Opel', 'Astra', 14500.00, 52, 0),
	('9021UUU', 0, 11000, 'Nissan', 'NV200', 16500.00, 55, 0),
	('9022VVV', 2, 0, 'Tesla', 'Model S', 85000.00, 0, 0),
	('9023WWW', 5, 80000, 'Knaus', 'Sky TI', 42000.00, 90, 0),
	('9024XXX', 1, 600000, 'Renault', 'Magnum', 25000.00, 500, 0),
	('9025YYY', 3, 3500, 'Jaguar', 'F-Type', 72000.00, 63, 0),
	('9026ZZZ', 2, 75000, 'Peugeot', '308', 12500.00, 53, 0),
	('9027A1B', 4, 15000, 'Toyota', 'Sienna', 42000.00, 75, 0),
	('9028C2D', 0, 3000, 'Toyota', 'Proace', 31000.00, 70, 0),
	('9029E3F', 2, 44000, 'Lexus', 'IS', 33000.00, 66, 0),
	('9030G4H', 1, 100000, 'Mack', 'Anthem', 110000.00, 600, 0),
	('9031I5J', 3, 100, 'Bugatti', 'Chiron', 3000000.00, 100, 0),
	('9900UIO', 1, 120000, 'Scania', 'R500', 92000.00, 450, 0),
	('9988HHH', 2, 0, 'Hyundai', 'i30', 21000.00, 45, 0),
	('9999SPD', 3, 15000, 'Audi', 'R8', 110000.00, 83, 0);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;

INSERT INTO Pacientes (Nombres, Apellidos, Documento, FechaDeNacimiento, Direccion, Telefono, Email)
VALUES 
('Juan Carlos', 'González Pérez', '12345678', '1985-03-15', 'Av. Siempre Viva 742', '098123456', 'juancarlos.gp@example.com'),
('María Fernanda', 'Martínez López', '87654321', '1992-07-22', 'Calle Falsa 123', '091234567', 'maria.fernanda@example.com'),
('José Luis', 'Rodríguez Sánchez', '65432109', '1978-01-05', 'Calle Los Pinos 456', '098765432', 'jose.luis@example.com'),
('Ana Isabel', 'Ramírez Díaz', '34567890', '1989-12-10', 'Calle Las Rosas 789', '095123987', 'ana.isabel@example.com'),
('Carlos Eduardo', 'Hernández Castro', '11223344', '1975-04-28', 'Calle Sol 321', '093987654', 'carlos.eduardo@example.com'),
('Lucía Paola', 'Fernández Torres', '99887766', '2000-09-15', 'Avenida del Parque 22', '099234567', 'lucia.paola@example.com'),
('Miguel Ángel', 'Vargas Domínguez', '55667788', '1965-11-30', 'Calle del Río 567', '098654321', 'miguel.angel@example.com'),
('Sofía Verónica', 'Méndez Jiménez', '44332211', '1995-05-18', 'Avenida Libertador 99', '092345678', 'sofia.veronica@example.com'),
('Jorge Enrique', 'Pérez García', '33221144', '1980-02-25', 'Calle del Sol 876', '091098765', 'jorge.enrique@example.com'),
('Claudia Patricia', 'López Ruiz', '12344321', '1999-08-07', 'Calle 8 de Octubre 543', '094567890', 'claudia.patricia@example.com');

INSERT INTO CitasMedicas (Fecha, Estado, CalendarioId, ConsultorioId, PacienteId)
VALUES 
(GETDATE(), 'Completada', NULL, NULL, 1),
(GETDATE(), 'Completada', NULL, NULL, 2),
(GETDATE(), 'Agendada', NULL, NULL, 3),
(GETDATE(), 'Agendada', NULL, NULL, 4),
(GETDATE(), 'Agendada', NULL, NULL, 5),
(GETDATE(), 'Agendada', NULL, NULL, 6),
(GETDATE(), 'Agendada', NULL, NULL, 7),
(GETDATE(), 'Agendada', NULL, NULL, 8),
(GETDATE(), 'Agendada', NULL, NULL, 9),
(GETDATE(), 'Agendada', NULL, NULL, 10);

INSERT INTO ConsultasMedicas (Descripcion, Diagnostico, CitaMedicaId)
VALUES 
('Consulta general sobre dolor de cabeza', 'Diagnóstico de migraña leve', 1),
('Revisión de control post-operatorio', 'Diagnóstico de recuperación satisfactoria', 2);

-- Insertar dos estudios para la consulta médica con Id = 2
INSERT INTO Estudios (Nombre, Descripcion, FechaRealizado, FechaResultado, ConsultaMedicaId, ImagenUrl)
VALUES 
('Radiografía de Tórax', 'Estudio de imagen del tórax', '2024-10-01', '2024-10-02', 1, 'DSADFASD'),
('Examen de Sangre', 'Prueba completa de laboratorio', '2024-10-01', '2024-10-02', 1, 'ASDASDASD');

-- Insertar un estudio para la consulta médica con Id = 3
INSERT INTO Estudios (Nombre, Descripcion, FechaRealizado, FechaResultado, ConsultaMedicaId, ImagenUrl)
VALUES 
('Ultrasonido Abdominal', 'Exploración del abdomen', '2024-10-05', '2024-10-06', 2, 'ASDASDASD');

-- Insertar una receta para la consulta médica con Id = 2
INSERT INTO Recetas (Vencimiento, NombreMedicamento, Cantidad, Frecuencia, ConsultaMedicaId)
VALUES 
('2024-12-01', 'Ibuprofeno', 20, 'Cada 8 horas', 1);

-- Insertar dos recetas para la consulta médica con Id = 3
INSERT INTO Recetas (Vencimiento, NombreMedicamento, Cantidad, Frecuencia, ConsultaMedicaId)
VALUES 
('2024-11-01', 'Paracetamol', 30, 'Cada 6 horas', 2),
('2024-11-01', 'Flodifrip', 15, 'Cada 8 horas', 2);

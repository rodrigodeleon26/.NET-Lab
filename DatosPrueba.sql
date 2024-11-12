--INSERTS DE NACHO
--Medicos
INSERT INTO Medicos (Nombres, Apellidos, Documento, Email, Telefono) VALUES
('Julius', 'Hibbert', 40123987, 'drJulius@gmail.com', '0987123456'),
('Alberto', 'Garcia', 31567894, 'drAlberto@gmail.com', '0912345678'),
('Carlos', 'Martinez', 34567891, 'drCarlosM@gmail.com', '0923456789'),
('Elena', 'Gomez', 32987654, 'drElenaG@gmail.com', '0987345678'),
('Luisa', 'Fernandez', 39876543, 'drLuisaF@gmail.com', '0954321789'),
('Pablo', 'Gutierrez', 30765432, 'drPabloG@gmail.com', '0945678123'),
('Ana', 'Lopez', 32456789, 'drAnaL@gmail.com', '0918765432'),
('Mario', 'Torres', 31234567, 'drMarioT@gmail.com', '0976543210'),
('Lucia', 'Mendez', 34876543, 'drLuciaM@gmail.com', '0934567890'),
('Oscar', 'Ramirez', 32345678, 'drOscarR@gmail.com', '0923456781'),
('Camila', 'Diaz', 33567894, 'drCamilaD@gmail.com', '0935678901'),
('Fernando', 'Ruiz', 31298765, 'drFernandoR@gmail.com', '0956781234'),
('Gabriela', 'Sanchez', 39871234, 'drGabrielaS@gmail.com', '0941234567'),
('Julio', 'Castro', 37654321, 'drJulioC@gmail.com', '0987654321'),
('Sofia', 'Vega', 30123456, 'drSofiaV@gmail.com', '0978123456'),
('Victor', 'Rojas', 30987654, 'drVictorR@gmail.com', '0912345670'),
('Marta', 'Pereira', 34567823, 'drMartaP@gmail.com', '0932456789'),
('Andrea', 'Morales', 39872345, 'drAndreaM@gmail.com', '0921345678'),
('Luis', 'Suarez', 31123456, 'drLuisS@gmail.com', '0976345123'),
('Patricia', 'Herrera', 31456789, 'drPatriciaH@gmail.com', '0912345789');

--Especialidades
INSERT INTO [HCE].[dbo].[Especialidades] (Nombre, Descripcion) VALUES
('Medicina General', 'Atencion primaria de salud para diagnostico y tratamiento general.'),
('Pediatria', 'Atencion medica para ni os y adolescentes.'),
('Cardiologia', 'Diagnostico y tratamiento de enfermedades del corazon y el sistema circulatorio.'),
('Dermatologia', 'Diagnostico y tratamiento de enfermedades de la piel.'),
('Ginecologia', 'Atencion medica para la salud reproductiva femenina.'),
('Oftalmologia', 'Diagnostico y tratamiento de enfermedades de los ojos.'),
('Neurologia', 'Diagnostico y tratamiento de trastornos del sistema nervioso.'),
('Psiquiatria', 'Atencion medica para la salud mental y trastornos psiquiatricos.'),
('Ortopedia', 'Tratamiento de enfermedades y lesiones del sistema musculo-esqueletico.'),
('Gastroenterologia', 'Diagnostico y tratamiento de enfermedades del sistema digestivo.');

--Especialidades de los medicos
    -- Medico 1 tiene todas las especialidades
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1);

    -- Medico 2 no tiene ninguna especialidad
    -- No se inserta ninguna fila para MedicoId = 2

    -- Medico 3 tiene solo una especialidad (Medicina General)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 3);

    -- Medico 4 tiene dos especialidades (Cardiologea y Dermatologia)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (3, 4), (4, 4);

    -- Medico 5 tiene tres especialidades (Pediatria, Oftalmologia, y Neurologia)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (2, 5), (6, 5), (7, 5);

    -- Medico 6 tiene todas las especialidades menos una (no tiene Psiquiatria)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 6), (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6);

    -- Medico 7 tiene solo una especialidad (Ginecologia)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (5, 7);

    -- Medico 8 tiene cuatro especialidades (Medicina General, Pediatria, Ortopedia, Gastroenterologia)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 8), (2, 8), (9, 8), (10, 8);

    -- Medico 9 tiene solo dos especialidades (Psiquiatria y Neurologia)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (7, 9), (8, 9);

    -- Medico 10 tiene todas las especialidades
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 10), (2, 10), (3, 10), (4, 10), (5, 10), (6, 10), (7, 10), (8, 10);

-- Calendarios para el Medico 1 (Julius Hibbert)

INSERT INTO [HCE].[dbo].[Consultorios] (Numero, Piso)
VALUES 
(101, 1);

-- Calendario 1 (Medicina General)
INSERT INTO [HCE].[dbo].[Calendarios] 
(HoraInicio, HoraFin, TiempoCita, ConsultorioId, DiasSemanaString, EspecialidadId, MedicoId, CantidadCitas) 
VALUES 
('08:00', '12:00', 30, 1, 'Lunes,Martes,Miercoles,Jueves,Viernes', 1, 1, 10);

-- Calendario 2 (Pediatria)
INSERT INTO [HCE].[dbo].[Calendarios] 
(HoraInicio, HoraFin, TiempoCita, ConsultorioId, DiasSemanaString, EspecialidadId, MedicoId, CantidadCitas) 
VALUES 
('09:00', '13:00', 30, 1, 'Lunes,Miercoles,Viernes', 2, 1, 8);

-- Calendario 3 (Medicina General)
INSERT INTO [HCE].[dbo].[Calendarios] 
(HoraInicio, HoraFin, TiempoCita, ConsultorioId, DiasSemanaString, EspecialidadId, MedicoId, CantidadCitas) 
VALUES 
('14:00', '18:00', 30, 1, 'Martes,Jueves', 1, 1, 12);

-- Calendario 4 (Pediatria)
INSERT INTO [HCE].[dbo].[Calendarios] 
(HoraInicio, HoraFin, TiempoCita, ConsultorioId, DiasSemanaString, EspecialidadId, MedicoId, CantidadCitas) 
VALUES 
('10:00', '14:00', 30, 1, 'Martes,Jueves,Viernes', 2, 1, 6);

-- Calendario 5 (Medicina General)
INSERT INTO [HCE].[dbo].[Calendarios] 
(HoraInicio, HoraFin, TiempoCita, ConsultorioId, DiasSemanaString, EspecialidadId, MedicoId, CantidadCitas) 
VALUES 
('15:00', '19:00', 30, 1, 'Lunes,Miercoles,Viernes', 1, 1, 15);


--Articulos
INSERT INTO [HCE].[dbo].[Articulos] (Nombre) VALUES
('Consulta Domiciliaria'),
('Atencion de Urgencias'),
('Procedimiento Quirurgico Menor'),
('Procedimiento Quirurgico Mayor'),
('Consulta de Seguimiento'),
('Consulta Preventiva'),
('Evaluacion Preoperatoria'),
('Chequeo Anual Completo'),
('Asesoramiento Nutricional'),
('Prueba de Laboratorio General');

--Seguros Medicos
INSERT INTO [HCE].[dbo].[SegurosMedicos] (Nombre, Descripcion) VALUES
('Fonasa', 'Seguro publico de salud'),
('Jubilados', 'Seguro de salud para personas jubiladas'),
('ISAPRE', 'Seguro privado de salud en Chile'),
('Plan Basico Salud', 'Cobertura basica para consultas y emergencias'),
('Seguro Plus', 'Cobertura ampliada con beneficios adicionales'),
('Seguro Familiar', 'Seguro que cubre a todo el grupo familiar'),
('Seguro Internacional', 'Cobertura de salud para atencion en el extranjero');

--Precios de Seguros
INSERT INTO [HCE].[dbo].[Precios] (CopagoId, SeguroMedicoId, PrecioBase, FechaInicio) VALUES
(NULL, 1, 150.00, '2023-01-01'), -- Fonasa viejo
(NULL, 1, 100.00, '2024-01-01'),  -- Fonasa nuevo
(NULL, 2, 180.00, '2023-01-01'), -- Jubilados viejo
(NULL, 2, 80.00, '2024-01-01'),   -- Jubilados nuevo
(NULL, 3, 220.00, '2023-01-01'), -- ISAPRE viejo
(NULL, 3, 200.00, '2024-01-01'),  -- ISAPRE nuevo
(NULL, 4, 150.00, '2024-01-01'),  -- Plan Basico Salud
(NULL, 5, 250.00, '2024-01-01'),  -- Seguro Plus
(NULL, 6, 180.00, '2024-01-01'),  -- Seguro Familiar
(NULL, 7, 300.00, '2024-01-01');  -- Seguro Internacional

--Copagos
INSERT INTO [HCE].[dbo].[Copagos] (ArticuloId, SeguroMedicoId, EspecialidadId) VALUES
(1, 1, 1),  -- Consulta Domiciliaria, Fonasa, Medicina General
(2, 1, 2),  -- Atencion de Urgencias, Fonasa, Pediatria
(3, 2, 3),  -- Procedimiento Quirurgico Menor, Jubilados, Cardiologia
(4, 2, 4),  -- Procedimiento Quirurgico Mayor, Jubilados, Dermatologia
(5, 3, 1),  -- Consulta de Seguimiento, ISAPRE, Medicina General
(6, 4, 5);  -- Consulta Preventiva, Plan Basico Salud, Ginecologia

-- Precios asociados a copagos especificos
INSERT INTO [HCE].[dbo].[Precios] (CopagoId, SeguroMedicoId, PrecioBase, FechaInicio) VALUES
(1, NULL, 120.00, '2023-01-01'), -- Precio historico para Consulta Domiciliaria (Fonasa, Medicina General)
(1, NULL, 130.00, '2024-01-01'), -- Nuevo precio para el mismo copago, efectivo en el futuro
(2, NULL, 200.00, '2023-06-01'), -- Precio para Atencion de Urgencias (Fonasa, Pediatria)
(3, NULL, 300.00, '2023-03-01'), -- Precio para Procedimiento Quirurgico Menor (Jubilados, Cardiologia)
(4, NULL, 500.00, '2023-09-01'); -- Precio para Procedimiento Quirurgico Mayor (Jubilados, Dermatologia)

--Fin inserts de nacho

INSERT INTO Pacientes (Nombres, Apellidos, Documento, FechaDeNacimiento, Direccion, Telefono, Email)
VALUES 
('Juan Carlos', 'Gonz lez P rez', '12345678', '1985-03-15', 'Av. Siempre Viva 742', '098123456', 'juancarlos.gp@example.com'),
('Mar a Fernanda', 'Mart nez L pez', '87654321', '1992-07-22', 'Calle Falsa 123', '091234567', 'maria.fernanda@example.com'),
('Jos  Luis', 'Rodr guez S nchez', '65432109', '1978-01-05', 'Calle Los Pinos 456', '098765432', 'jose.luis@example.com'),
('Ana Isabel', 'Ram rez D az', '34567890', '1989-12-10', 'Calle Las Rosas 789', '095123987', 'ana.isabel@example.com'),
('Carlos Eduardo', 'Hern ndez Castro', '11223344', '1975-04-28', 'Calle Sol 321', '093987654', 'carlos.eduardo@example.com'),
('Luc a Paola', 'Fern ndez Torres', '99887766', '2000-09-15', 'Avenida del Parque 22', '099234567', 'lucia.paola@example.com'),
('Miguel  ngel', 'Vargas Dom nguez', '55667788', '1965-11-30', 'Calle del R o 567', '098654321', 'miguel.angel@example.com'),
('Sof a Ver nica', 'M ndez Jim nez', '44332211', '1995-05-18', 'Avenida Libertador 99', '092345678', 'sofia.veronica@example.com'),
('Jorge Enrique', 'P rez Garc a', '33221144', '1980-02-25', 'Calle del Sol 876', '091098765', 'jorge.enrique@example.com'),
('Claudia Patricia', 'L pez Ruiz', '12344321', '1999-08-07', 'Calle 8 de Octubre 543', '094567890', 'claudia.patricia@example.com');

INSERT INTO CitasMedicas (Fecha, Estado, CalendarioId, PacienteId, ConsultoriosId, ConsultaMedicaId)
VALUES 
(GETDATE(), 'Completada', NULL, 1, NULL, 1),
(GETDATE(), 'Completada', NULL, 2, NULL, 2),
(GETDATE(), 'Completada', NULL, 1, NULL, 3),
(GETDATE(), 'Completada', NULL, 1, NULL, 4),
(GETDATE(), 'Agendada', 1, 5, NULL, NULL),
(GETDATE(), 'Agendada', 2, 6, NULL, NULL),
(GETDATE(), 'Agendada', 3, 7, NULL, NULL),
(GETDATE(), 'Agendada', 4, 8, NULL, NULL),
(GETDATE(), 'Agendada', 5, 9, NULL, NULL),
(GETDATE(), 'Agendada', NULL, 10, NULL, NULL);

INSERT INTO ConsultasMedicas (Descripcion, Diagnostico, CitaMedicaId)
VALUES 
('Consulta general sobre dolor de cabeza', 'Diagnóstico de migraña leve', 1),
('Revisión de control post-operatorio', 'Diagnóstico de recuperación satisfactoria', 2),
('Consulta por dolor abdominal', 'Diagnóstico de gastritis', 3),
('Consulta por dolor de espalda', 'Diagnóstico de lumbalgia', 4);

-- Insertar estudios para la consulta médica con Id = 3
INSERT INTO Estudios (Nombre, Descripcion, FechaRealizado, FechaResultado, ImagenUrl, ConsultaMedicaId)
VALUES 
('Radiografía de Tórax', 'Estudio de imagen del tórax', '2024-10-01', '2024-10-02', 'DSADFASD', 1),
('Examen de Sangre', 'Prueba completa de laboratorio', '2024-10-01', '2024-10-02', 'ASDASDASD', 1),
('Ultrasonido Abdominal', 'Exploración del abdomen', '2024-10-05', '2024-10-06', 'ASDASDASD', 2),
('Endoscopia', 'Exploración del tracto digestivo', '2024-10-10', '2024-10-11', 'ENDOSCOPIA_URL', 3),
('Resonancia Magnética', 'Exploración de la columna vertebral', '2024-10-12', '2024-10-13', 'RESONANCIA_URL', 4);

-- Insertar recetas para las consultas médicas con Id = 3 y 4
INSERT INTO Recetas (NombreMedicamento, Vencimiento, Cantidad, Frecuencia, ConsultaMedicaId)
VALUES 
('Paracetamol', GETDATE(), '500', 'Cada 8 horas', 2),
('Ibuprofeno', GETDATE(), '400', 'Cada 6 horas', 2),
('Omeprazol', GETDATE(), '20', 'Cada 24 horas', 3),
('Diclofenaco', GETDATE(), '50', 'Cada 12 horas', 4);
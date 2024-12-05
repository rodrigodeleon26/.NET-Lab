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
('Medicina General', 'Atención primaria de salud para diagnóstico y tratamiento general.'),
('Pediatría', 'Atención médica para niños y adolescentes.'),
('Cardiología', 'Diagnóstico y tratamiento de enfermedades del corazón y el sistema circulatorio.'),
('Dermatología', 'Diagnóstico y tratamiento de enfermedades de la piel.'),
('Ginecología', 'Atención médica para la salud reproductiva femenina.'),
('Oftalmología', 'Diagnóstico y tratamiento de enfermedades de los ojos.'),
('Neurología', 'Diagnóstico y tratamiento de trastornos del sistema nervioso.'),
('Psiquiatría', 'Atención médica para la salud mental y trastornos psiquiátricos.'),
('Ortopedia', 'Tratamiento de enfermedades y lesiones del sistema músculo-esquelético.'),
('Gastroenterología', 'Diagnóstico y tratamiento de enfermedades del sistema digestivo.');

--Especialidades de los medicos
    -- Médico 1 tiene todas las especialidades
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1);

    -- Médico 2 no tiene ninguna especialidad
    -- No se inserta ninguna fila para MedicoId = 2

    -- Médico 3 tiene solo una especialidad (Medicina General)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 3);

    -- Médico 4 tiene dos especialidades (Cardiología y Dermatología)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (3, 4), (4, 4);

    -- Médico 5 tiene tres especialidades (Pediatría, Oftalmología, y Neurología)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (2, 5), (6, 5), (7, 5);

    -- Médico 6 tiene todas las especialidades menos una (no tiene Psiquiatría)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 6), (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6);

    -- Médico 7 tiene solo una especialidad (Ginecología)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (5, 7);

    -- Médico 8 tiene cuatro especialidades (Medicina General, Pediatría, Ortopedia, Gastroenterología)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 8), (2, 8), (9, 8), (10, 8);

    -- Médico 9 tiene solo dos especialidades (Psiquiatría y Neurología)
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (7, 9), (8, 9);

    -- Médico 10 tiene todas las especialidades
    INSERT INTO [HCE].[dbo].[EspecialidadesMedicos] (EspecialidadId, MedicoId) VALUES
    (1, 10), (2, 10), (3, 10), (4, 10), (5, 10), (6, 10), (7, 10), (8, 10);

--Articulos
INSERT INTO [HCE].[dbo].[Articulos] (Nombre) VALUES
('Consulta Domiciliaria'),
('Atención de Urgencias'),
('Procedimiento Quirúrgico Menor'),
('Procedimiento Quirúrgico Mayor'),
('Consulta de Seguimiento'),
('Consulta Preventiva'),
('Evaluación Preoperatoria'),
('Chequeo Anual Completo'),
('Asesoramiento Nutricional'),
('Prueba de Laboratorio General');

--Seguros Médicos
INSERT INTO [HCE].[dbo].[SegurosMedicos] (Nombre, Descripcion) VALUES
('Fonasa', 'Seguro público de salud'),
('Jubilados', 'Seguro de salud para personas jubiladas'),
('ISAPRE', 'Seguro privado de salud en Chile'),
('Plan Básico Salud', 'Cobertura básica para consultas y emergencias'),
('Seguro Plus', 'Cobertura ampliada con beneficios adicionales'),
('Seguro Familiar', 'Seguro que cubre a todo el grupo familiar'),
('Seguro Internacional', 'Cobertura de salud para atención en el extranjero');

--Precios de Seguros
INSERT INTO [HCE].[dbo].[Precios] (CopagoId, SeguroMedicoId, PrecioBase, FechaInicio) VALUES
(NULL, 1, 150.00, '2023-01-01'), -- Fonasa viejo
(NULL, 1, 100.00, '2024-01-01'),  -- Fonasa nuevo
(NULL, 2, 180.00, '2023-01-01'), -- Jubilados viejo
(NULL, 2, 80.00, '2024-01-01'),   -- Jubilados nuevo
(NULL, 3, 220.00, '2023-01-01'), -- ISAPRE viejo
(NULL, 3, 200.00, '2024-01-01'),  -- ISAPRE nuevo
(NULL, 4, 150.00, '2024-01-01'),  -- Plan Básico Salud
(NULL, 5, 250.00, '2024-01-01'),  -- Seguro Plus
(NULL, 6, 180.00, '2024-01-01'),  -- Seguro Familiar
(NULL, 7, 300.00, '2024-01-01');  -- Seguro Internacional

--Copagos
INSERT INTO [HCE].[dbo].[Copagos] (ArticuloId, SeguroMedicoId, EspecialidadId) VALUES
(1, 1, 1),  -- Consulta Domiciliaria, Fonasa, Medicina General
(2, 1, 2),  -- Atención de Urgencias, Fonasa, Pediatría
(3, 2, 3),  -- Procedimiento Quirúrgico Menor, Jubilados, Cardiología
(4, 2, 4),  -- Procedimiento Quirúrgico Mayor, Jubilados, Dermatología
(5, 3, 1),  -- Consulta de Seguimiento, ISAPRE, Medicina General
(6, 4, 5);  -- Consulta Preventiva, Plan Básico Salud, Ginecología

-- Precios asociados a copagos específicos
INSERT INTO [HCE].[dbo].[Precios] (CopagoId, SeguroMedicoId, PrecioBase, FechaInicio) VALUES
(1, NULL, 120.00, '2023-01-01'), -- Precio histórico para Consulta Domiciliaria (Fonasa, Medicina General)
(1, NULL, 130.00, '2024-01-01'), -- Nuevo precio para el mismo copago, efectivo en el futuro
(2, NULL, 200.00, '2023-06-01'), -- Precio para Atención de Urgencias (Fonasa, Pediatría)
(3, NULL, 300.00, '2023-03-01'), -- Precio para Procedimiento Quirúrgico Menor (Jubilados, Cardiología)
(4, NULL, 500.00, '2023-09-01'); -- Precio para Procedimiento Quirúrgico Mayor (Jubilados, Dermatología)

--Fin inserts de nacho

-- Nuevos Inserts de Nacho

-- Consultorios
INSERT INTO [HCE].[dbo].[Consultorios] ([Numero], [Piso]) VALUES
(101, 1), -- ID 1
(102, 1), -- ID 2
(201, 2), -- ID 3
(202, 2), -- ID 4
(301, 3), -- ID 5
(302, 3); -- ID 6

-- Calendarios
INSERT INTO [HCE].[dbo].[Calendarios] 
([HoraInicio], [HoraFin], [TiempoCita], [CantidadCitas], [DiasSemanaString], [EspecialidadId], [MedicoId], [ConsultorioId], [Activo]) 
VALUES
('08:00:00', '12:00:00', 30, 8, 'Lunes,Martes,Miércoles,Jueves,Viernes,Sábado', 1, 1, 1, 1), -- Dr. Hibbert, Medicina General, Consultorio ID 1
('13:00:00', '17:00:00', 30, 8, 'Lunes,Martes,Miércoles,Jueves,Viernes', 2, 1, 2, 1), -- Dr. Hibbert, Pediatría, Consultorio ID 2
('09:00:00', '12:00:00', 20, 9, 'Lunes,Miércoles,Viernes', 1, 3, 3, 1), -- Dr. Martinez, Medicina General, Consultorio ID 3
('14:00:00', '18:00:00', 15, 16, 'Lunes,Martes,Miércoles,Jueves', 3, 4, 4, 1), -- Dr. Gomez, Cardiología, Consultorio ID 4
('08:00:00', '11:00:00', 20, 9, 'Martes,Jueves,Sábado', 5, 5, 5, 1), -- Dr. Fernandez, Oftalmología, Consultorio ID 5
('12:00:00', '16:00:00', 25, 9, 'Lunes,Miércoles,Viernes', 7, 9, 6, 1); -- Dr. Ramirez, Psiquiatría, Consultorio ID 6

-- Pacientes
INSERT INTO [HCE].[dbo].[Pacientes] 
([Nombres], [Apellidos], [Documento], [FechaDeNacimiento], [Direccion], [Telefono], [Email], [Activo]) 
VALUES
('Bart', 'Simpson', 12345678, '2010-04-01', '742 Evergreen Terrace', '0987654321', 'bart.simpson@gmail.com', 1),
('Homer', 'Simpson', 12345677, '1980-03-24', '742 Evergreen Terrace', '0987654323', 'homer.simpson@gmail.com', 1),
('Lisa', 'Simpson', 12345679, '2012-05-09', '742 Evergreen Terrace', '0987654322', 'lisa.simpson@gmail.com', 1),
('Marge', 'Simpson', 12345676, '1982-01-12', '742 Evergreen Terrace', '0987654324', 'marge.simpson@gmail.com', 1);

-- Consultas Médicas
INSERT INTO [HCE].[dbo].[ConsultasMedicas] ([Descripcion], [Diagnostico]) VALUES
('Consulta por dolor de cabeza frecuente', 'Migraña'),
('Chequeo general de salud', 'Sin anomalías'),
('Control de presión arterial', 'Hipertensión leve');

-- Citas Médicas
INSERT INTO [HCE].[dbo].[CitasMedicas] 
([Fecha], [Estado], [CalendarioId], [PacienteId], [ConsultoriosId], [ConsultaMedicaId], [CopagoId]) 
VALUES
('2024-12-07T08:30:00', 'Agendada', 1, 'm4R0rUAW4REnW0XPhHfDCw==', 1, NULL, 1), -- Bart con Dr. Hibbert
('2024-12-05T09:30:00', 'Completada', 3, '3xJQi2TwslfHECDe7X6Ewg==', 3, 1, 1), -- Homer con Dr. Martinez
('2024-12-04T09:30:00', 'Completada', 3, 'm4R0rUAW4REnW0XPhHfDCw==', 3, 2, 1), -- Bart con Dr. Martinez
('2024-12-03T09:30:00', 'Completada', 3, 'm4R0rUAW4REnW0XPhHfDCw==', 3, 3, 1), -- Bart con Dr. Martinez
('2024-12-05T10:30:00', 'Agendada', 1, 'm4R0rUAW4REnW0XPhHfDCw==', 5, NULL, 1); -- Bart con Dr. Fernandez

-- Contratos
INSERT INTO [HCE].[dbo].[Contratos] 
([FechaInicio], [Activo], [PacienteId], [SeguroMedicoId]) 
VALUES
('2024-01-01', 0, 1, 1), -- Bart con Fonasa inactivo
('2024-01-01', 1, 2, 2), -- Homer con Jubilados
('2024-01-01', 1, 3, 3), -- Lisa con ISAPRE
('2024-01-01', 1, 4, 4); -- Marge con Plan Básico Salud


-- Notificaciones
INSERT INTO [HCE].[dbo].[Notificaciones] ([Mensaje], [FechaEnvio], [Visto], [PacienteId]) VALUES
('Su cita ha sido confirmada', '2024-11-01', 0, 1),
('Recordatorio de cita médica', '2024-11-02', 0, 2);

-- Recetas
INSERT INTO [HCE].[dbo].[Recetas] 
([Vencimiento], [NombreMedicamento], [Cantidad], [Frecuencia], [ConsultaMedicaId]) 
VALUES
('2024-12-01', 'Paracetamol', 20, 'Cada 8 horas', 1),
('2024-12-15', 'Ibuprofeno', 15, 'Cada 12 horas', 2),
('2024-11-30', 'Loratadina', 10, 'Diaria', 3);

-- Estudios
INSERT INTO [HCE].[dbo].[Estudios] 
([Nombre], [Descripcion], [FechaRealizado], [FechaResultado], [ImagenUrl], [ConsultaMedicaId]) 
VALUES
('Radiografía', 'Estudio de rayos X para evaluar fracturas', '2024-11-03', '2024-11-04', 'mtQbL/FQaymYId8LfgxxlfAuwvZc7q7uOsuocpnc8ChxePPB4F4JKiDLH171FX+f3mEbK+dQwjQmtTRunBy0mA==', 1),
('Análisis de Sangre', 'Evaluación general de salud', '2024-11-03', '2024-11-05', 'mtQbL/FQaymYId8LfgxxlfAuwvZc7q7uOsuocpnc8ChxePPB4F4JKiDLH171FX+f3mEbK+dQwjQmtTRunBy0mA==', 1);

INSERT INTO [HCE].[dbo].[PagosPayPal]
([linkPago], [pagoId])
VALUES
('https://www.sandbox.paypal.com/checkoutnow?token=9N730399TM3784443', '9N730399TM3784443'); -- id 1

INSERT INTO [HCE].[dbo].[PagosPayPal]
([linkPago], [pagoId])
VALUES
('https://www.sandbox.paypal.com/checkoutnow?token=3BT325782F343883J', '3BT325782F343883J'); -- id 2

INSERT INTO [HCE].[dbo].[PagosPayPal]
([linkPago], [pagoId])
VALUES
('https://www.sandbox.paypal.com/checkoutnow?token=6YX60823H5376410F', '6YX60823H5376410F'); -- id 3

INSERT INTO [HCE].[dbo].[PagosPayPal]
([linkPago], [pagoId])
VALUES
('https://www.sandbox.paypal.com/checkoutnow?token=80K32528XF911262D', '80K32528XF911262D'); -- id 4

INSERT INTO [HCE].[dbo].[PagosPayPal]
([linkPago], [pagoId])
VALUES
('https://www.sandbox.paypal.com/checkoutnow?token=86U13026KW492120K', '86U13026KW492120K'); -- id 5 (enero 2025 bart)



INSERT INTO [HCE].[dbo].[Facturas] 
([Fecha], [Monto], [Pago], [FechaPago], [PacienteId], [Descripcion], [PagoPayPalId]) 
VALUES
('2024-11-04 00:00:00.0000000', 50, 0, NULL, 2, 'Consulta médica general (Completada)', 1), -- Homer consulta 11
('2024-12-01 00:00:00.0000000', 250, 0, NULL, 2, 'Mensualidad de seguro médico: Jubilados', 1), -- Homer mensualidad 12
('2024-12-01 00:00:00.0000000', 150, 0, NULL, 1, 'Mensualidad de seguro médico: Fonasa', 4), -- Bart mensualidad 12
('2024-11-11 00:00:00.0000000', 150, 0, NULL, 1, 'Consulta medica', 4), -- Bart consulta 11
('2024-11-01 00:00:00.0000000', 150, 0, NULL, 1, 'Mensualidad de seguro médico: Fonasa', 3), -- Bart mensualidad 11
('2024-10-01 00:00:00.0000000', 150, 0, NULL, 1, 'Mensualidad de seguro médico: Fonasa', 2); -- Bart mensualidad 10


--USARIOS
    INSERT INTO [HCE].[dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], 
    [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], 
    [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [PacienteId], [RefreshToken], 
    [RefreshTokenExpiryTime], [TwoFactorAuthKey], [MedicoId]) 
    VALUES
    ('0d4f5539-e556-4e7d-ab34-d536b73ff22a', 'drCarlosM@gmail.com', 'DRCARLOSM@GMAIL.COM', 
    'drCarlosM@gmail.com', 'DRCARLOSM@GMAIL.COM', 1, 
    'AQAAAAIAAYagAAAAEIcFD5R2sndQ0y2AWDcU3/2bjcKucr5DB+p4AGmmrVFbBuYaGjWR0OijvGZx22DYSw==', 
    'Q4SYV7ACHQVGPBOS5XJZIOZLNZPYQZK5', '862b245c-ea41-48c0-b683-51457c17a751', 
    NULL, 0, 0, NULL, 1, 0, 'drCarlosM@gmail.com', NULL, NULL, '0001-01-01 00:00:00.0000000', NULL, 3),
    
    ('4af45a2e-d2f6-43ad-8cf8-78b55fc008ed', 'bartsimsonnet@gmail.com', 'BARTSIMSONNET@GMAIL.COM', 
    'bartsimsonnet@gmail.com', 'BARTSIMSONNET@GMAIL.COM', 1, 
    'AQAAAAIAAYagAAAAEDoH9Fb+YK2GV3yEah14xKWsmZUsZbgAfrn2gdAnZbeY71UXrm51r4fNor1rDpt/Cg==', 
    'XMO3DQ3QG3L2PMW7QOYPRDQMWV5XZIEX', 'db04858e-cfed-4512-838d-3ff199ecc0cc', 
    NULL, 0, 0, NULL, 1, 0, 'bartsimsonnet@gmail.com', 1, NULL, '0001-01-01 00:00:00.0000000', NULL, NULL),
    
    ('69225179-d778-4be3-ba42-0115fd48e356', 'admin@gmail.com', 'ADMIN@GMAIL.COM', 
    'admin@gmail.com', 'ADMIN@GMAIL.COM', 1, 
    'AQAAAAIAAYagAAAAED6YtqV7aeP4YxKznIo8BXPqHDyoHFtt0MvXVl78qXVguV4G3+eW3crKrEBCeujhWg==', 
    'T6K65UPRSTORQATAHHDAOALJYWCDYJ2T', '44c5a22e-dd3c-4d81-b12b-660eb5459616', 
    NULL, 0, 0, NULL, 1, 0, 'admin@gmail.com', NULL, NULL, '0001-01-01 00:00:00.0000000', NULL, NULL),
    
    ('d581ae5d-cfb3-48d5-9241-1c6d44c6ac18', 'homer.simpson@gmail.com', 'HOMER.SIMPSON@GMAIL.COM', 
    'homer.simpson@gmail.com', 'HOMER.SIMPSON@GMAIL.COM', 1, 
    'AQAAAAIAAYagAAAAEPfTV5QbT76+EvGslgzGbh5D4Usdp5i0aHRD1ZPYwvGYEswa5+N8d26eaQKCS5raEg==', 
    'O3DAJMQMUAXG3JGOBUFICH3Z3CAYMVXL', '65c272cd-3250-4491-85b8-a6732d7ecb4c', 
    NULL, 0, 0, NULL, 1, 0, 'homer.simpson@gmail.com', 2, NULL, '0001-01-01 00:00:00.0000000', NULL, NULL),
    
    ('f4b2c112-a394-4f31-911b-87a6ea9d6772', 'drJulius@gmail.com', 'DRJULIUS@GMAIL.COM', 
    'drJulius@gmail.com', 'DRJULIUS@GMAIL.COM', 1, 
    'AQAAAAIAAYagAAAAEJa59M6y5a9loDhN0a+nN6xDSAjhWrYsVOuehCYg8TGp2WPWJ0XkTfGiYj8whq6m5Q==', 
    'OACNCEA357TNRQSSKJXY6YZUNYLBSBTQ', 'da50465c-7e62-4d6e-b41d-c8453b02bc68', 
    NULL, 0, 0, NULL, 1, 0, 'drJulius@gmail.com', NULL, NULL, '0001-01-01 00:00:00.0000000', NULL, 1);

-- Roles
    INSERT INTO [HCE].[dbo].[AspNetUserRoles] ([UserId], [RoleId]) 
    VALUES
    ('69225179-d778-4be3-ba42-0115fd48e356', 1), -- admin@gmail.com
    ('0d4f5539-e556-4e7d-ab34-d536b73ff22a', 2), -- drCarlosM@gmail.com
    ('f4b2c112-a394-4f31-911b-87a6ea9d6772', 2), -- drJulius@gmail.com
    ('4af45a2e-d2f6-43ad-8cf8-78b55fc008ed', 3), -- bart.simpson@gmail.com
    ('d581ae5d-cfb3-48d5-9241-1c6d44c6ac18', 3); -- homer.simpson@gmail.com


-- Fin de Inserts de Nacho




-- INSERT INTO Pacientes (Nombres, Apellidos, Documento, FechaDeNacimiento, Direccion, Telefono, Email)
-- VALUES 
-- ('Juan', 'Gonzalez', '12345678', '1985-03-15', 'Av. Siempre Viva 742', '098123456', 'juancarlos.gp@example.com'),
-- ('María', 'Martínez', '87654321', '1992-07-22', 'Calle Falsa 123', '091234567', 'maria.fernanda@example.com'),
-- ('José Luis', 'Rodríguez', '65432109', '1978-01-05', 'Calle Los Pinos 456', '098765432', 'jose.luis@example.com'),
-- ('Ana', 'Ramírez', '34567890', '1989-12-10', 'Calle Las Rosas 789', '095123987', 'ana.isabel@example.com'),
-- ('Eduardo', 'Castro', '11223344', '1975-04-28', 'Calle Sol 321', '093987654', 'carlos.eduardo@example.com'),
-- ('Paola', 'Torres', '99887766', '2000-09-15', 'Avenida del Parque 22', '099234567', 'lucia.paola@example.com'),
-- ('Miguel', 'Vargas', '55667788', '1965-11-30', 'Calle del Río 567', '098654321', 'miguel.angel@example.com'),
-- ('Sofía', 'Méndez', '44332211', '1995-05-18', 'Avenida Libertador 99', '092345678', 'sofia.veronica@example.com'),
-- ('Jorge Enrique', 'Pérez', '33221144', '1980-02-25', 'Calle del Sol 876', '091098765', 'jorge.enrique@example.com'),
-- ('Patricia', 'Ruiz', '12344321', '1999-08-07', 'Calle 8 de Octubre 543', '094567890', 'claudia.patricia@example.com');

-- INSERT INTO CitasMedicas (Fecha, Estado, CalendarioId, ConsultorioId, PacienteId)
-- VALUES 
-- (GETDATE(), 'Completada', NULL, NULL, 1),
-- (GETDATE(), 'Completada', NULL, NULL, 2),
-- (GETDATE(), 'Agendada', NULL, NULL, 3),
-- (GETDATE(), 'Agendada', NULL, NULL, 4),
-- (GETDATE(), 'Agendada', NULL, NULL, 5),
-- (GETDATE(), 'Agendada', NULL, NULL, 6),
-- (GETDATE(), 'Agendada', NULL, NULL, 7),
-- (GETDATE(), 'Agendada', NULL, NULL, 8),
-- (GETDATE(), 'Agendada', NULL, NULL, 9),
-- (GETDATE(), 'Agendada', NULL, NULL, 10);

-- INSERT INTO ConsultasMedicas (Descripcion, Diagnostico, CitaMedicaId)
-- VALUES 
-- ('Consulta general sobre dolor de cabeza', 'Diagn�stico de migra�a leve', 1),
-- ('Revisi�n de control post-operatorio', 'Diagn�stico de recuperaci�n satisfactoria', 2);

-- -- Insertar dos estudios para la consulta m�dica con Id = 2
-- INSERT INTO Estudios (Nombre, Descripcion, FechaRealizado, FechaResultado, ConsultaMedicaId, ImagenUrl)
-- VALUES 
-- ('Radiograf�a de T�rax', 'Estudio de imagen del t�rax', '2024-10-01', '2024-10-02', 1, 'DSADFASD'),
-- ('Examen de Sangre', 'Prueba completa de laboratorio', '2024-10-01', '2024-10-02', 1, 'ASDASDASD');

-- -- Insertar un estudio para la consulta m�dica con Id = 3
-- INSERT INTO Estudios (Nombre, Descripcion, FechaRealizado, FechaResultado, ConsultaMedicaId, ImagenUrl)
-- VALUES 
-- ('Ultrasonido Abdominal', 'Exploraci�n del abdomen', '2024-10-05', '2024-10-06', 2, 'ASDASDASD');

-- -- Insertar una receta para la consulta m�dica con Id = 2
-- INSERT INTO Recetas (Vencimiento, NombreMedicamento, Cantidad, Frecuencia, ConsultaMedicaId)
-- VALUES 
-- ('2024-12-01', 'Ibuprofeno', 20, 'Cada 8 horas', 1);

-- -- Insertar dos recetas para la consulta m�dica con Id = 3
-- INSERT INTO Recetas (Vencimiento, NombreMedicamento, Cantidad, Frecuencia, ConsultaMedicaId)
-- VALUES 
-- ('2024-11-01', 'Paracetamol', 30, 'Cada 6 horas', 2),
-- ('2024-11-01', 'Flodifrip', 15, 'Cada 8 horas', 2);

USE [master]
GO
/****** Object:  Database [cadenaRestaurantes]    Script Date: 02/09/2024 8:27:28 ******/
CREATE DATABASE [cadenaRestaurantes]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'cadenaRestaurantes', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\cadenaRestaurantes.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'cadenaRestaurantes_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\cadenaRestaurantes_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [cadenaRestaurantes] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cadenaRestaurantes].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [cadenaRestaurantes] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET ARITHABORT OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [cadenaRestaurantes] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [cadenaRestaurantes] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET  DISABLE_BROKER 
GO
ALTER DATABASE [cadenaRestaurantes] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [cadenaRestaurantes] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [cadenaRestaurantes] SET  MULTI_USER 
GO
ALTER DATABASE [cadenaRestaurantes] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [cadenaRestaurantes] SET DB_CHAINING OFF 
GO
ALTER DATABASE [cadenaRestaurantes] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [cadenaRestaurantes] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [cadenaRestaurantes] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [cadenaRestaurantes] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [cadenaRestaurantes] SET QUERY_STORE = ON
GO
ALTER DATABASE [cadenaRestaurantes] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [cadenaRestaurantes]
GO
/****** Object:  Table [dbo].[employment_details]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employment_details](
	[id_detalle_contratacion] [int] IDENTITY(1,1) NOT NULL,
	[cedula] [varchar](10) NOT NULL,
	[fin_contrato] [date] NOT NULL,
	[inicio_jornada] [time](7) NOT NULL,
	[nit_restaurante] [bigint] NOT NULL,
	[fecha_creacion] [datetime] NOT NULL,
 CONSTRAINT [PK_detalle_contratacion] PRIMARY KEY CLUSTERED 
(
	[id_detalle_contratacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employment_log]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employment_log](
	[id_registro] [int] IDENTITY(1,1) NOT NULL,
	[empleado_modificado] [varchar](10) NOT NULL,
	[fin_contrato] [date] NOT NULL,
	[inicio_jornada] [time](7) NOT NULL,
	[nit_restaurante] [bigint] NOT NULL,
	[estado] [varchar](50) NOT NULL,
	[fecha_creacion] [datetime] NOT NULL,
 CONSTRAINT [PK_recruitment_log] PRIMARY KEY CLUSTERED 
(
	[id_registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cedula] [varchar](10) NOT NULL,
	[clave] [varchar](100) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[workers]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[workers](
	[cedula] [varchar](10) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[telefono] [varchar](50) NOT NULL,
	[correo] [varchar](80) NOT NULL,
	[cargo] [varchar](50) NOT NULL,
	[estado] [varchar](50) NOT NULL,
 CONSTRAINT [cedula_empleados] PRIMARY KEY CLUSTERED 
(
	[cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[employment_details] ON 

INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (6, N'1000000002', CAST(N'2025-08-11' AS Date), CAST(N'13:00:00' AS Time), 111111111, CAST(N'2024-08-11T01:17:30.853' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (7, N'1000000003', CAST(N'2025-08-11' AS Date), CAST(N'08:00:00' AS Time), 222222222, CAST(N'2024-08-11T09:22:49.907' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (8, N'2000000001', CAST(N'2025-08-11' AS Date), CAST(N'13:00:00' AS Time), 333333333, CAST(N'2024-08-11T09:32:32.703' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (9, N'1000000004', CAST(N'2026-08-21' AS Date), CAST(N'13:00:00' AS Time), 111111111, CAST(N'2024-08-21T09:17:23.797' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (10, N'2000000002', CAST(N'2025-08-21' AS Date), CAST(N'13:00:00' AS Time), 111111111, CAST(N'2024-08-20T00:00:00.000' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (11, N'2000000003', CAST(N'2025-08-21' AS Date), CAST(N'13:00:00' AS Time), 111111111, CAST(N'2024-08-20T00:00:00.000' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (12, N'2000000004', CAST(N'2025-08-12' AS Date), CAST(N'08:00:00' AS Time), 444444444, CAST(N'2024-08-12T10:02:36.383' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (13, N'1111111111', CAST(N'2025-08-20' AS Date), CAST(N'13:00:00' AS Time), 222222222, CAST(N'2024-08-20T11:33:57.130' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (14, N'2111111111', CAST(N'2025-08-20' AS Date), CAST(N'13:00:00' AS Time), 222222222, CAST(N'2024-08-20T11:40:08.617' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (15, N'1222222222', CAST(N'2025-08-20' AS Date), CAST(N'08:00:00' AS Time), 333333333, CAST(N'2024-08-20T14:21:16.687' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (16, N'1000000005', CAST(N'2025-08-20' AS Date), CAST(N'08:00:00' AS Time), 333333333, CAST(N'2024-08-20T14:23:37.087' AS DateTime))
INSERT [dbo].[employment_details] ([id_detalle_contratacion], [cedula], [fin_contrato], [inicio_jornada], [nit_restaurante], [fecha_creacion]) VALUES (18, N'2222222222', CAST(N'2025-08-24' AS Date), CAST(N'13:00:00' AS Time), 111111111, CAST(N'2024-08-24T01:18:20.620' AS DateTime))
SET IDENTITY_INSERT [dbo].[employment_details] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id], [cedula], [clave]) VALUES (8, N'1000000001', N'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a')
INSERT [dbo].[users] ([id], [cedula], [clave]) VALUES (9, N'1000000002', N'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a')
INSERT [dbo].[users] ([id], [cedula], [clave]) VALUES (10, N'1000000003', N'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a')
INSERT [dbo].[users] ([id], [cedula], [clave]) VALUES (11, N'2000000001', N'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a')
INSERT [dbo].[users] ([id], [cedula], [clave]) VALUES (12, N'2000000004', N'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a')
SET IDENTITY_INSERT [dbo].[users] OFF
GO
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1000000001', N'Super Admin', N'3200000001', N'superadmin@gmail.com', N'Superadmin', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1000000002', N'Administrador Uno', N'3200000002', N'admin1@gmail.com', N'Administrador', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1000000003', N'Administrador Dos', N'3200000003', N'admin2@gmail.com', N'Administrador', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1000000004', N'Chef', N'3200000005', N'personal1@gmail.com', N'Chef', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1000000005', N'Supervisor', N'3200000012', N'personal7@gmail.com', N'Supervisor', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1111111111', N'Repartidor', N'3200000009', N'personal4@gmail.com', N'Repartidor', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'1222222222', N'Lavaplatos', N'3200000011', N'personal6@gmail.com', N'Lavaplatos', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2000000001', N'Administrador Tres', N'3200000004', N'admin3@gmail.com', N'Administrador', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2000000002', N'Mesero', N'3200000006', N'personal2@gmail.com', N'Mesero', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2000000003', N'Cajero', N'3200000007', N'personal3@gmail.com', N'Cajero', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2000000004', N'Administrador Cuatro', N'3200000008', N'admin4@gmail.com', N'Administrador', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2111111111', N'Ayudante Cocina', N'3200000010', N'personal5@gmail.com', N'Ayudante Cocina', N'Activo')
INSERT [dbo].[workers] ([cedula], [nombre], [telefono], [correo], [cargo], [estado]) VALUES (N'2222222222', N'Auxiliar de Cocina', N'3200000013', N'personal8@gmail.com', N'Auxiliar de Cocina', N'Activo')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_users]    Script Date: 02/09/2024 8:27:29 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_users] ON [dbo].[users]
(
	[cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [correo_empleados]    Script Date: 02/09/2024 8:27:29 ******/
CREATE UNIQUE NONCLUSTERED INDEX [correo_empleados] ON [dbo].[workers]
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [telefono_empleados]    Script Date: 02/09/2024 8:27:29 ******/
CREATE UNIQUE NONCLUSTERED INDEX [telefono_empleados] ON [dbo].[workers]
(
	[telefono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[employment_details]  WITH CHECK ADD  CONSTRAINT [FK_employment_details_workers] FOREIGN KEY([cedula])
REFERENCES [dbo].[workers] ([cedula])
GO
ALTER TABLE [dbo].[employment_details] CHECK CONSTRAINT [FK_employment_details_workers]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_workers1] FOREIGN KEY([cedula])
REFERENCES [dbo].[workers] ([cedula])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_users_workers1]
GO
/****** Object:  StoredProcedure [dbo].[addEmploymentLog]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addEmploymentLog]
    @cedula NVARCHAR(10),
    @fecha DATE,
    @hora TIME,
    @nit BIGINT,
    @estado NVARCHAR(50),
    @creacion DATETIME   
AS   
BEGIN
INSERT INTO employment_log(empleado_modificado, fin_contrato, inicio_jornada, nit_restaurante, estado, fecha_creacion)
	VALUES(@cedula, @fecha, @hora, @nit,@estado, @creacion);
END
GO
/****** Object:  StoredProcedure [dbo].[addUser]    Script Date: 02/09/2024 8:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addUser]
    @cedula int,
    @clave nvarchar(100)
AS   
BEGIN
	INSERT INTO users (cedula, clave)
	VALUES( @cedula, @clave);
END
GO
USE [master]
GO
ALTER DATABASE [cadenaRestaurantes] SET  READ_WRITE 
GO

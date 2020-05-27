USE [master]
GO
/****** Object:  Database [Charity]    Script Date: 15-Apr-20 18:02:50 ******/
CREATE DATABASE [Charity]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Charity', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Charity.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Charity_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Charity_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Charity] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Charity].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Charity] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Charity] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Charity] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Charity] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Charity] SET ARITHABORT OFF 
GO
ALTER DATABASE [Charity] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Charity] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Charity] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Charity] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Charity] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Charity] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Charity] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Charity] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Charity] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Charity] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Charity] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Charity] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Charity] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Charity] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Charity] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Charity] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Charity] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Charity] SET RECOVERY FULL 
GO
ALTER DATABASE [Charity] SET  MULTI_USER 
GO
ALTER DATABASE [Charity] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Charity] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Charity] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Charity] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Charity] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Charity', N'ON'
GO
ALTER DATABASE [Charity] SET QUERY_STORE = OFF
GO
USE [Charity]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;
GO
USE [Charity]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PatronID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[CityEntityID] [int] NOT NULL,
	[Neighbourhood] [nvarchar](64) NULL,
	[Phone] [nvarchar](32) NULL,
	[Value] [nvarchar](max) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Asset]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asset](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PatronID] [int] NOT NULL,
	[TypeEntityID] [int] NULL,
	[EstimatedValue] [bigint] NULL,
	[Address] [nvarchar](512) NULL,
	[CreateUser] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[PatronID] [int] NOT NULL,
	[Path] [nvarchar](256) NULL,
	[Type] [tinyint] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [int] NULL,
	[ModifyDate] [datetime] NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](64) NULL,
	[Value] [nvarchar](32) NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Family]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Family](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PatronID] [int] NOT NULL,
	[FirstName] [nvarchar](32) NULL,
	[LastName] [nvarchar](32) NULL,
	[BirthDate] [datetime] NULL,
	[EmploymentStatus] [smallint] NULL,
	[Income] [int] NULL,
	[RelationEntityID] [int] NULL,
	[EducationStatus] [smallint] NOT NULL,
	[EducationEntityID] [int] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [int] NULL,
	[ModifyDate] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Sponsorship] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PatronID] [int] NOT NULL,
	[Title] [nvarchar](64) NULL,
	[Income] [int] NOT NULL,
	[Phone] [nvarchar](32) NULL,
	[Address] [nvarchar](512) NULL,
	[CreateUser] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreateUser] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Package]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Package](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageItem]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NOT NULL,
	[TypeEntityID] [int] NULL,
	[Amount] [bigint] NULL,
	[Title] [nvarchar](32) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_PackageItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patron]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patron](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[MaritalStatus] [int] NOT NULL,
	[Children] [int] NOT NULL,
	[BloodType] [nvarchar](8) NULL,
	[SpecialDisease] [nvarchar](64) NULL,
	[Religion] [nvarchar](16) NULL,
	[Sect] [nvarchar](16) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Patron] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionKey] [nvarchar](64) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](32) NULL,
	[LastName] [nvarchar](32) NULL,
	[FatherName] [nvarchar](32) NULL,
	[BirthDate] [date] NULL,
	[NationalNo] [nvarchar](16) NULL,
	[BirthPlace] [nvarchar](32) NULL,
	[RegNo] [nvarchar](16) NULL,
	[RegPlace] [nvarchar](16) NULL,
	[MobileNo] [nvarchar](16) NULL,
	[NationEntityID] [int] NULL,
	[EducationEntityID] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Picture]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Picture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[UserName] [nvarchar](32) NOT NULL,
	[Password] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 15-Apr-20 18:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[PermissionID] [int] NULL,
 CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Patron]    Script Date: 15-Apr-20 18:02:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Patron] ON [dbo].[Patron]
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD  CONSTRAINT [DF_Document_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Patron] FOREIGN KEY([PatronID])
REFERENCES [dbo].[Patron] ([ID])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Patron]
GO
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_Patron] FOREIGN KEY([PatronID])
REFERENCES [dbo].[Patron] ([ID])
GO
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Patron]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_Patron] FOREIGN KEY([PatronID])
REFERENCES [dbo].[Patron] ([ID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_Patron]
GO
ALTER TABLE [dbo].[Family]  WITH CHECK ADD  CONSTRAINT [FK_Sponsorship_Patron] FOREIGN KEY([PatronID])
REFERENCES [dbo].[Patron] ([ID])
GO
ALTER TABLE [dbo].[Family] CHECK CONSTRAINT [FK_Sponsorship_Patron]
GO
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Patron] FOREIGN KEY([PatronID])
REFERENCES [dbo].[Patron] ([ID])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Patron]
GO
ALTER TABLE [dbo].[Login]  WITH CHECK ADD  CONSTRAINT [FK_Login_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Login] CHECK CONSTRAINT [FK_Login_User]
GO
ALTER TABLE [dbo].[PackageItem]  WITH CHECK ADD  CONSTRAINT [FK_PackageItem_Package] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Package] ([ID])
GO
ALTER TABLE [dbo].[PackageItem] CHECK CONSTRAINT [FK_PackageItem_Package]
GO
ALTER TABLE [dbo].[Patron]  WITH CHECK ADD  CONSTRAINT [FK_Patron_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Patron] CHECK CONSTRAINT [FK_Patron_Person]
GO
ALTER TABLE [dbo].[Picture]  WITH CHECK ADD  CONSTRAINT [FK_Picture_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Picture] CHECK CONSTRAINT [FK_Picture_Person]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Person]
GO
ALTER TABLE [dbo].[UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permission] ([ID])
GO
ALTER TABLE [dbo].[UserPermission] CHECK CONSTRAINT [FK_UserPermission_Permission]
GO
ALTER TABLE [dbo].[UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserPermission_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserPermission] CHECK CONSTRAINT [FK_UserPermission_User]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'دارایی های فرد' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Asset'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'جدول حاوی اسناد و مدارک' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Document'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'افراد تحت تکفل' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Family'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'اطلاعات شغل و درآمد افراد' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Job'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'جدولی شامل اطلاعات بسته های حمایتی تعریف شده توسط موسسه' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Package'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ارزش ریالی کالای حمایتی' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PackageItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'وضعیت تاهل' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Patron', @level2type=N'COLUMN',@level2name=N'MaritalStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'تعداد فرزندان' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Patron', @level2type=N'COLUMN',@level2name=N'Children'
GO
USE [master]
GO
ALTER DATABASE [Charity] SET  READ_WRITE 
GO

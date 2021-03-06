USE [master]
GO
/****** Object:  Database [TimeLogger]    Script Date: 17. 08. 2017 22:15:20 ******/
CREATE DATABASE [TimeLogger]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TimeLogger', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TimeLogger.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TimeLogger_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TimeLogger_log.ldf' , SIZE = 7616KB , MAXSIZE = 6291456KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TimeLogger] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TimeLogger].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TimeLogger] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TimeLogger] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TimeLogger] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TimeLogger] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TimeLogger] SET ARITHABORT OFF 
GO
ALTER DATABASE [TimeLogger] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TimeLogger] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TimeLogger] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TimeLogger] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TimeLogger] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TimeLogger] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TimeLogger] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TimeLogger] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TimeLogger] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TimeLogger] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TimeLogger] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TimeLogger] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TimeLogger] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TimeLogger] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TimeLogger] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TimeLogger] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TimeLogger] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TimeLogger] SET RECOVERY FULL 
GO
ALTER DATABASE [TimeLogger] SET  MULTI_USER 
GO
ALTER DATABASE [TimeLogger] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TimeLogger] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TimeLogger] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TimeLogger] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TimeLogger] SET DELAYED_DURABILITY = DISABLED 
GO
USE [TimeLogger]
GO
/****** Object:  User [TimeLoggerApp]    Script Date: 17. 08. 2017 22:15:23 ******/
CREATE USER [TimeLoggerApp] FOR LOGIN [TimeLoggerApp]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [TimeLoggerApp]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [TimeLoggerApp]
GO
ALTER ROLE [db_datareader] ADD MEMBER [TimeLoggerApp]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [TimeLoggerApp]
GO

/****** Object:  Table [ACCOUNT]    Script Date: 17. 08. 2017 22:15:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ACCOUNT](
	[ACCOUNT_ID] [uniqueidentifier] NOT NULL,
	[DESCRIPTION] [nvarchar](250) NULL,
	[CREATED] [datetime] NOT NULL,
	[OWNER_USER_ID] [uniqueidentifier] NOT NULL,
	[ACTIVE_YN] [char](1) NOT NULL,
 CONSTRAINT [Key3] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ACCOUNT_USER]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ACCOUNT_USER](
	[ACCOUNT_USER_ID] [uniqueidentifier] NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[USER_ID] [uniqueidentifier] NOT NULL,
	[ACCOUNT_ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [Key6] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ASSIGNMENT]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ASSIGNMENT](
	[ASSIGNMENT_ID] [uniqueidentifier] NOT NULL,
	[DESCRIPTION] [nvarchar](max) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[PROJECT_ID] [uniqueidentifier] NULL,
	[USER_ID] [uniqueidentifier] NULL,
 CONSTRAINT [Key10] PRIMARY KEY CLUSTERED 
(
	[ASSIGNMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [AUDIT]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AUDIT](
	[AUDIT_ID] [uniqueidentifier] NOT NULL,
	[AUDIT_TYPE_IO] [char](1) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[USER_ID] [uniqueidentifier] NULL,
 CONSTRAINT [Key7] PRIMARY KEY CLUSTERED 
(
	[AUDIT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [BILLING]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BILLING](
	[BILLING_ID] [uniqueidentifier] NOT NULL,
	[PAY_PROVIDER_ID] [nvarchar](100) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[DATE_PAID] [datetime] NULL,
	[BILLING_OPTION_ID] [bigint] NOT NULL,
	[ACCOUNT_ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [Key4] PRIMARY KEY CLUSTERED 
(
	[BILLING_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [CD_ACCOUNT_TYPE]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CD_ACCOUNT_TYPE](
	[ACCOUNT_TYPE_ID] [smallint] IDENTITY(1,1) NOT NULL,
	[CODE] [char](4) NOT NULL,
	[DESCRIPTION] [nvarchar](200) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[VALID_FROM] [date] NOT NULL,
	[VALID_TO] [date] NULL,
 CONSTRAINT [Key2] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [CD_BILLING_OPTION]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CD_BILLING_OPTION](
	[BILLING_OPTION_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CODE] [char](4) NOT NULL,
	[DESCRIPTION] [nvarchar](200) NOT NULL,
	[PAYMENTS_MY] [char](1) NOT NULL,
	[PRICE] [decimal](8, 2) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[VALID_FROM] [date] NOT NULL,
	[VALID_TO] [date] NULL,
	[ACCOUNT_TYPE_ID] [smallint] NOT NULL,
 CONSTRAINT [Key1] PRIMARY KEY CLUSTERED 
(
	[BILLING_OPTION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [PASSWORD_RESET_REQUEST]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [PASSWORD_RESET_REQUEST](
	[PASSWORD_RESET_REQUEST_ID] [uniqueidentifier] NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[ACTIVE_YN] [char](1) NOT NULL,
	[USER_ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [Key11] PRIMARY KEY CLUSTERED 
(
	[PASSWORD_RESET_REQUEST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [PROJECT]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [PROJECT](
	[PROJECT_ID] [uniqueidentifier] NOT NULL,
	[CODE] [nvarchar](200) NOT NULL,
	[DESCRIPTION] [nvarchar](max) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[USER_ID] [uniqueidentifier] NULL,
 CONSTRAINT [Key9] PRIMARY KEY CLUSTERED 
(
	[PROJECT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [TIME_LOG]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TIME_LOG](
	[TIME_LOG_ID] [uniqueidentifier] NOT NULL,
	[FROM] [datetime] NOT NULL,
	[TO] [datetime] NULL,
	[DESCRIPTION] [nvarchar](max) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[ASSIGNMENT_ID] [uniqueidentifier] NULL,
	[USER_ID] [uniqueidentifier] NULL,
 CONSTRAINT [Key8] PRIMARY KEY CLUSTERED 
(
	[TIME_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [USER]    Script Date: 17. 08. 2017 22:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [USER](
	[USER_ID] [uniqueidentifier] NOT NULL,
	[EMAIL] [nvarchar](250) NOT NULL,
	[PASSWORD] [nvarchar](150) NOT NULL,
	[CREATED] [datetime] NOT NULL,
	[ACTIVE_YN] [char](1) NOT NULL,
 CONSTRAINT [Key5] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Relationship11]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship11] ON [ACCOUNT]
(
	[OWNER_USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship12]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship12] ON [ACCOUNT_USER]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship13]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship13] ON [ACCOUNT_USER]
(
	[ACCOUNT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship15]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship15] ON [ASSIGNMENT]
(
	[PROJECT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship18]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship18] ON [ASSIGNMENT]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship14]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship14] ON [AUDIT]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship10]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship10] ON [BILLING]
(
	[ACCOUNT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship9]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship9] ON [BILLING]
(
	[BILLING_OPTION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship8]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship8] ON [CD_BILLING_OPTION]
(
	[ACCOUNT_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship1]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship1] ON [PASSWORD_RESET_REQUEST]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_SEARCH]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_SEARCH] ON [PASSWORD_RESET_REQUEST]
(
	[USER_ID] ASC,
	[ACTIVE_YN] ASC,
	[CREATED] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship19]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship19] ON [PROJECT]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship16]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship16] ON [TIME_LOG]
(
	[ASSIGNMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship17]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_Relationship17] ON [TIME_LOG]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TIME_LOG_USER_ID_FROM]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_TIME_LOG_USER_ID_FROM] ON [TIME_LOG]
(
	[USER_ID] ASC,
	[FROM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_USER_EMAIL]    Script Date: 17. 08. 2017 22:15:26 ******/
CREATE NONCLUSTERED INDEX [IX_USER_EMAIL] ON [USER]
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [ACCOUNT] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [ACCOUNT] ADD  DEFAULT ('Y') FOR [ACTIVE_YN]
GO
ALTER TABLE [ACCOUNT_USER] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [ASSIGNMENT] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [AUDIT] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [BILLING] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [CD_ACCOUNT_TYPE] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [CD_BILLING_OPTION] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [PASSWORD_RESET_REQUEST] ADD  DEFAULT ('N') FOR [ACTIVE_YN]
GO
ALTER TABLE [PROJECT] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [TIME_LOG] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [USER] ADD  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [USER] ADD  DEFAULT ('Y') FOR [ACTIVE_YN]
GO
ALTER TABLE [ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_ACCOUNT_USER] FOREIGN KEY([OWNER_USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [ACCOUNT] CHECK CONSTRAINT [FK_ACCOUNT_USER]
GO
ALTER TABLE [ACCOUNT_USER]  WITH CHECK ADD  CONSTRAINT [FK_ACCOUNT_USER_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [ACCOUNT_USER] CHECK CONSTRAINT [FK_ACCOUNT_USER_ACCOUNT]
GO
ALTER TABLE [ACCOUNT_USER]  WITH CHECK ADD  CONSTRAINT [FK_ACCOUNT_USER_USER] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [ACCOUNT_USER] CHECK CONSTRAINT [FK_ACCOUNT_USER_USER]
GO
ALTER TABLE [ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [FK_ASSIGNENT_USER] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [ASSIGNMENT] CHECK CONSTRAINT [FK_ASSIGNENT_USER]
GO
ALTER TABLE [ASSIGNMENT]  WITH CHECK ADD  CONSTRAINT [FK_ASSIGNMENT_PROJECT] FOREIGN KEY([PROJECT_ID])
REFERENCES [PROJECT] ([PROJECT_ID])
GO
ALTER TABLE [ASSIGNMENT] CHECK CONSTRAINT [FK_ASSIGNMENT_PROJECT]
GO
ALTER TABLE [AUDIT]  WITH CHECK ADD  CONSTRAINT [FK_AUDIT_USER] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [AUDIT] CHECK CONSTRAINT [FK_AUDIT_USER]
GO
ALTER TABLE [BILLING]  WITH CHECK ADD  CONSTRAINT [FK_BILLING_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [BILLING] CHECK CONSTRAINT [FK_BILLING_ACCOUNT]
GO
ALTER TABLE [BILLING]  WITH CHECK ADD  CONSTRAINT [FK_BILLING_BILLING_OPTION] FOREIGN KEY([BILLING_OPTION_ID])
REFERENCES [CD_BILLING_OPTION] ([BILLING_OPTION_ID])
GO
ALTER TABLE [BILLING] CHECK CONSTRAINT [FK_BILLING_BILLING_OPTION]
GO
ALTER TABLE [CD_BILLING_OPTION]  WITH CHECK ADD  CONSTRAINT [FK_BILLING_OPTION_ACCOUNT_TYPE] FOREIGN KEY([ACCOUNT_TYPE_ID])
REFERENCES [CD_ACCOUNT_TYPE] ([ACCOUNT_TYPE_ID])
GO
ALTER TABLE [CD_BILLING_OPTION] CHECK CONSTRAINT [FK_BILLING_OPTION_ACCOUNT_TYPE]
GO
ALTER TABLE [PASSWORD_RESET_REQUEST]  WITH CHECK ADD  CONSTRAINT [Relationship1] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [PASSWORD_RESET_REQUEST] CHECK CONSTRAINT [Relationship1]
GO
ALTER TABLE [PROJECT]  WITH CHECK ADD  CONSTRAINT [FK_PROJECT_USER] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [PROJECT] CHECK CONSTRAINT [FK_PROJECT_USER]
GO
ALTER TABLE [TIME_LOG]  WITH CHECK ADD  CONSTRAINT [FK_TIME_LOG_ASSIGNMENT] FOREIGN KEY([ASSIGNMENT_ID])
REFERENCES [ASSIGNMENT] ([ASSIGNMENT_ID])
GO
ALTER TABLE [TIME_LOG] CHECK CONSTRAINT [FK_TIME_LOG_ASSIGNMENT]
GO
ALTER TABLE [TIME_LOG]  WITH CHECK ADD  CONSTRAINT [FK_TIME_LOG_USER] FOREIGN KEY([USER_ID])
REFERENCES [USER] ([USER_ID])
GO
ALTER TABLE [TIME_LOG] CHECK CONSTRAINT [FK_TIME_LOG_USER]
GO
USE [master]
GO
ALTER DATABASE [TimeLogger] SET  READ_WRITE 
GO

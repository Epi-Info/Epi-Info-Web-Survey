USE [OSELS_EIWS]
GO
/****** Object:  ForeignKey [FK_SurveyMetaData_lk_SurveyType]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_lk_SurveyType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] DROP CONSTRAINT [FK_SurveyMetaData_lk_SurveyType]
GO
/****** Object:  ForeignKey [FK_SurveyMetaData_Organization]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] DROP CONSTRAINT [FK_SurveyMetaData_Organization]
GO
/****** Object:  ForeignKey [FK_SurveyResponse_lk_Status]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_lk_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_lk_Status]
GO
/****** Object:  ForeignKey [FK_SurveyResponse_SurveyMetaData]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_SurveyMetaData]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_SurveyMetaData]
GO
/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_lk_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_lk_Status]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_SurveyMetaData]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_SurveyMetaData]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SurveyResponse_DateLastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [DF_SurveyResponse_DateLastUpdated]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponse]') AND type in (N'U'))
DROP TABLE [dbo].[SurveyResponse]
GO
/****** Object:  Table [dbo].[SurveyMetaData]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_lk_SurveyType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] DROP CONSTRAINT [FK_SurveyMetaData_lk_SurveyType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] DROP CONSTRAINT [FK_SurveyMetaData_Organization]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SurveyMetaData_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SurveyMetaData] DROP CONSTRAINT [DF_SurveyMetaData_DateCreated]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]') AND type in (N'U'))
DROP TABLE [dbo].[SurveyMetaData]
GO
/****** Object:  Table [dbo].[lk_Status]    Script Date: 08/23/2012 14:02:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[lk_Status]') AND type in (N'U'))
DROP TABLE [dbo].[lk_Status]
GO
/****** Object:  Table [dbo].[lk_SurveyType]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[lk_SurveyType]') AND type in (N'U'))
DROP TABLE [dbo].[lk_SurveyType]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 08/23/2012 14:02:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization]') AND type in (N'U'))
DROP TABLE [dbo].[Organization]
GO
--/****** Object:  User [EPIWS App User]    Script Date: 08/23/2012 14:02:28 ******/
--IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'EPIWS App User')
--DROP USER [EPIWS App User]
--GO
--/****** Object:  User [EPIWS App User]    Script Date: 08/23/2012 14:02:28 ******/
--IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'EPIWS App User')
--CREATE USER [EPIWS App User] FOR LOGIN [osels_epiws_appuser] WITH DEFAULT_SCHEMA=[dbo]
--GO
/****** Object:  Table [dbo].[Organization]    Script Date: 08/23/2012 14:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Organization](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationKey] [nvarchar](500) NOT NULL,
	[Organization] [nvarchar](400) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Organization] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[lk_SurveyType]    Script Date: 08/23/2012 14:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[lk_SurveyType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[lk_SurveyType](
	[SurveyTypeId] [int] NOT NULL,
	[SurveyType] [varchar](50) NULL,
 CONSTRAINT [PK_lk_SurveyType] PRIMARY KEY CLUSTERED 
(
	[SurveyTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[lk_Status]    Script Date: 08/23/2012 14:02:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[lk_Status]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[lk_Status](
	[StatusId] [int] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_lk_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SurveyMetaData]    Script Date: 08/23/2012 14:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SurveyMetaData](
	[SurveyId] [uniqueidentifier] NOT NULL,
	[SurveyNumber] [nvarchar](50) NULL,
	[SurveyTypeId] [int] NOT NULL,
	[ClosingDate] [datetime2](7) NOT NULL,
	[SurveyName] [nvarchar](500) NOT NULL,
	[OrganizationName] [nvarchar](500) NULL,
	[DepartmentName] [nvarchar](500) NULL,
	[IntroductionText] [nvarchar](max) NULL,
	[TemplateXML] [xml] NOT NULL,
	[ExitText] [nvarchar](max) NULL,
	[UserPublishKey] [uniqueidentifier] NOT NULL,
	[TemplateXMLSize] [bigint] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL CONSTRAINT [DF_SurveyMetaData_DateCreated]  DEFAULT (getdate()),
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_SurveyMetaData] PRIMARY KEY CLUSTERED 
(
	[SurveyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 08/23/2012 14:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponse]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SurveyResponse](
	[ResponseId] [uniqueidentifier] NOT NULL,
	[SurveyId] [uniqueidentifier] NOT NULL,
	[DateUpdated] [datetime2](7) NOT NULL CONSTRAINT [DF_SurveyResponse_DateLastUpdated]  DEFAULT (getdate()),
	[DateCompleted] [datetime2](7) NULL,
	[StatusId] [int] NOT NULL,
	[ResponseXML] [xml] NOT NULL,
	[ResponsePasscode] [nvarchar](30) NULL,
	[ResponseXMLSize] [bigint] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_SurveyResponse] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  ForeignKey [FK_SurveyMetaData_lk_SurveyType]    Script Date: 08/23/2012 14:02:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_lk_SurveyType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData]  WITH CHECK ADD  CONSTRAINT [FK_SurveyMetaData_lk_SurveyType] FOREIGN KEY([SurveyTypeId])
REFERENCES [dbo].[lk_SurveyType] ([SurveyTypeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_lk_SurveyType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] CHECK CONSTRAINT [FK_SurveyMetaData_lk_SurveyType]
GO
/****** Object:  ForeignKey [FK_SurveyMetaData_Organization]    Script Date: 08/23/2012 14:02:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData]  WITH CHECK ADD  CONSTRAINT [FK_SurveyMetaData_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([OrganizationId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyMetaData_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyMetaData]'))
ALTER TABLE [dbo].[SurveyMetaData] CHECK CONSTRAINT [FK_SurveyMetaData_Organization]
GO
/****** Object:  ForeignKey [FK_SurveyResponse_lk_Status]    Script Date: 08/23/2012 14:02:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_lk_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_lk_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[lk_Status] ([StatusId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_lk_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_lk_Status]
GO
/****** Object:  ForeignKey [FK_SurveyResponse_SurveyMetaData]    Script Date: 08/23/2012 14:02:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_SurveyMetaData]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_SurveyMetaData] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[SurveyMetaData] ([SurveyId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_SurveyMetaData]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_SurveyMetaData]
GO


/*********************************************************SurveyResponse*************************************************/

USE [OSELS_EIWS]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_lk_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_lk_Status]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyResponse_SurveyMetaData]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyResponse]'))
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [FK_SurveyResponse_SurveyMetaData]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SurveyResponse_DateLastUpdated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SurveyResponse] DROP CONSTRAINT [DF_SurveyResponse_DateLastUpdated]
END

GO

USE [OSELS_EIWS]
GO

/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 05/10/2012 14:56:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponse]') AND type in (N'U'))
DROP TABLE [dbo].[SurveyResponse]
GO

USE [OSELS_EIWS]
GO

/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 05/10/2012 14:56:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SurveyResponse](
	[ResponseId] [uniqueidentifier] NOT NULL,
	[SurveyId] [uniqueidentifier] NOT NULL,
	[DateUpdated] [datetime2](7) NOT NULL,
	[DateCompleted] [datetime2](7) NULL,
	[StatusId] [int] NOT NULL,
	[ResponseXML] [xml] NOT NULL,
	[ResponsePasscode] [nvarchar](30) NULL,
	[ResponseXMLSize] [bigint] NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_SurveyResponse] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_lk_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[lk_Status] ([StatusId])
GO

ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_lk_Status]
GO

ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_SurveyMetaData] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[SurveyMetaData] ([SurveyId])
GO

ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_SurveyMetaData]
GO

ALTER TABLE [dbo].[SurveyResponse] ADD  CONSTRAINT [DF_SurveyResponse_DateLastUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
GO



/************************************************Organization*********************************************************************/

USE [OSELS_EIWS]
GO

/****** Object:  Table [dbo].[Organization]    Script Date: 05/10/2012 14:58:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization]') AND type in (N'U'))
DROP TABLE [dbo].[Organization]
GO

USE [OSELS_EIWS]
GO

/****** Object:  Table [dbo].[Organization]    Script Date: 05/10/2012 14:58:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Organization](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationKey] [nvarchar](500) NOT NULL,
	[Organization] [nvarchar](200) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Organization] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





-- INFORMATION: The below section was NOT autogenerated by SQL Server


INSERT INTO [OSELS_EIWS].[dbo].[lk_Status]
           ([StatusId]
           ,[Status])
     VALUES
           (1
           ,'In Progress')
GO

INSERT INTO [OSELS_EIWS].[dbo].[lk_Status]
           ([StatusId]
           ,[Status])
     VALUES
           (2
           ,'In Progress (URL)')
GO

INSERT INTO [OSELS_EIWS].[dbo].[lk_Status]
           ([StatusId]
           ,[Status])
     VALUES
           (3
           ,'Complete')
GO
INSERT INTO [OSELS_EIWS].[dbo].[lk_SurveyType]
           ([SurveyTypeId]
           ,[SurveyType])
     VALUES
           (1
           ,'Single Response')
GO

INSERT INTO [OSELS_EIWS].[dbo].[lk_SurveyType]
           ([SurveyTypeId]
           ,[SurveyType])
     VALUES
           (2
           ,'Multiple Response')
GO

SET IDENTITY_INSERT [dbo].[Organization] ON
INSERT [dbo].[Organization] ([OrganizationId], [OrganizationKey], [Organization], [IsEnabled]) VALUES (39, N'qg1wbEdOm78Y7mPp6twUjvPeLQsmJbtTLnLmRgy7E2Wnl+987A1uvlvmqtydmUlk', N'EIWS Test Organization', 1)
SET IDENTITY_INSERT [dbo].[Organization] OFF
GO
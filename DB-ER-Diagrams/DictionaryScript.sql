USE [master]
GO
/****** Object:  Database [Dictionary]    Script Date: 12.7.2013 г. 09:58:56 ч. ******/
CREATE DATABASE [Dictionary]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Dictionary', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Dictionary.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Dictionary_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Dictionary_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Dictionary] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Dictionary].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Dictionary] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Dictionary] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Dictionary] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Dictionary] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Dictionary] SET ARITHABORT OFF 
GO
ALTER DATABASE [Dictionary] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Dictionary] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Dictionary] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Dictionary] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Dictionary] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Dictionary] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Dictionary] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Dictionary] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Dictionary] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Dictionary] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Dictionary] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Dictionary] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Dictionary] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Dictionary] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Dictionary] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Dictionary] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Dictionary] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Dictionary] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Dictionary] SET RECOVERY FULL 
GO
ALTER DATABASE [Dictionary] SET  MULTI_USER 
GO
ALTER DATABASE [Dictionary] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Dictionary] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Dictionary] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Dictionary] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Dictionary', N'ON'
GO
USE [Dictionary]
GO
/****** Object:  User [vkv]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
CREATE USER [vkv] FOR LOGIN [vkv] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [vkv]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [vkv]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [vkv]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [vkv]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [vkv]
GO
ALTER ROLE [db_datareader] ADD MEMBER [vkv]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [vkv]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [vkv]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [vkv]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[LanguageId] [int] NOT NULL,
	[Name] [nchar](20) NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[LanguageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PartsOfSpeech]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartsOfSpeech](
	[PartOfSpeech] [int] NOT NULL,
	[Name] [nchar](20) NOT NULL,
 CONSTRAINT [PK_PartsOfSpeech] PRIMARY KEY CLUSTERED 
(
	[PartOfSpeech] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WordAntonyms]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WordAntonyms](
	[WordId] [int] NOT NULL,
	[AntonymId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WordHypernyms]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WordHypernyms](
	[WordId] [int] NOT NULL,
	[HypernymId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Words]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Words](
	[WordId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Explanation] [text] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[PartOfSpeech] [int] NOT NULL,
 CONSTRAINT [PK_Words] PRIMARY KEY CLUSTERED 
(
	[WordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WordSynonyms]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WordSynonyms](
	[WordId] [int] NOT NULL,
	[SynonymId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WordTranslations]    Script Date: 12.7.2013 г. 09:58:57 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WordTranslations](
	[WordId] [int] NOT NULL,
	[TranslationId] [int] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WordAntonyms]  WITH CHECK ADD  CONSTRAINT [FK_WordAntonyms_Words] FOREIGN KEY([WordId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordAntonyms] CHECK CONSTRAINT [FK_WordAntonyms_Words]
GO
ALTER TABLE [dbo].[WordAntonyms]  WITH CHECK ADD  CONSTRAINT [FK_WordAntonyms_Words1] FOREIGN KEY([AntonymId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordAntonyms] CHECK CONSTRAINT [FK_WordAntonyms_Words1]
GO
ALTER TABLE [dbo].[WordHypernyms]  WITH CHECK ADD  CONSTRAINT [FK_WordHypernyms_Words] FOREIGN KEY([HypernymId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordHypernyms] CHECK CONSTRAINT [FK_WordHypernyms_Words]
GO
ALTER TABLE [dbo].[WordHypernyms]  WITH CHECK ADD  CONSTRAINT [FK_WordHypernyms_Words1] FOREIGN KEY([WordId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordHypernyms] CHECK CONSTRAINT [FK_WordHypernyms_Words1]
GO
ALTER TABLE [dbo].[Words]  WITH CHECK ADD  CONSTRAINT [FK_Words_Languages] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Languages] ([LanguageId])
GO
ALTER TABLE [dbo].[Words] CHECK CONSTRAINT [FK_Words_Languages]
GO
ALTER TABLE [dbo].[Words]  WITH CHECK ADD  CONSTRAINT [FK_Words_PartsOfSpeech] FOREIGN KEY([PartOfSpeech])
REFERENCES [dbo].[PartsOfSpeech] ([PartOfSpeech])
GO
ALTER TABLE [dbo].[Words] CHECK CONSTRAINT [FK_Words_PartsOfSpeech]
GO
ALTER TABLE [dbo].[WordSynonyms]  WITH CHECK ADD  CONSTRAINT [FK_WordSynonyms_Words] FOREIGN KEY([WordId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordSynonyms] CHECK CONSTRAINT [FK_WordSynonyms_Words]
GO
ALTER TABLE [dbo].[WordSynonyms]  WITH CHECK ADD  CONSTRAINT [FK_WordSynonyms_Words1] FOREIGN KEY([SynonymId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordSynonyms] CHECK CONSTRAINT [FK_WordSynonyms_Words1]
GO
ALTER TABLE [dbo].[WordTranslations]  WITH CHECK ADD  CONSTRAINT [FK_WordTranslations_Words] FOREIGN KEY([WordId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordTranslations] CHECK CONSTRAINT [FK_WordTranslations_Words]
GO
ALTER TABLE [dbo].[WordTranslations]  WITH CHECK ADD  CONSTRAINT [FK_WordTranslations_Words1] FOREIGN KEY([TranslationId])
REFERENCES [dbo].[Words] ([WordId])
GO
ALTER TABLE [dbo].[WordTranslations] CHECK CONSTRAINT [FK_WordTranslations_Words1]
GO
USE [master]
GO
ALTER DATABASE [Dictionary] SET  READ_WRITE 
GO

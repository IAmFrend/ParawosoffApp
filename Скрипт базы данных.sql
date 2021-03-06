USE [master]
GO
/****** Object:  Database [ParawosoffDB]    Script Date: 06.06.2020 13:14:09 ******/
CREATE DATABASE [ParawosoffDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ParawosoffDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.IAMFREND\MSSQL\DATA\ParawosoffDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ParawosoffDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.IAMFREND\MSSQL\DATA\ParawosoffDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ParawosoffDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ParawosoffDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ParawosoffDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ParawosoffDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ParawosoffDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ParawosoffDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ParawosoffDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ParawosoffDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ParawosoffDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ParawosoffDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ParawosoffDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ParawosoffDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ParawosoffDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ParawosoffDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ParawosoffDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ParawosoffDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ParawosoffDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ParawosoffDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ParawosoffDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ParawosoffDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ParawosoffDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ParawosoffDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ParawosoffDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ParawosoffDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ParawosoffDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ParawosoffDB] SET  MULTI_USER 
GO
ALTER DATABASE [ParawosoffDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ParawosoffDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ParawosoffDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ParawosoffDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ParawosoffDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ParawosoffDB] SET QUERY_STORE = OFF
GO
USE [ParawosoffDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [ParawosoffDB]
GO
/****** Object:  UserDefinedFunction [dbo].[Autorization]    Script Date: 06.06.2020 13:14:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[Autorization](@Login [varchar] (255),@Password [varchar] (255))
returns [int]
with execute as caller
as
begin
declare @Return [int] = -1
if (select count(*) from [dbo].[Staff] where ([Login] = @Login) and ([Password] = @Password) and ([LogDelete] = 0))>0
set @Return = (select [ID_Staff] from [dbo].[Staff] where ([Login] = @Login) and ([Password] = @Password))
return @Return
end
GO
/****** Object:  UserDefinedFunction [dbo].[CheckGet]    Script Date: 06.06.2020 13:14:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[CheckGet]()
returns [varchar] (40)
with execute as caller
as
begin
declare @Return [varchar] (MAX) = ''
declare @Current [varchar] (MAX)= (select [Check] from [dbo].[Sell] where [Timestamp] = (select MAX([Timestamp]) from [dbo].[Sell]))
declare @CurrentLen [int] = len(@Current)
set @Return = Convert([varchar],(Convert([int],@Current)+1))
if len(@Return)>@CurrentLen
begin
set @CurrentLen = @CurrentLen+1
set @Return = 1
end
while len(@Return)<@CurrentLen
set @Return = '0'+@Return
return @Return
end
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 06.06.2020 13:14:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[ID_ProductType] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[TypeName] [varchar](40) NOT NULL,
	[Terms] [text] NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[ID_ProductType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_TypeName] UNIQUE NONCLUSTERED 
(
	[TypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID_Product] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[Name] [varchar](40) NOT NULL,
	[Storage] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[FirstDec] [varchar](10) NOT NULL,
	[Scheme_ID] [int] NOT NULL,
	[Type_ID] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ID_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Product_Main]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Product_Main]
AS
SELECT        dbo.Product.ID_Product, dbo.Product.Name, dbo.Product.Storage, dbo.Product.Price, dbo.ProductType.TypeName, dbo.Product.LogDelete
FROM            dbo.Product INNER JOIN
                         dbo.ProductType ON dbo.Product.Type_ID = dbo.ProductType.ID_ProductType
WHERE        (dbo.Product.LogDelete = 0) AND (dbo.Product.ID_Product > 0)
GO
/****** Object:  Table [dbo].[Position]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[ID_Position] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[PositionName] [varchar](40) NOT NULL,
	[Salary] [int] NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[ID_Position] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_PosName] UNIQUE NONCLUSTERED 
(
	[PositionName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID_Staff] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[Surname] [varchar](40) NOT NULL,
	[Name] [varchar](40) NOT NULL,
	[Firstname] [varchar](40) NOT NULL,
	[Recruitment] [varchar](10) NOT NULL,
	[Exp] [int] NULL,
	[Login] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Position_ID] [int] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[ID_Staff] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Login] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Staff_Main]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Staff_Main]
AS
SELECT        dbo.Staff.ID_Staff, dbo.Staff.Surname, dbo.Staff.Name, dbo.Staff.Firstname, dbo.Staff.Recruitment, dbo.Staff.Exp, dbo.Position.PositionName, dbo.Position.Salary, dbo.Staff.LogDelete
FROM            dbo.Position INNER JOIN
                         dbo.Staff ON dbo.Position.ID_Position = dbo.Staff.Position_ID
WHERE        (dbo.Staff.LogDelete = 0)
GO
/****** Object:  Table [dbo].[MarkScheme]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkScheme](
	[ID_Scheme] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[SchemeName] [varchar](40) NOT NULL,
	[Basis] [text] NULL,
	[Reality] [text] NULL,
 CONSTRAINT [PK_Scheme] PRIMARY KEY CLUSTERED 
(
	[ID_Scheme] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_SchemeName] UNIQUE NONCLUSTERED 
(
	[SchemeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID_Order] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[OrderName] [varchar](40) NOT NULL,
	[OrderText] [text] NOT NULL,
	[OrderDoc] [text] NULL,
	[Staff_ID] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID_Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sell]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sell](
	[ID_Sell] [int] NOT NULL,
	[LogDelete] [bit] NULL,
	[Timestamp] [datetime] NULL,
	[Check] [varchar](40) NULL,
	[Amount] [int] NOT NULL,
	[SellDate] [varchar](10) NOT NULL,
	[Price] [int] NULL,
	[CheckDoc] [text] NULL,
	[Staff_ID] [int] NOT NULL,
	[Product_ID] [int] NOT NULL,
 CONSTRAINT [PK_Sell] PRIMARY KEY CLUSTERED 
(
	[ID_Sell] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[MarkScheme] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Position] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[ProductType] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Sell] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT ((0)) FOR [LogDelete]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT ((0)) FOR [Exp]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_OrderStaff] FOREIGN KEY([Staff_ID])
REFERENCES [dbo].[Staff] ([ID_Staff])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_OrderStaff]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Scheme] FOREIGN KEY([Scheme_ID])
REFERENCES [dbo].[MarkScheme] ([ID_Scheme])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Scheme]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Type] FOREIGN KEY([Type_ID])
REFERENCES [dbo].[ProductType] ([ID_ProductType])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Type]
GO
ALTER TABLE [dbo].[Sell]  WITH CHECK ADD  CONSTRAINT [FK_Product] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([ID_Product])
GO
ALTER TABLE [dbo].[Sell] CHECK CONSTRAINT [FK_Product]
GO
ALTER TABLE [dbo].[Sell]  WITH CHECK ADD  CONSTRAINT [FK_Staff] FOREIGN KEY([Staff_ID])
REFERENCES [dbo].[Staff] ([ID_Staff])
GO
ALTER TABLE [dbo].[Sell] CHECK CONSTRAINT [FK_Staff]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Position] FOREIGN KEY([Position_ID])
REFERENCES [dbo].[Position] ([ID_Position])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Position]
GO
ALTER TABLE [dbo].[Position]  WITH CHECK ADD  CONSTRAINT [CH_Salary] CHECK  (([Salary]>(0)))
GO
ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [CH_Salary]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CH_FirstDec] CHECK  (([FirstDec] like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CH_FirstDec]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CH_Price] CHECK  (([Price]>(0)))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CH_Price]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CH_Storage] CHECK  (([Storage]>=(0)))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CH_Storage]
GO
ALTER TABLE [dbo].[Sell]  WITH CHECK ADD  CONSTRAINT [CH_Amount] CHECK  (([Amount]>(0)))
GO
ALTER TABLE [dbo].[Sell] CHECK CONSTRAINT [CH_Amount]
GO
ALTER TABLE [dbo].[Sell]  WITH CHECK ADD  CONSTRAINT [CH_SellDate] CHECK  (([SellDate] like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Sell] CHECK CONSTRAINT [CH_SellDate]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [CH_Exp] CHECK  (([Exp]>=(0)))
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [CH_Exp]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [CH_Password] CHECK  ((len([Password])>(5)))
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [CH_Password]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [CH_Recruitment] CHECK  (([Recruitment] like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [CH_Recruitment]
GO
/****** Object:  StoredProcedure [dbo].[Order_Doc_Create]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Order_Doc_Create] (@ID_Order [int], @OrderDoc [Text])
as
update [dbo].[Order] set [OrderDoc] = @OrderDoc where [ID_Order] = @ID_Order
GO
/****** Object:  StoredProcedure [dbo].[Order_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Order_Insert] (@OrderName [varchar] (40), @OrderText [text], @Staff_ID [int])
as
if ((select count (*) from [dbo].[Order] where [OrderName] = @OrderName)<=0)
begin
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[Order] where [ID_Order] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[Order]([ID_Order],[LogDelete],[Timestamp],[OrderName],[OrderText],[OrderDoc],[Staff_ID])
values 
(@ID_Count,0,GETDATE(),@OrderName,@OrderText,'',@Staff_ID)
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Order_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Order_LogDelete] (@ID_Order [int])
as
if (@ID_Order >0)
update [dbo].[Order] set [LogDelete] = 1 where [ID_Order] = @ID_Order
GO
/****** Object:  StoredProcedure [dbo].[Order_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Order_Update] (@ID_Order [int],@OrderName [varchar] (40), @OrderText [text], @Staff_ID [int])
as
if (@ID_Order>0)
if ((select count (*) from [dbo].[Order] where [OrderName] = @OrderName and not [ID_Order] = @ID_Order)>0)
begin
update [dbo].[Order] set
[OrderName] = @OrderName,
[OrderText] = @OrderText
where [ID_Order] = @ID_Order
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Pos_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Pos_Update] (@ID_Pos [int],@PositionName [varchar] (40), @Salary [int])
as
if ((select count (*) from [dbo].[Position] where [PositionName] = @PositionName and not [ID_Position] = @ID_Pos)>0)
print('Недопустимое имя!')
else
begin
if ((@Salary<=0)or(@ID_Pos = 0))
print('Проверьте введённые данные')
else
if ((select count (*) from [dbo].[Position] where [PositionName] = @PositionName and [ID_Position] = @ID_Pos)=0)
update [dbo].[Position] set
[PositionName] = @PositionName
Where [ID_Position] = @ID_Pos
update [dbo].[Position] set
[Timestamp] = GETDATE(),
[Salary] = @Salary
Where [ID_Position] = @ID_Pos
end
GO
/****** Object:  StoredProcedure [dbo].[Product_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Product_Insert] (@Name [varchar] (40), @Price [int], @Type_ID [int])
as
if ((@Price>0)and((select count (*) from [dbo].[Product] where [Name] = @Name)<=0))
begin
declare @Day [varchar] (2) = Convert(varchar,Day(GetDate()))
if len(@Day)<2
set @Day = '0'+@Day
declare @Month [varchar] (2) = Convert(varchar,Month(GetDate()))
if len(@Month)<2
set @Month = '0'+@Month
declare @FirstDec [varchar] (10) =@Day+'.'+@Month+'.'+Convert(varchar,Year(GetDate()))
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[Product] where [ID_Product] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[Product]([ID_Product],[LogDelete],[Timestamp],[Name],[Storage],[Price],[FirstDec],[Scheme_ID],[Type_ID])
values
(@ID_Count,0,GETDATE(),@Name,0,@Price,@FirstDec,0,@Type_ID)
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Product_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Product_LogDelete](@ID_Product [int], @AllowCascade [bit])
as
if (@ID_Product >0)
begin
update [dbo].[Product] set [LogDelete] = 1 where [ID_Product] = @ID_Product
if (@AllowCascade = 1)
update [dbo].[Sell] set [LogDelete] = 1 where [Product_ID] = @ID_Product
end
GO
/****** Object:  StoredProcedure [dbo].[Product_Reamount]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Product_Reamount](@ID_Product [int], @Value [int], @Dir [bit])
as
if (@ID_Product >0)
if (@Dir = 0 and @Value>(select [Storage] from [dbo].[Product] where [ID_Product] = @ID_Product))
print('Проверьте введённое значение')
else
if (@Dir = 0)
update [dbo].[Product] set [Storage] = [Storage]-@Value where [ID_Product] = @ID_Product
else
update [dbo].[Product] set [Storage] = [Storage]+@Value where [ID_Product] = @ID_Product
GO
/****** Object:  StoredProcedure [dbo].[Product_SchemeSet]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Product_SchemeSet](@ID_Product [int], @ID_Scheme [int])
as
if (@ID_Product >0)
update [dbo].[Product] set [Scheme_ID] = @ID_Scheme where [ID_Product] = @ID_Product
GO
/****** Object:  StoredProcedure [dbo].[Product_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Product_Update] (@ID_Product [int],@Name [varchar] (40), @Price [int], @FirstDec [varchar] (10), @Scheme_ID [int], @Type_ID [int])
as
if (@ID_Product>0)
if ((@Price>0)and((select count (*) from [dbo].[Product] where [Name] = @Name and not [ID_Product] = @ID_Product)<=0)and(@FirstDec like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
begin
if ((select count (*) from [dbo].[Product] where [Name] = @Name and [ID_Product] = @ID_Product)=0)
update [dbo].[Product] set
[Name] = @Name
where
[ID_Product] = @ID_Product
update [dbo].[Product] set
[Timestamp] = GETDATE(),
[Price] = @Price,
[FirstDec] = @FirstDec,
[Scheme_ID] = @Scheme_ID,
[Type_ID] = @Type_ID
where
[ID_Product] = @ID_Product
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Scheme_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Scheme_Insert] (@Scheme_Name [varchar] (40), @Basis [text], @Reality [text])
as
if ((select count (*) from [dbo].[MarkScheme] where [SchemeName] = @Scheme_Name)>0)
print('Это название исользовать нельзя')
else
begin
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[MarkScheme] where [ID_Scheme] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[MarkScheme]([ID_Scheme],[LogDelete],[Timestamp],[SchemeName],[Basis],[Reality])
values
(@ID_Count,0,GETDATE(),@Scheme_Name,@Basis,@Reality)
end
GO
/****** Object:  StoredProcedure [dbo].[Scheme_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Scheme_LogDelete](@ID_Scheme [int], @AllowCascade [bit])
as
if (@ID_Scheme >0)
begin
update [dbo].[MarkScheme] set [LogDelete] = 1 where [ID_Scheme] = @ID_Scheme
if (@AllowCascade = 1)
begin
declare @Count [int] = (select count (*) from [dbo].[Product])
declare @ID [int] = 0
while @Count >0
begin
set @ID = (select [ID_Product] from (select row_number() over (order by [ID_Product]) as n, [ID_Product] from [dbo].[Product]) X where n = @Count)
if ((select [Scheme_ID] from [dbo].[Product] where [ID_Product] = @ID)=@ID_Scheme)
exec [dbo].[Product_LogDelete] @ID,1
set @Count = @Count -1
end
end
end
GO
/****** Object:  StoredProcedure [dbo].[Scheme_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Scheme_Update] (@ID_Scheme [int], @Scheme_Name [varchar] (40), @Basis [text], @Reality [text])
as
if (@ID_Scheme >0)
if ((select count (*) from [dbo].[MarkScheme] where [SchemeName] = @Scheme_Name and not [ID_Scheme] = @ID_Scheme)>0)
print('Это название исользовать нельзя')
else
begin
if ((select count (*) from [dbo].[MarkScheme] where [SchemeName] = @Scheme_Name and [ID_Scheme] = @ID_Scheme)=0)
update [dbo].[MarkScheme] set
[SchemeName] = @Scheme_Name
where [ID_Scheme] = @ID_Scheme
update [dbo].[MarkScheme] set
[Basis] = @Basis,
[Reality] = @Reality
where [ID_Scheme] = @ID_Scheme
end
GO
/****** Object:  StoredProcedure [dbo].[Sell_Doc_Create]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Sell_Doc_Create] (@Check [varchar] (40), @CheckDoc [Text])
as
update [dbo].[Sell] set [CheckDoc] = @CheckDoc where [Check] = @Check
GO
/****** Object:  StoredProcedure [dbo].[Sell_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sell_Insert] (@Check [varchar] (40), @Amount [int], @Staff_ID [int], @Product_ID [int])
as
if ((@Amount>0)and(@Amount<=(select [Storage] from [dbo].[Product] where [ID_Product] = @Product_ID)))
begin
declare @Price [int] = @Amount*(select [Price] from [dbo].[Product] where [ID_Product] = @Product_ID)
declare @Day [varchar] (2) = Convert(varchar,Day(GetDate()))
if len(@Day)<2
set @Day = '0'+@Day
declare @Month [varchar] (2) = Convert(varchar,Month(GetDate()))
if len(@Month)<2
set @Month = '0'+@Month
declare @Selldate [varchar] (10) = @Day+'.'+@Month+'.'+Convert(varchar,Year(GetDate()))
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[Sell] where [ID_Sell] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[Sell]([ID_Sell],[LogDelete],[Timestamp],[Check],[Amount],[SellDate],[Price],[CheckDoc],[Staff_ID],[Product_ID])
values 
(@ID_Count,0,GETDATE(),@Check,@Amount,@Selldate,@Price,'',@Staff_ID,@Product_ID)
update [dbo].[Product] set [Storage] = [Storage] - @Amount where [ID_Product] = @Product_ID
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Sell_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Sell_LogDelete] (@ID_Sell [int])
as
if (@ID_Sell >0)
update [dbo].[Sell] set [LogDelete] = 1 where [ID_Sell] = @ID_Sell
GO
/****** Object:  StoredProcedure [dbo].[Sell_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Sell_Update] (@ID_Sell [int],@Check [varchar] (40), @Amount [int], @SellDate [varchar] (10), @Staff_ID [int], @Product_ID [int])
as
if (@ID_Sell>0)
if ((@Amount>0)and(@Amount<(select [Storage] from [dbo].[Product] where [ID_Product] = @Product_ID))and(@SellDate like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
begin
declare @Price [int] = @Amount*(select [Price] from [dbo].[Product] where [ID_Product] = @Product_ID)
update [dbo].[Sell] set
[Timestamp] = GETDATE(),
[Check] = @Check,
[Amount] = @Amount,
[SellDate] = @SellDate,
[Price] = @Price,
[Staff_ID] = @Staff_ID,
[Product_ID] = @Product_ID
where [ID_Sell] = @ID_Sell
end
else
print('Проверьте введённые значения!')
GO
/****** Object:  StoredProcedure [dbo].[Staff_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Staff_Insert] (@Surname [varchar] (40), @Name [varchar] (40), @Firstname [varchar] (40), @Exp [int], @Login [varchar] (255), @Password [varchar] (255), @Position_ID [int])
as
if ((select count (*) from [dbo].[Staff] where [Login] = @Login)>0)
print('Этот логин использовать нельзя')
else
if ((len(@Password)<5)or(@Exp<0))
print('проверьте введённые данные')
else
begin
declare @Day [varchar] (2) = Convert(varchar,Day(GetDate()))
if len(@Day)<2
set @Day = '0'+@Day
declare @Month [varchar] (2) = Convert(varchar,Month(GetDate()))
if len(@Month)<2
set @Month = '0'+@Month
declare @Recruitment [varchar] (10) = @Day+'.'+@Month+'.'+Convert(varchar,Year(GetDate()))
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[Staff] where [ID_Staff] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[Staff]([ID_Staff],[LogDelete],[Timestamp],[Surname],[Name],[Firstname],[Recruitment],[Exp],[Login],[Password],[Position_ID])
values
(@ID_Count,0,GETDATE(),@Surname,@Name,@Firstname,@Recruitment,@Exp,@Login,@Password,@Position_ID)
end
GO
/****** Object:  StoredProcedure [dbo].[Staff_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Staff_LogDelete](@ID_Staff [int], @AllowCascade [bit])
as
if (@ID_Staff >0)
begin
update [dbo].[Staff] set [LogDelete] = 1 where [ID_Staff] = @ID_Staff
if (@AllowCascade = 1)
begin
declare @OrCount [int] = (select count (*) from [dbo].[Order])
declare @OrID [int] = 0
while @OrCount >0
begin
set @OrID = (select [ID_Order] from (select row_number() over (order by [ID_Order]) as n, [ID_Order] from [dbo].[Order]) X where n = @OrCount)
if ((select [Staff_ID] from [dbo].[Order] where [ID_Order] = @OrID)=@ID_Staff)
exec [dbo].[Order_LogDelete] @OrID
set @OrCount = @OrCount -1
end
declare @SCount [int] = (select count (*) from [dbo].[Sell])
declare @SID [int] = 0
while @SCount >0
begin
set @SID = (select [ID_Sell] from (select row_number() over (order by [ID_Sell]) as n, [ID_Sell] from [dbo].[Sell]) X where n = @SCount)
if ((select [Scheme_ID] from [dbo].[Product] where [ID_Product] = @SID)=@ID_Staff)
exec [dbo].[Sell_LogDelete] @SID
set @SCount = @SCount -1
end
end
end

GO
/****** Object:  StoredProcedure [dbo].[Staff_Reposition]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Staff_Reposition] (@ID_Staff [int], @Position_ID [int])
as
if (@ID_Staff >0)
update [dbo].[Staff] set
[Timestamp] = GETDATE(),
[Position_ID] = @Position_ID
where [ID_Staff] = @ID_Staff
GO
/****** Object:  StoredProcedure [dbo].[Staff_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Staff_Update] (@ID_Staff [int], @Surname [varchar] (40), @Name [varchar] (40), @Firstname [varchar] (40), @Recruitment [varchar] (10), @Exp [int], @Login [varchar] (255), @Password [varchar] (255), @Position_ID [int])
as
if ((select count (*) from [dbo].[Staff] where [Login] = @Login and not [ID_Staff] = @ID_Staff)>0)
print('Этот логин использовать нельзя')
else
if ((len(@Password)<5)or(@Exp<0)or(@Recruitment not like '[0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9]'))
print('проверьте введённые данные')
else
begin
if ((select count (*) from [dbo].[Staff] where [Login] = @Login and [ID_Staff] = @ID_Staff)=0)
update [dbo].[Staff] set
[Login] = @Login
where [ID_Staff] = @ID_Staff
update [dbo].[Staff] set
[Timestamp] = GETDATE(),
[Surname] = @Surname,
[Name] = @Name,
[Firstname] = @Firstname,
[Recruitment] = @Recruitment,
[Exp] = @Exp,
[Password] = @Password,
[Position_ID] = @Position_ID
where [ID_Staff] = @ID_Staff
end
GO
/****** Object:  StoredProcedure [dbo].[Type_Insert]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Type_Insert] (@Type_Name [varchar] (40), @Terms [text])
as
if ((select count (*) from [dbo].[ProductType] where [TypeName] = @Type_Name)>0)
print('Это название исользовать нельзя')
else
begin
declare @ID_Count [int] = 0
while ((select count (*) from [dbo].[ProductType] where [ID_ProductType] = @ID_Count)>0)
set @ID_Count = @ID_Count+1
insert into [dbo].[ProductType]([ID_ProductType],[LogDelete],[Timestamp],[TypeName],[Terms])
values
(@ID_Count,0,GETDATE(),@Type_Name,@Terms)
end
GO
/****** Object:  StoredProcedure [dbo].[Type_LogDelete]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Type_LogDelete](@ID_Type [int], @AllowCascade [bit])
as
if (@ID_Type >0)
begin
update [dbo].[ProductType] set [LogDelete] = 1 where [ID_ProductType] = @ID_Type
if (@AllowCascade = 1)
begin
declare @Count [int] = (select count (*) from [dbo].[Product])
declare @ID [int] = 0
while @Count >0
begin
set @ID = (select [ID_Product] from (select row_number() over (order by [ID_Product]) as n, [ID_Product] from [dbo].[Product]) X where n = @Count)
if ((select [Scheme_ID] from [dbo].[Product] where [ID_Product] = @ID)=@ID_Type)
exec [dbo].[Product_LogDelete] @ID,1
set @Count = @Count -1
end
end
end
GO
/****** Object:  StoredProcedure [dbo].[Type_Update]    Script Date: 06.06.2020 13:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Type_Update] (@ID_Type [int], @Type_Name [varchar] (40), @Terms [text])
as
if (@ID_Type >0)
if ((select count (*) from [dbo].[ProductType] where [TypeName] = @Type_Name and not [ID_ProductType] = @ID_Type)>0)
print('Это название исользовать нельзя')
else
begin
if ((select count (*) from [dbo].[ProductType] where [TypeName] = @Type_Name and [ID_ProductType] = @ID_Type)=0)
update [dbo].[ProductType] set
[TypeName] = @Type_Name
where [ID_ProductType] = @ID_Type
update [dbo].[ProductType] set
[Terms] = @Terms
where [ID_ProductType] = @ID_Type
end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Product"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ProductType"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 136
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Product_Main'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Product_Main'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Position"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Staff"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 136
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Staff_Main'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Staff_Main'
GO
USE [master]
GO
ALTER DATABASE [ParawosoffDB] SET  READ_WRITE 
GO

exec [dbo].[Scheme_Insert] 'Отсутствует','-','-'

exec [dbo].[Type_Insert] 'Не определён','-'

exec [dbo].[Product_Insert] 'Не указано', 1, 0
update [dbo].[Product] set [Storage] = 9999 where [ID_Product] = 0

insert into [dbo].[Position](ID_Position,LogDelete,[Timestamp],[PositionName],[Salary]) values (0,0,GETDATE(),'Admin',1)
insert into [dbo].[Position](ID_Position,LogDelete,[Timestamp],[PositionName],[Salary]) values (1,0,GETDATE(),'Продавец-кассир',1)
insert into [dbo].[Position](ID_Position,LogDelete,[Timestamp],[PositionName],[Salary]) values (2,0,GETDATE(),'Менеджер по работе с персоналом',1)
insert into [dbo].[Position](ID_Position,LogDelete,[Timestamp],[PositionName],[Salary]) values (3,0,GETDATE(),'Кладовщик',1)
insert into [dbo].[Position](ID_Position,LogDelete,[Timestamp],[PositionName],[Salary]) values (4,0,GETDATE(),'Менеджер по маркетингу',1)

exec [dbo].[Staff_Insert] 'Admin','Admin','Admin',0,'5bTkLHdN9fJAHBDeNaCKsw==','5bTkLHdN9fJAHBDeNaCKsw==',0
--Логин и пароль входа администратора: rootadmin

exec [dbo].[Order_Insert] 'Отсутствует','-',0

exec [dbo].[Sell_Insert] '00001',1,0,0
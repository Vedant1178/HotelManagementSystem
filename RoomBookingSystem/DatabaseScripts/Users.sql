USE [roombookingdb]
GO

/****** Object:  Table [dbo].[Admin]    Script Date: 4/5/2021 6:54:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUsers](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[MobileNumber] [nvarchar](10) NOT NULL,
	[AddressLine] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NULL,
	[PinCode] [nvarchar](7) NULL,
	[Country] [nvarchar](50) NULL,
	[Isactive] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[USERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO



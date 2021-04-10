USE [roombookingdb]
GO

/****** Object:  Table [dbo].[Admin]    Script Date: 4/5/2021 6:54:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblRoomReservations](
	[RoomReservationId] [int] IDENTITY(1,1) NOT NULL,
	[HotelId] [nvarchar](20) NOT NULL,
	[RoomId] [nvarchar](20) NOT NULL,
	[Rate][int] NOT NULL,
	[CheckIn] [date] NOT NULL,
	[CheckOut] [date] NOT NULL,
	[Guests] [int] NOT NULL,
	[UserId] int NOT NULL,
	[TotalBillAmount] int NOT NULL,
	[RoomReservationDate] [date] NOT NULL,
 CONSTRAINT [PK_RoomReservations] PRIMARY KEY CLUSTERED 
(
	[RoomReservationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO



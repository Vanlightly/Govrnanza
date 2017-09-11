CREATE TABLE [dbo].[Apis](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[BusinessOwner] [nvarchar](200) NOT NULL,
	[TechnicalOwner] [nvarchar](200) NOT NULL,
	[BusinessSubDomainId] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_Apis] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UK_Apis_Name] UNIQUE NONCLUSTERED ([Name] ASC),
	CONSTRAINT [FK_Apis_BusinessSubDomains] FOREIGN KEY([BusinessSubDomainId]) REFERENCES [dbo].[BusinessSubDomains] ([Id])
)

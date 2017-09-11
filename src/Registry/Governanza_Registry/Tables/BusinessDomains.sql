CREATE TABLE [dbo].[BusinessDomains](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_BusinessDomains] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UK_BusinessDomains_Name] UNIQUE NONCLUSTERED ([Name] ASC)
)
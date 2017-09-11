CREATE TABLE [dbo].[BusinessSubDomains](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_BusinessSubDomain] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UK_BusinessSubDomains_Name] UNIQUE NONCLUSTERED ([Name] ASC),
	CONSTRAINT [FK_BusinessSubDomain_BusinessDomain] FOREIGN KEY([ParentId]) REFERENCES [dbo].[BusinessDomains] ([Id])
)

CREATE TABLE [dbo].[ApiTags](
	[Id] [uniqueidentifier] NOT NULL,
	[TagId] [uniqueidentifier] NOT NULL,
	[ApiId] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_ApiTags] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ApisTags_Apis] FOREIGN KEY([ApiId]) REFERENCES [dbo].[Apis] ([Id]),
	CONSTRAINT [FK_ApisTags_Tags] FOREIGN KEY([TagId]) REFERENCES [dbo].[Tags] ([Id])
)
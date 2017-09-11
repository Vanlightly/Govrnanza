CREATE TABLE [dbo].[ApiVersions]
(
	[ApiId] UNIQUEIDENTIFIER NOT NULL,
	[MajorVersion] INT NOT NULL,
	[MinorVersion] INT NOT NULL,
	[Status] INT NOT NULL,
	CONSTRAINT [PK_ApiVersions] PRIMARY KEY CLUSTERED ([ApiId] ASC, [MajorVersion] ASC, [MinorVersion] ASC),
	CONSTRAINT [FK_ApiVersions_Apis] FOREIGN KEY ([ApiId]) REFERENCES [dbo].[Apis] ([Id])
)

CREATE TABLE [dbo].[Chats] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [ChatName] NVARCHAR (MAX) NOT NULL,
    [Type]     INT            NOT NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([Id] ASC)
);


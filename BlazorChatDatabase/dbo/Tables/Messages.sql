CREATE TABLE [dbo].[Messages] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [MessageText]           NVARCHAR (MAX) NOT NULL,
    [SentTime]              DATETIME2 (7)  NOT NULL,
    [ChatId]                INT            NOT NULL,
    [UserId]                NVARCHAR (450) NOT NULL,
    [SenderName]            NVARCHAR (MAX) NOT NULL,
    [DeletedOnlyFromMyChat] BIT            NOT NULL,
    [IsItReply] BIT NULL , 
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Chats_ChatId] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ChatId]
    ON [dbo].[Messages]([ChatId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_UserId]
    ON [dbo].[Messages]([UserId] ASC);


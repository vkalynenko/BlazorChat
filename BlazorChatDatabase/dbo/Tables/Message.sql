CREATE TABLE [dbo].[Message] (
    [Id]         INT            NOT NULL,
    [Text]       NVARCHAR (50)  NULL,
    [SentTime]   DATETIME       NULL,
    [SenderName] NVARCHAR (50)  NULL,
    [SenderId]   NVARCHAR (MAX) NULL,
    [ChatId]     INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


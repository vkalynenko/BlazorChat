CREATE TABLE [dbo].[Message] (
    [Id]         INT            NOT NULL,
    [MessageText]       NVARCHAR (50)  NULL,
    [SentTime]   DATETIME       NULL,
    [SenderName] NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    ChatID int FOREIGN KEY REFERENCES Chat(Id),
    UserId nvarchar(450) foreign key references AspNetUsers(Id), 
    [DeletedOnlyFromMyChat] BIT NULL
   
);

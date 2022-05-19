CREATE TABLE [dbo].[ChatUser]
(
	[Id] INT NOT NULL PRIMARY KEY,  
    ChatID int FOREIGN KEY REFERENCES Chat(Id),
    UserId nvarchar(450) foreign key references AspNetUsers(Id), 
)

CREATE TABLE [dbo].[MatchScores]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Home] NVARCHAR(100) NOT NULL, 
    [Away] NCHAR(100) NOT NULL, 
    [ScoreHome] INT NULL, 
    [ScoreAway] INT NULL
)

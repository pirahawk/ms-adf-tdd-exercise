CREATE TABLE [dbo].[MatchScores]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Home] NVARCHAR(100) NOT NULL, 
    [Away] NVARCHAR(100) NOT NULL, 
    [ScoreHome] INT NOT NULL, 
    [ScoreAway] INT NOT NULL, 
    [Result] NVARCHAR(1) NOT NULL
)

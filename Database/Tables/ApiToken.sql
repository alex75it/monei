CREATE TABLE [dbo].[ApiToken]
(
    [Id] UNIQUEIDENTIFIER NOT NULL, 
    [AccountId] INT NOT NULL, 
    [CreateDate] DATETIME2 NOT NULL, 
    [ExpiryDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_ApiToken] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ),
    constraint UK_AccountId UNIQUE (
        AccountId
    )
)

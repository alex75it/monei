/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/


-- Load default currencies
insert into Currency (Code, Symbol, Name)
values ('EUR', N'€', 'Eur')

insert into Currency (Code, Symbol, Name)
values ('USD', N'$', 'US Dollar')

insert into Currency (Code, Symbol, Name)
values ('GBP', N'£', 'United Kingdom Pound')

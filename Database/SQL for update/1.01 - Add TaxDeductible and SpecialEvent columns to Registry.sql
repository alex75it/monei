-- Add "Tax deductible" and "Special event" columns to Registry table

begin transaction

select top 20 * from Registry

alter table Registry add IsTaxDeductible bit not NULL DEFAULT 0
alter table Registry add IsSpecialEvent bit not NULL DEFAULT 0

select top 20 * from Registry

commit transaction

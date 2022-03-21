create table WalletTable
(
Id int not null primary key,
Account nvarchar(32) not null,
Amount decimal(33,3) not null,
Direction varchar(16) not null

)
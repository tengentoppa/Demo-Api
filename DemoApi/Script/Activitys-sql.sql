create schema 'Activities';
use `Activities`;

create table `Activities` 
(
    `Id` int,
    `Title` VARCHAR(50),
    `Location` VARCHAR(50),
    `StartTime` DATETIME ,
    `EndTime` DATETIME,
    `Description` VARCHAR(500)
);

create table `Orders`
(
    `Id` int,
    `ActivityId` int,
    `UserId` int,
    `OrderTime` DATETIME,
    `IsComplete` int
)

create table `Permissions`
(
	`PermissionId` int not null primary key auto_increment,
    `AccessibleContent` varchar(100)
);

create table `UserInfos`
(
	`UserInfoId` int not null primary key auto_increment,
    `Name` varchar(30),
    `Phone` varchar(20),
    `Address` nvarchar(100),
    `Memo` nvarchar(200)
);

create table `Users`
(
	`UserId` int not null primary key auto_increment,
    `Autho` varchar(100),
    `PermissionId` int,
    `UserInfoId` int,
	foreign key(PermissionId) references Permission(PermissionId),
    foreign key(UserInfoId) references UserInfo(UserInfoId)
)
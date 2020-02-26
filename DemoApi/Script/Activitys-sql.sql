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
create database if not exists Volleyball;
use Volleyball;

create table users(
	UserID int primary key auto_increment,
    account varchar(50) ,
    hash varchar(64) 
) engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;	

create table Team(
	UserID int ,
    TeamID int primary key auto_increment,
    Name varchar(64)
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table Player(
    PlayerID int auto_increment primary key,
	UserID int,
    TeamID int,
    PName varchar(20),
    PNum int, 
    Position INT
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table GameInfo(
    GameID int auto_increment primary key,
	UserID int, 
    Serve int,
    TeamL int,
    TeamR int,
    GameDate Date,
    GameName varchar(30)
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table SetWinner(
	UserID int,
    GameID int,
    TotalSet int,
    Set1 int,
    Set2 int,
    Set3 int,
    Set4 int,
    Set5 int,
    primary key(UserID, GameID)
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table FirstPlayer(
	UserID int,
    GameID int, 
    PL1 int,
    PL2 int,
    PL3 int,
    PL4 int,
    PL5 int,
    PL6 int,
    PR1 int, 
    PR2 int,
    PR3 int,
    PR4 int, 
    PR5 int,
    PR6 int,
    primary key(UserID, GameID)
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table SetSide(
	UserID int,
    GameID int, 
    TeamL1 int, 
    TeamL2 int,
    TeamL3 int,
    TeamL4 int,
    TeamL5 int, 
    primary key(UserID, GameID)
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

create table GameData(
	BallID int primary key auto_increment,
	UserID int, 
    GameID int, 
    `Set` int,
    TeamID int,
    side int,
    Player1 int, 
    Player2 int,
    Player3 int,
    formation varchar(50),
    startx int,
    starty int,
    endx int,
    endy int,
    behavior int, 
    score int
)engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;

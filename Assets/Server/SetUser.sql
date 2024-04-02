use Volleyball;
insert into users(`account`, `hash`) value('Test1234', 'aaaa');

insert into Team(`UserID`, `Name`) value(1, 'TeamA');
insert into Team(`UserID`, `Name`) value(1, 'TeamB');

insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'a', 1, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'b', 2, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'c', 3, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'd', 4, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'e', 5, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'f', 6, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'g', 7, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'h', 8, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'i', 9, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'j', 10, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'k', 11, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 1, 'l', 12, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'A', 13, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'B', 14, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'C', 15, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'D', 16, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'E', 17, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'F', 18, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'G', 19, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'H', 20, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'I', 21, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'J', 22, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'K', 23, 0);
insert into Player(UserID, TeamID, PName, PNum, Position) value(1, 2, 'L', 24, 0);

insert into GameInfo(UserID, GameID, Serve, TeamL, TeamR) value(1, 1, 1, 1, 2);

insert into FirstPlayer(UserID, GameID, PL1, PL2, PL3, PL4, PL5, PL6, PR1, PR2, PR3, PR4, PR5, PR6) value(1, 1, 1, 2, 3, 4, 5, 6, 13, 14, 15, 16, 17, 18);

insert into SetSide(UserID, GameID, TeamL1, TeamL2, TeamL3, TeamL4, TeamL5) value(1, 1, 1, 2, 1, 2, 1);



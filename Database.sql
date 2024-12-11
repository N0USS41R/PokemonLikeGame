
-- Create the database
CREATE DATABASE ExerciceMonster;
GO

-- Use the database
USE ExerciceMonster;
GO

-- Table: Login
CREATE TABLE Login (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);
GO

-- Table: Player
CREATE TABLE Player (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    LoginID INT,
    FOREIGN KEY (LoginID) REFERENCES Login(ID)
);
GO

-- Table: Monster
CREATE TABLE Monster (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Health INT NOT NULL,
    ImageURL NVARCHAR(255) NULL
);
GO

-- Table: Spell
CREATE TABLE Spell (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Damage INT NOT NULL,
    Description NVARCHAR(MAX)
);
GO

-- Table: PlayerMonster (Relation Player <-> Monster)
CREATE TABLE PlayerMonster (
    PlayerID INT NOT NULL,
    MonsterID INT NOT NULL,
    PRIMARY KEY (PlayerID, MonsterID),
    FOREIGN KEY (PlayerID) REFERENCES Player(ID),
    FOREIGN KEY (MonsterID) REFERENCES Monster(ID)
);
GO

-- Table: MonsterSpell (Relation Monster <-> Spell)
CREATE TABLE MonsterSpell (
    MonsterID INT NOT NULL,
    SpellID INT NOT NULL,
    PRIMARY KEY (MonsterID, SpellID),
    FOREIGN KEY (MonsterID) REFERENCES Monster(ID),
    FOREIGN KEY (SpellID) REFERENCES Spell(ID)
);
GO

-- Insert Seed Data

-- Insert into Login
INSERT INTO Login (Username, PasswordHash)
VALUES 
    ('AshUser', HASHBYTES('SHA2_256', 'password1')), 
    ('MistyUser', HASHBYTES('SHA2_256', 'password2'));
GO

-- Insert into Player
INSERT INTO Player (Name, LoginID)
VALUES 
    ('Ash', 1),
    ('Misty', 2);
GO

-- Insert into Monster
INSERT INTO Monster (Name, Health, ImageURL)
VALUES 
    ('Pikachu', 100, 'https://pngimg.com/uploads/pokemon/pokemon_PNG11.png'),
    ('Staryu', 80, 'https://images.wikidexcdn.net/mwuploads/wikidex/d/d9/latest/20230204135126/EP1229_Starmie_de_Misty.png');
GO

-- Insert into Spell
INSERT INTO Spell (Name, Damage, Description)
VALUES 
    ('Thunderbolt', 25, 'A strong electric attack.'),
    ('Quick Attack', 15, 'A swift physical strike.'),
    ('Bubble Beam', 20, 'A spray of bubbles to attack.'),
    ('Water Gun', 20, 'A stream of water to attack.');
GO

-- Insert into MonsterSpell
INSERT INTO MonsterSpell (MonsterID, SpellID)
VALUES 
    (1, 1), -- Pikachu -> Thunderbolt
    (1, 2), -- Pikachu -> Quick Attack
    (2, 3), -- Staryu -> Bubble Beam
    (2, 4); -- Staryu -> Water Gun
GO

-- Insert into PlayerMonster
INSERT INTO PlayerMonster (PlayerID, MonsterID)
VALUES 
    (1, 1), -- Ash -> Pikachu
    (2, 2); -- Misty -> Staryu
GO

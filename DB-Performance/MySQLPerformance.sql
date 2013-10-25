-- 04. Table partitioning in MySQL

CREATE DATABASE PartitioningDB;

USE PartitioningDB;

-- CREATE TABLE Logs(
--   LogId INT NOT NULL AUTO_INCREMENT,
--   MessageText TEXT NOT NULL,
--   SendDate DATE NOT NULL,
--   PRIMARY KEY (LogId, SendDate)
-- ) PARTITION BY HASH(SendDate) PARTITIONS 5;

-- SELECT * FROM Logs
-- WHERE SendDate = 2;
-- EXPLAIN PARTITIONS SELECT * FROM Logs;
-- EXPLAIN PARTITIONS SELECT * FROM Logs WHERE SendDate = 2;
-- DECLARE @counter INT;

CREATE TABLE Logs (
    LogId INT NOT NULL AUTO_INCREMENT,
    MessageText TEXT NOT NULL,
    SendDate DATE NOT NULL,
    PRIMARY KEY (LogId , SendDate)
) PARTITION BY RANGE (YEAR(MsgDate)) 
( PARTITION till2015 VALUES LESS THAN (2015) , 
  PARTITION till2020 VALUES LESS THAN (2020) , 
  PARTITION till2030 VALUES LESS THAN (2030) , 
  PARTITION till2050 VALUES LESS THAN (2050) , 
  PARTITION after2050 VALUES LESS THAN MAXVALUE);

  
INSERT INTO Logs(MessageText, SendDate) 
  VALUES ('Text 1', NOW() + DAY(1)),
  VALUES ('Text 1', NOW() + DAY(10)),
  VALUES ('Text 1', NOW() + DAY(100)),
  VALUES ('Text 1', NOW() + DAY(150)),
  VALUES ('Text 1', NOW() + DAY(400)),
  VALUES ('Text 1', NOW() + DAY(1000)),
  VALUES ('Text 1', NOW() + DAY(2000)),
  VALUES ('Text 1', NOW() + DAY(3000)),
  VALUES ('Text 1', NOW() + DAY(4000)),
  VALUES ('Text 1', NOW() + DAY(5000)),
  VALUES ('Text 1', NOW() + DAY(6000)),
  VALUES ('Text 1', NOW() + DAY(8000)),
  VALUES ('Text 1', NOW() + DAY(10000)),
  VALUES ('Text 1', NOW() + DAY(20000));
-- DECLARE @counter INT DEFAULT 0;
-- CREATE PROCEDURE FillTable()
-- BEGIN
-- DECLARE counter INT DEFAULT 0;
-- WHILE counter < 10000000 DO
--  INSERT INTO Logs(MessageText, SendDate) 
--  VALUES ('Text ' + CONVERT(TEXT, counter), NOW() + DAY(counter));
--  SET counter = counter + 1;
-- END WHILE;
-- END;

SELECT * FROM Logs PARTITION (till2015);

SELECT * FROM Logs PARTITION (till2020);

SELECT * FROM Logs PARTITION (till2030);

SELECT * FROM Logs PARTITION (till2050);

SELECT * FROM Logs PARTITION (after2050);

-- Select from all partittions
SELECT * FROM Logs;

-- Select from a single partition
SELECT * FROM Logs
WHERE YEAR(SendDate) > 2030;
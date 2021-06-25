SELECT* FROM igraci
SELECT* FROM igracStatistika
SELECT* FROM nastupi
SELECT* FROM ocjene
SELECT* FROM skillsStavke
SELECT* FROM utakmice
SELECT* FROM izv
SELECT* FROM prvaPostava
SELECT* FROM izvjestaji
SELECT* FROM izmjena
SELECT* FROM utakmicaIzmjena
SELECT* FROM utakmice
SELECT* FROM notifikacije
SELECT* FROM dogadjaji
SELECT* FROM treninzi

SELECT* FROM Trener
SELECT* FROM trenerStatistika
SELECT* FROM igraci
SELECT* FROM AspNetUsers

style="padding-left:5px;padding-right:5px;"
style="width:25px;"


DELETE FROM Trener
DELETE FROM AspNetUsers

DELETE FROM igraci
DELETE FROM AspNetUsers

DELETE FROM igraci
DELETE FROM prvaPostava
DELETE FROM klupa
DELETE FROM igracSkills
DELETE FROM skillsStavke
DELETE FROM igracStatistika

DBCC CHECKIDENT (igraci, RESEED, 0);
GO

DELETE FROM nastupi
DELETE FROM ocjene
DELETE FROM izmjena
DELETE FROM utakmicaIzmjena
DELETE FROM izvjestaji
DELETE FROM utakmice
DELETE FROM notifikacije
DELETE FROM zahtjevi

SELECT k.ime + ' ' + k.prezime, i.igracID, k.Id
FROM igraci as i
INNER JOIN AspNetUsers as k
ON i.korisnikID = k.Id

SELECT g.nazivGrada,d.nazivDrzave
FROM gradovi as g
INNER JOIN drzave as d
ON g.drzavaID = d.drzavaID

SELECT g.nazivGrada
FROM gradovi as g

SELECT g.nazivDrzave
FROM drzave as g

BACKUP DATABASE eBordoDB_10
TO DISK = 'F:\eBordoDB.bak';

ALTER DATABASE eBordoDB_10 
MODIFY FILE 
(
    NAME = eBordoDB_10_log,
    SIZE = 76MB,
    MAXSIZE = 200MB,
    FILEGROWTH = 10MB
);
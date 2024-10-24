CREATE PROCEDURE [dbo].[spAggregatiPerGiorno](@Anno int, @Mese int)AS

DECLARE @tblDati TABLE (Giorno int, costo decimal, costobasso decimal, costomedio decimal, costoalto decimal)
DECLARE @tblDati2 TABLE (Giorno int, costo decimal)

INSERT INTO @tblDati SELECT 1,0,25,32,39
INSERT INTO @tblDati SELECT 2,0,50,64,78
INSERT INTO @tblDati SELECT 3,0,75,96,117
INSERT INTO @tblDati SELECT 4,0,100,128,156
INSERT INTO @tblDati SELECT 5,0,125,160,195
INSERT INTO @tblDati SELECT 6,0,150,192,234
INSERT INTO @tblDati SELECT 7,0,175,224,273
INSERT INTO @tblDati SELECT 8,0,200,256,312
INSERT INTO @tblDati SELECT 9,0,225,288,351
INSERT INTO @tblDati SELECT 10,0,250,320,390
INSERT INTO @tblDati SELECT 11,0,275,352,429
INSERT INTO @tblDati SELECT 12,0,300,384,468
INSERT INTO @tblDati SELECT 13,0,325,416,507
INSERT INTO @tblDati SELECT 14,0,350,448,546
INSERT INTO @tblDati SELECT 15,0,375,480,585
INSERT INTO @tblDati SELECT 16,0,400,512,624
INSERT INTO @tblDati SELECT 17,0,425,544,663
INSERT INTO @tblDati SELECT 18,0,450,576,702
INSERT INTO @tblDati SELECT 19,0,475,608,741
INSERT INTO @tblDati SELECT 20,0,500,640,780
INSERT INTO @tblDati SELECT 21,0,525,672,819
INSERT INTO @tblDati SELECT 22,0,550,704,858
INSERT INTO @tblDati SELECT 23,0,575,736,897
INSERT INTO @tblDati SELECT 24,0,600,768,936
INSERT INTO @tblDati SELECT 25,0,625,800,975
INSERT INTO @tblDati SELECT 26,0,650,832,1014
INSERT INTO @tblDati SELECT 27,0,675,864,1053
INSERT INTO @tblDati SELECT 28,0,700,896,1092
INSERT INTO @tblDati SELECT 29,0,725,928,1131
INSERT INTO @tblDati SELECT 30,0,750,960,1170
INSERT INTO @tblDati SELECT 31,0,775,992,1209

UPDATE 
	@tblDati
SET 
	costobasso = costobasso * -1,
	costomedio = costomedio * -1,
	costoalto = costoalto * -1

INSERT INTO @tblDati2
SELECT 
	ATT.Giorno,
	SUM(Att.Costo) AS CostoGiorno
FROM 
	tblAttivita ATT
	INNER JOIN tblPeriodi PE ON PE.ID = ATT.RifPeriodo
WHERE 
	PE.Mese = @Mese AND PE.Anno = @Anno
GROUP BY 
	ATT.RifPeriodo, ATT.Giorno
ORDER BY 
	ATT.Giorno

UPDATE
	@tblDati
SET
	costo = dati2.costo
FROM 
	@tblDati dati1
	INNER JOIN @tblDati2 dati2 ON dati1.Giorno = dati2.Giorno

IF @Anno = YEAR(GETDATE()) AND @Mese = MONTH(GETDATE())
BEGIN
	SELECT 
		Giorno, 
		(costo) AS CostoGiorno, 
		costobasso, 
		costomedio, 
		costoalto
	FROM 
		@tblDati 
	WHERE 
		Giorno <= DAY(GETDATE())
	ORDER BY 
		Giorno	
END
ELSE
BEGIN
	SELECT 
		Giorno, 
		(costo) AS CostoGiorno, 
		costobasso, 
		costomedio, 
		costoalto
	FROM 
		@tblDati 
	ORDER BY 
		Giorno
END
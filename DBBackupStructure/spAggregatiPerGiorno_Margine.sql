CREATE PROCEDURE [dbo].[spAggregatiPerGiorno_Margine](@Anno int, @Mese int)AS

DECLARE @tblDati TABLE (Giorno int, costo decimal, costobasso decimal, costomedio decimal, costoalto decimal, marginebasso decimal, marginemedio decimal, marginealto decimal, previsionale decimal)
DECLARE @tblDati2 TABLE (Giorno int, costo decimal)

INSERT INTO @tblDati SELECT 1,0,25,32,39,0,0,0,0
INSERT INTO @tblDati SELECT 2,0,50,64,78,0,0,0,0
INSERT INTO @tblDati SELECT 3,0,75,96,117,0,0,0,0
INSERT INTO @tblDati SELECT 4,0,100,128,156,0,0,0,0
INSERT INTO @tblDati SELECT 5,0,125,160,195,0,0,0,0
INSERT INTO @tblDati SELECT 6,0,150,192,234,0,0,0,0
INSERT INTO @tblDati SELECT 7,0,175,224,273,0,0,0,0
INSERT INTO @tblDati SELECT 8,0,200,256,312,0,0,0,0
INSERT INTO @tblDati SELECT 9,0,225,288,351,0,0,0,0
INSERT INTO @tblDati SELECT 10,0,250,320,390,0,0,0,0
INSERT INTO @tblDati SELECT 11,0,275,352,429,0,0,0,0
INSERT INTO @tblDati SELECT 12,0,300,384,468,0,0,0,0
INSERT INTO @tblDati SELECT 13,0,325,416,507,0,0,0,0
INSERT INTO @tblDati SELECT 14,0,350,448,546,0,0,0,0
INSERT INTO @tblDati SELECT 15,0,375,480,585,0,0,0,0
INSERT INTO @tblDati SELECT 16,0,400,512,624,0,0,0,0
INSERT INTO @tblDati SELECT 17,0,425,544,663,0,0,0,0
INSERT INTO @tblDati SELECT 18,0,450,576,702,0,0,0,0
INSERT INTO @tblDati SELECT 19,0,475,608,741,0,0,0,0
INSERT INTO @tblDati SELECT 20,0,500,640,780,0,0,0,0
INSERT INTO @tblDati SELECT 21,0,525,672,819,0,0,0,0
INSERT INTO @tblDati SELECT 22,0,550,704,858,0,0,0,0
INSERT INTO @tblDati SELECT 23,0,575,736,897,0,0,0,0
INSERT INTO @tblDati SELECT 24,0,600,768,936,0,0,0,0
INSERT INTO @tblDati SELECT 25,0,625,800,975,0,0,0,0
INSERT INTO @tblDati SELECT 26,0,650,832,1014,0,0,0,0
INSERT INTO @tblDati SELECT 27,0,675,864,1053,0,0,0,0
INSERT INTO @tblDati SELECT 28,0,700,896,1092,0,0,0,0
INSERT INTO @tblDati SELECT 29,0,725,928,1131,0,0,0,0
INSERT INTO @tblDati SELECT 30,0,750,960,1170,0,0,0,0
INSERT INTO @tblDati SELECT 31,0,775,992,1209,0,0,0,0

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

DECLARE @Giorno INT
DECLARE @sSumFinoALgiorno Decimal
SET @sSumFinoALgiorno = 0
DECLARE c_curs CURSOR FOR
SELECT Giorno FROM @tblDati ORDER BY Giorno
OPEN c_curs
FETCH NEXT FROM c_curs
INTO @Giorno
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @sSumFinoALgiorno = 0
	SELECT @sSumFinoALgiorno = SUM(costo) FROM @tblDati2 WHERE Giorno <=@Giorno

	UPDATE @tblDati
	SET costo = @sSumFinoALgiorno 
	WHERE Giorno = @Giorno

FETCH NEXT FROM c_curs
INTO @Giorno
END

UPDATE @tblDati
SET costo = ISNULL(costo, 0) 

CLOSE c_curs
DEALLOCATE c_curs

UPDATE
	@tblDati
SET
	marginebasso = dati1.costo+dati1.costobasso,
	marginemedio = dati1.costo+dati1.costomedio,
	marginealto = dati1.costo+dati1.costoalto,
	previsionale = dati1.costo+1200
FROM 
	@tblDati dati1

IF @Anno = YEAR(GETDATE()) AND @Mese = MONTH(GETDATE())
BEGIN
	SELECT 
		Giorno, 
		(costo) AS CostoGiorno, 
		costobasso, 
		costomedio, 
		costoalto,
		marginebasso,
		marginemedio,
		marginealto
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
		costoalto,
		marginebasso,
		marginemedio,
		marginealto
	FROM 
		@tblDati 
	ORDER BY 
		Giorno
END
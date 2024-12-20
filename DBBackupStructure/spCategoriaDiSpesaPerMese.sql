CREATE PROCEDURE [dbo].[spCategoriaDiSpesaPerMese](@iRifTipoAtt int)AS

DECLARE @tblDati TABLE(ID int, Descrizione varchar(100), TipoAttivita varchar(100), Valore decimal, Media decimal, TotaleMese decimal, PercentualeSulTotale decimal, Tipologia varchar(100), ColoreHTML varchar(25))

INSERT INTO @tblDati(ID, Descrizione, TipoAttivita, Valore)
SELECT 
	Per.ID, 
	PER.Descrizione, 
	TP.TipoAttivita,
	CASE 
		WHEN SUM(ATT.Costo) < 0 THEN SUM(ATT.Costo)*-1
		ELSE SUM(ATT.Costo)
	END AS Valore
FROM 
	tblAttivita ATT
	INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita
WHERE 
	ATT.RifTipoAttivita = @iRifTipoAtt 
GROUP BY
	Per.ID, PER.Descrizione, TP.TipoAttivita
ORDER BY 
	Per.ID DESC, PER.Descrizione, TP.TipoAttivita

DECLARE @dMedia decimal
SELECT @dMedia = AVG(Valore) FROM @tblDati

DECLARE @sTipo varchar(50)
DECLARE @sColoreHTML varchar(50)

SELECT 
	@sTipo = Tipologia, 
	@sColoreHTML = ColoreHTML 
FROM 
	tblTipoAttivita 
WHERE 
	ID = @iRifTipoAtt

DECLARE @tbl2 TABLE (Descrizione varchar(100), Valore decimal)

INSERT INTO @tbl2
SELECT
	Per.Descrizione, 	
	CASE 
		WHEN SUM(ATT.Costo) < 0 THEN SUM(ATT.Costo)*-1
		ELSE SUM(ATT.Costo)
	END
FROM 
	@tblDati Dati
	inner join tblPeriodi Per on Per.Descrizione = Dati.Descrizione
	inner join tblAttivita Att ON Att.RifPeriodo = Per.ID
	inner join tblTipoAttivita TP on TP.ID = Att.RifTipoAttivita
WHERE 
	TP.Tipologia = @sTipo
GROUP BY 
	Per.Descrizione

UPDATE
	@tblDati
SET
	TotaleMese = dati2.Valore,
	PercentualeSulTotale = Round(((Dat.Valore*100)/dati2.Valore),2),
	Tipologia = @sTipo,
	ColoreHTML = @sColoreHTML,
	Media = @dMedia
FROM 
	@tblDati Dat
	INNER JOIN @tbl2 dati2 on dati2.Descrizione = Dat.Descrizione

SELECT 
	Descrizione, 
	TipoAttivita, 
	Valore,
	Media,
	TotaleMese,
	PercentualeSulTotale,
	Tipologia,
	ColoreHTML
FROM 
	@tblDati
ORDER BY 
	ID DESC
CREATE PROCEDURE [dbo].[spMedie] AS
DECLARE @iMaxrifPeriodo int

DECLARE @tblDati TABLE (Rifperiodo int, TipoAttivita varchar(100), Colore varchar(100), Valore decimal) 
DECLARE @tblDati2 TABLE (TipoAttivita varchar(100), Colore varchar(100), Valore decimal) 

INSERT INTO @tblDati
SELECT 
	ATT.RifPeriodo,
	TP.Tipologia + ' - ' + TP.TipoAttivita,
	TP.ColoreHTML,
	SUM(ATT.Costo)
FROM 
	tblAttivita ATT
	INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita
GROUP BY 
	ATT.RifPeriodo, TP.Tipologia + ' - ' + TP.TipoAttivita, TP.ColoreHTML

SELECT @iMaxrifPeriodo = Max(Rifperiodo) FROM @tblDati

INSERT INTO @tblDati2
SELECT 
	TipoAttivita,
	Colore,
	AVG(Valore)
FROM 
	@tblDati 
WHERE 
	RifPeriodo <> @iMaxrifPeriodo
GROUP BY 
	TipoAttivita, Colore

SELECT 
	TipoAttivita,
	Colore, 
	CASE 
		WHEN TipoAttivita like '%entrata%' THEN Valore
		ELSE Valore*-1
	END AS Valore 
FROM 
	@tblDati2 
ORDER BY 
		CASE 
		WHEN TipoAttivita like '%entrata%' THEN 2
		ELSE 1
	END,
	Valore desc, 
	TipoAttivita
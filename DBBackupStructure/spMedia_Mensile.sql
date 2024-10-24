CREATE PROCEDURE [dbo].[spMedia_Mensile] AS

DECLARE @tblDati TABLE (Periodo varchar(100), costo money)
insert into @tblDati
SELECT 
	Per.Descrizione,
	SUM(ATT.Costo)
FROM 
	tblAttivita ATT
	INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita
WHERE 
	Att.Costo < 0
GROUP BY 
	Per.Descrizione

SELECT AVG(costo) AS SpesaMediaMensile FROM @tblDati

DECLARE @tblDati2 TABLE (Periodo varchar(100), costo money)
insert into @tblDati2
SELECT 
	Per.Descrizione,
	SUM(ATT.Costo)
FROM 
	tblAttivita ATT
	INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita
WHERE 
	Att.Costo > 0
GROUP BY 
	Per.Descrizione

SELECT AVG(costo) AS EntrateMediaMensile FROM @tblDati2


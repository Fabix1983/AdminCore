CREATE PROCEDURE [dbo].[spTrend] (@iTop int) AS

IF @iTop = 12 
BEGIN
	SELECT TOP 12
		Per.ID, 
		PER.Descrizione,
		SUM(ATT.Costo) AS Bilancio
	FROM 
		tblAttivita ATT
		INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	GROUP BY
		Per.ID, PER.Descrizione
	ORDER BY 
		Per.ID DESC, PER.Descrizione
END
IF @iTop = 24 
BEGIN
	SELECT TOP 24
		Per.ID, 
		PER.Descrizione,
		SUM(ATT.Costo) AS Bilancio
	FROM 
		tblAttivita ATT
		INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	GROUP BY
		Per.ID, PER.Descrizione
	ORDER BY 
		Per.ID DESC, PER.Descrizione
END 
IF @iTop = 36 
BEGIN
	SELECT TOP 36
		Per.ID, 
		PER.Descrizione,
		SUM(ATT.Costo) AS Bilancio
	FROM 
		tblAttivita ATT
		INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	GROUP BY
		Per.ID, PER.Descrizione
	ORDER BY 
		Per.ID DESC, PER.Descrizione
END  
IF @iTop > 100
BEGIN
	SELECT
		Per.ID, 
		PER.Descrizione,
		SUM(ATT.Costo) AS Bilancio
	FROM 
		tblAttivita ATT
		INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo
	GROUP BY
		Per.ID, PER.Descrizione
	ORDER BY 
		Per.ID DESC, PER.Descrizione
END  

CREATE PROCEDURE [dbo].[spCercaAttivitaPerParola_Aggregato](@sParola varchar(150)) AS

SELECT         
	TP.TipoAttivita,     
	TP.ColoreHTML,         
	Cast(ROUND(CASE 
		WHEN SUM(ATT.Costo) < 0 THEN SUM(ATT.Costo)*-1
		ELSE SUM(ATT.Costo)
	END, 0) as int) AS Costo
FROM     
	[dbo].[tblAttivita] ATT     
	INNER JOIN [dbo].[tblTipoAttivita] TP ON TP.ID = ATT.RifTipoAttivita     
	INNER JOIN [dbo].[tblPeriodi] PE ON PE.ID = ATT.RifPeriodo 
WHERE     
	UPPER(ATT.Dettagli) like '%'+@sParola+'%'  
GROUP BY 
	TP.TipoAttivita,TP.ColoreHTML
ORDER BY      
	SUM(ATT.Costo) DESC


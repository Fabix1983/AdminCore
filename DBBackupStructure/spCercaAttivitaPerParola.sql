CREATE PROCEDURE [dbo].[spCercaAttivitaPerParola](@sParola varchar(150)) AS

SELECT     
	CAST(Cast(Att.Giorno as varchar) + '/' + Cast(PE.Mese as varchar) + '/' + Cast(PE.Anno as varchar) as datetime) AS DataSpesa,     
	Att.Dettagli,     
	TP.TipoAttivita,     
	TP.ColoreHTML,     
	TP.Tipologia,     
	TP.ColoreAzione,     
	PE.Descrizione,     
	ATT.Costo 
FROM     
	[dbo].[tblAttivita] ATT     
	INNER JOIN [dbo].[tblTipoAttivita] TP ON TP.ID = ATT.RifTipoAttivita     
	INNER JOIN [dbo].[tblPeriodi] PE ON PE.ID = ATT.RifPeriodo 
WHERE     
	UPPER(ATT.Dettagli) like '%'+@sParola+'%'  
ORDER BY      
	CAST(Cast(Att.Giorno as varchar) + '/' + Cast(PE.Mese as varchar) + '/' + Cast(PE.Anno as varchar) as datetime) desc


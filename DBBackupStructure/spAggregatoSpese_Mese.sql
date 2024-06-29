CREATE PROCEDURE spAggregatoSpese_Mese(@Anno int, @Mese int)AS

SELECT
   TP.TipoAttivita,
   TP.ColoreHTML, 
   SUM(ATT.Costo) AS TotAtt
FROM 
   tblAttivita ATT
   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo 
   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita 
WHERE 
   Per.Anno = @Anno AND Per.Mese = @Mese
GROUP BY 
   ATT.RifPeriodo, TP.TipoAttivita, TP.ColoreHTML 
ORDER BY 
    SUM(ATT.Costo)
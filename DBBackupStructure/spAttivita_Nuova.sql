CREATE PROCEDURE [dbo].[spAttivita_Nuova] (
	@Giorno int, 
	@RifPeriodo int, 
	@RiftipoAttivita int, 
	@Dettagli varchar(250), 
	@Costo money
)AS

DECLARE @Tipologia varchar(50)
SELECT @Tipologia = Tipologia FROM tblTipoAttivita WHERE ID = @RiftipoAttivita

IF @Tipologia = 'Costo'
BEGIN 
	IF @Costo > 0
	BEGIN
		SET @Costo = @Costo * -1
	END
END
ELSE 
BEGIN
	IF @Costo < 0
	BEGIN
		SET @Costo = @Costo * -1
	END
END

IF RTRIM(LTRIM(ISNULL(@Dettagli, ''))) = ''
BEGIN
	SET @Dettagli = '-'
END

SET XACT_ABORT ON
BEGIN TRAN tr

INSERT INTO tblAttivita(Giorno, RifPeriodo, RifTipoAttivita, Dettagli, Costo)
VALUES(@Giorno, @RifPeriodo, @RiftipoAttivita, @Dettagli, @Costo)

COMMIT TRAN tr
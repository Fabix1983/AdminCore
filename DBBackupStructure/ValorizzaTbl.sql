
SET XACT_ABORT ON
BEGIN TRAN transcation

-- Caricamento del tipo di attivita:
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Alimentare', 'CORAL', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Vestiti', 'MEDIUMPURPLE', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Trasporto', 'CORNFLOWERBLUE', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Varie', 'GREY', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Una Tantum', 'GOLDENROD', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Salute', 'CRIMSON', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Bollette', 'hotpink', 'Costo', 'RED')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Stipendio', 'DARKGREEN', 'Entrata', 'GREEN')
INSERT INTO tblTipoAttivita(TipoAttivita, ColoreHTML, Tipologia, ColoreAzione)
VALUES('Altro', 'YELLOWGREEN', 'Entrata', 'GREEN')

SelecT * from [dbo].[tblTipoAttivita]

-- Caricamento dei periodi
DECLARE @y int = 2023
DECLARE @m int = 1
DECLARE @Descrizione varchar(50) = ''

WHILE @y <= 2040
BEGIN
	SET @m = 1
	WHILE @m <= 12
	BEGIN
		SET @Descrizione = ''

		IF @m = 1
		BEGIN 
			SET @Descrizione = 'Gennaio'
		END
		ELSE IF @m = 2
		BEGIN 
			SET @Descrizione = 'Febbraio'
		END
		ELSE IF @m = 3
		BEGIN 
			SET @Descrizione = 'Marzo'
		END
		ELSE IF @m = 4
		BEGIN 
			SET @Descrizione = 'Aprile'
		END
		ELSE IF @m = 5
		BEGIN 
			SET @Descrizione = 'Maggio'
		END
		ELSE IF @m = 6
		BEGIN 
			SET @Descrizione = 'Giugno'
		END
		ELSE IF @m = 7
		BEGIN 
			SET @Descrizione = 'Luglio'
		END
		ELSE IF @m = 8
		BEGIN 
			SET @Descrizione = 'Agosto'
		END
		ELSE IF @m = 9
		BEGIN 
			SET @Descrizione = 'Settembre'
		END
		ELSE IF @m = 10
		BEGIN 
			SET @Descrizione = 'Ottobre'
		END
		ELSE IF @m = 11
		BEGIN 
			SET @Descrizione = 'Novembre'
		END
		ELSE IF @m = 12
		BEGIN 
			SET @Descrizione = 'Dicembre'
		END

		SET @Descrizione = @Descrizione + ' ' + CAST(@y as varchar(25)) 

		INSERT INTO tblPeriodi(Mese, Anno, Descrizione)
		VALUES(@m, @y, @Descrizione)

		SET @m = @m + 1
	END
    SET @y = @y + 1
END

SelecT * from [dbo].[tblPeriodi] order by id desc

ROLLBACK TRAN transcation
--COMMIT TRAN transcation
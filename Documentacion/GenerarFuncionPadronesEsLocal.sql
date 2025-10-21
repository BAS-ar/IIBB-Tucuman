/**************************
 **** 14-10-2025 11:49 ****
 ****   Versión X      ****
 **************************/

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = N'fn_padrones_eslocal') 
   DROP FUNCTION [dbo].[fn_padrones_eslocal]
GO

CREATE  FUNCTION [dbo].[fn_padrones_eslocal] (@cuit char(15), @codprv char(3)) RETURNS bit
AS
BEGIN

DECLARE
@eslocal bit

SELECT @eslocal=CASE WHEN @codprv in (SELECT codprv 
	FROM VISTACTACTESDOMICILIOS (nolock)
	WHERE cueprefi in ('C', 'P') AND nrodoc1=@cuit)  THEN 1 ELSE 0 END

FIN:
RETURN @eslocal
END


GO



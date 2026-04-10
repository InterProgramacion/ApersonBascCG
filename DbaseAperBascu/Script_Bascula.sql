SELECT 
    f.name AS foreign_key_name,
    OBJECT_NAME(f.parent_object_id) AS referencing_table
FROM sys.foreign_keys AS f
WHERE f.referenced_object_id = OBJECT_ID('buques');

Select * from auditpesajes

DROP TABLE buques;

ALTER TABLE Detalle_Buque DROP CONSTRAINT FK_detalle_buque
ALTER TABLE auditpesajes DROP CONSTRAINT FK_auditpesajes_buques
ALTER TABLE buqueviaje DROP CONSTRAINT FK_buqueviaje_buque
ALTER TABLE pesajes DROP CONSTRAINT FK_pesajes_buques

ALTER TABLE Detalle_Buque 
ALTER COLUMN viaje_buque_dbq NCHAR(10) NOT NULL;

ALTER TABLE auditpesajes 
ALTER COLUMN Idvbuq_aud NCHAR(10) NOT NULL;

ALTER TABLE pesajes 
ALTER COLUMN Idvbuq_pes NCHAR(10) NOT NULL;

INSERT INTO dbo.buques
Select * from [DbAperBascu].dbo.buques


Usuario    = sa
Contraseña = @SysCGsa2025

CREATE LOGIN BasculaCG WITH PASSWORD = '88992025*@';


USE DbAperBascu;
CREATE USER BasculaS FOR LOGIN BasculaS;

ALTER ROLE db_datareader ADD MEMBER BasculaS;
ALTER ROLE db_datawriter ADD MEMBER BasculaS;

ALTER ROLE db_owner ADD MEMBER BasculaS;



ALTER TABLE Detalle_Buque 
ADD CONSTRAINT FK_detalle_buque 
FOREIGN KEY (buque_dbq) REFERENCES buques(Id_buq);

ALTER TABLE auditpesajes 
ADD CONSTRAINT FK_auditpesajes_buques 
FOREIGN KEY (Idbuq_aud) REFERENCES buques(Id_buq)

ALTER TABLE buqueviaje 
ADD CONSTRAINT FK_buqueviaje_buque 
FOREIGN KEY (Id_buq) REFERENCES buques(Id_buq)

ALTER TABLE pesajes 
ADD CONSTRAINT FK_pesajes_buques 
FOREIGN KEY (Idbuq_pes) REFERENCES buques(Id_buq)


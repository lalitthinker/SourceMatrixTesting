CREATE FUNCTION [dbo].[GetValueListTable]
(              
                -- Add the parameters for the function here
                @ValueList varchar(max), 
                @Deliminator char(1)
)
RETURNS @Values TABLE(ListItem varchar(255)) 
AS
BEGIN

DECLARE @StartAt int
DECLARE @DelimAt int
DECLARE @Value varchar(255)

SET @StartAt = 0
SET @DelimAt = CHARINDEX(@Deliminator,@ValueList,@StartAt)

WHILE @DelimAt < LEN(@ValueList)
Begin
                
                SET @DelimAt = CHARINDEX(@Deliminator,@ValueList,@StartAt)
                IF @DelimAt = 0
                                SET @DelimAt = LEN(@ValueList) + 1

                SET @Value = SUBSTRING(@ValueList,@StartAt,@DelimAt - @StartAt)
                
                INSERT INTO @Values VALUES(@Value)

                SET @StartAt = @DelimAt + 1

End

RETURN 

END
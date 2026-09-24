namespace Darbu_records.OldItems
{
    public class Quest_SqlQuery
    {
        public string dbConn = "Data Source=LAPTOP-TP\\SQLEXPRESS01;Initial Catalog=Praktika;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public string Gedimai = "SELECT List,Description,Record_Id,Solution,Nuotrauka FROM Gedimas";
        public string Instrukcijos_radimas = "SELECT Pavadinimas,Aprasymas,Instrukcijos_id FROM Instrukcijos";
        public string Record_check_quest = "SELECT List,Description,Record_Id,Solution,Nuotrauka FROM Gedimas";

        public string Instrukcijos_all = @"
    SELECT 
        I.title,
        I.description,
        I.instruction_id,
        I.file_name,
        K.Patinka,
        K.Nepatinka
    FROM 
        Instrukcijos AS I
    LEFT JOIN 
        Instrukcijos_statusas AS K ON I.instruction_id = K.Instrukcijos_id
   
";
    }
}

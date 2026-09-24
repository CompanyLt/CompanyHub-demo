namespace Darbu_records.Query
{


    public class SqlQuery
    {


        public string patinka_pridejimas = "Update Instrukcijos_statusas SET Patinka=@skaicius WHERE Instrukcijos_id=@id";
        public string dbConn = @"Data Source=localhost\SQLEXPRESS;
                                    Initial Catalog=CompanyHub;
                                    Integrated Security=True;
                                    Connect Timeout=30;Encrypt=False;";
        public string add_query = "INSERT INTO AppUser() VALUES()";
        public string UserCheck = "Select count(*) FROM AppUser WHERE Email=@email";



        public string Login_Checkold = "Select AppUser_Id, Login FROM AppUser Where Login = @name AND Password = @password";
        public string Login_Check= @"Select
            AU.AppUser_Id,
            AU.Login,
            W.DepartmentId,
            R.role_name,
            W.id
            FROM AppUser AS AU
            LEFT JOIN Worker AS W ON W.appUserId = AU.AppUser_Id
            INNER JOIN Role AS R ON W.role_id=R.id
            WHERE AU.Login = @name AND AU.Password = @password
        ";



        public string Add_App = "INSERT INTO AppUser(Login,Password,Email) VALUES(@name, @password, @email)";
        //gedimo radimas
        public string incidentSearchQuery = "SELECT List,Description,Record_Id,Solution FROM Gedimas Where appUser_Id=@id AND status=@status";

        public string Record_check_quest = "SELECT List,Description,Record_Id,Solution,Nuotrauka FROM Gedimas";
        //instrkcijos radimas
        public string instructionSearchQuery = "SELECT title,description,instruction_id FROM Instruction Where user_id=@id AND status=@status";
        //isvedame sukurtos eilutes id OUTPUT INSERTED
       //Perkelta// public string addIncident = "INSERT INTO Gedimas(List,appUser_Id,Description,Solution,file_name,status) OUTPUT INSERTED.Record_Id VALUES(@name,@client_id,@description,@solution,@file_name,@status)";
        public string addInstruction = "INSERT INTO Instrukcijos(title,description,user_id,file_name,upload_date,status) OUTPUT INSERTED.instruction_id VALUES(@pavadinimas,@aprasymas,@vartotojo_id,@file_name,GETDATE(),@status)";
        //Perkelta//public string Delete_Record = "UPDATE Gedimas SET status=@status where Record_Id = @Check_Id";
        public string instruction_delete = "UPDATE Instruction SET status=@status WHERE instruction_id = @Check_Id";
       //Perkelta// public string Edit_Record = "UPDATE Gedimas SET List = @name , Description = @description ,Solution = @solution WHERE Record_Id = @id";
       //Perkelta// public string editInstruction = "UPDATE Instrukcijos SET title = @name , description = @description WHERE instruction_id = @id";
        //2in1
        public string instruction_all = @"
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
    WHERE 
        I.user_id = @id;
";
        ///paima visus irasus pagal id ir apima kitas lenteles su foreign key
        public string incident_all = @"
        SELECT 
          I.Record_Id,
          I.List,
          I.Description,
          I.Solution,         
          I.Nuotrauka,
          P.file_name,
          P.upload_date
        FROM
          Gedimas AS I
        LEFT JOIN
          photos_incident AS P ON I.Record_Id = P.post_id
        LEFT JOIN
          comment_incident AS C ON I.Record_Id = C.post_id
        WHERE
          I.appUser_Id = @id AND I.status = @status;

";

        //Perkelta//
//        public string incident_by_id = @"
//        SELECT 
//          I.Record_Id,
//          I.List,
//          I.Description,
//          I.Solution,
//          I.Nuotrauka,
//          I.file_name AS incident_file_name,
//          P.file_name,
//          P.upload_date
//        FROM
//          Gedimas AS I
//        LEFT JOIN
//          photos_incident AS P ON I.Record_Id = P.post_id
//        LEFT JOIN
//          comment_incident AS C ON I.Record_Id = C.post_id
//        WHERE
//          I.Record_Id = @id AND I.status=@status;

//";
        //naujas query su kategorijom
        //Perkeltas
        //public string qr = @"Select
        //    n.List,
        //    n.Description,
        //    n.Record_Id,
        //    n.Solution
        //    FROM
        //    Gedimas AS n
        //    INNER JOIN
        //    IncidentCategory nc ON n.Record_Id = nc.incident_id
        //    INNER JOIN 
        //    Category c ON nc.category_id = c.id
        //    WHERE c.category_name = @category AND n.status=@status AND n.appUser_Id=@id";

        public string instructionByCategory = @"Select
        I.title,
        I.description,
        I.instruction_id,
        K.Patinka
        FROM 
        Instrukcijos AS I
        INNER JOIN 
        InstructionCategory ic ON I.instruction_id = ic.instruction_id
        INNER JOIN
        Category c ON ic.category_id = c.id
        INNER JOIN
        Instrukcijos_statusas AS K ON I.instruction_id = K.Instrukcijos_id
        WHERE c.id = @category AND I.user_id = @id AND I.status=@status";


        public string instruction_by_id = @"Select
        I.title,
        I.description,
        I.instruction_id,
        I.file_name AS instructionFileName,     
        S.Patinka,
        P.file_name,
        P.upload_date,
        P.directory        
        FROM
        Instrukcijos AS I
        LEFT JOIN
        Instrukcijos_statusas AS S ON I.instruction_id = S.Instrukcijos_id
        LEFT JOIN 
        photos_instructions AS P ON I.instruction_id = P.instruction_id
        WHERE I.instruction_id = @instruction_id AND I.status = @status;
";


    }


}


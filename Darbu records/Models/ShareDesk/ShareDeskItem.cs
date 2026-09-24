namespace Darbu_records.Models.ShareDesk
{
    public class ShareDeskItem
    {
        public int Id { get; set; }

        public int Share_id { get; set; }

        public int Config_id { get; set; }

        public int Created_by { get; set; }


        public int Department_access { get; set; }

        public int Department_id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;


        public string Asp_controller { get; set; } = string.Empty;

        public string Asp_action { get; set; } = string.Empty;


    }
}

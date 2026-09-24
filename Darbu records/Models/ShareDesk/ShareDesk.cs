namespace Darbu_records.Models.ShareDesk
{
    public class ShareDesk
    {
      public List<ShareDeskItem> Items { get; set; }=new List<ShareDeskItem>();







        public List<ShareDeskItem> getShared(int id)
        {



            List<ShareDeskItem> items = Items.Where(c=>c.Department_id == id).ToList();
            return items;
        }


        public List<ShareDeskItem> getSharedAnother(int id)
        {



            List<ShareDeskItem> items = Items.Where(c => c.Department_access == id).ToList();
            return items;
        }

    }
}

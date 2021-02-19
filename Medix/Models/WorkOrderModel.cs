namespace Medix.Models
{
    public class WorkOrderModel
    {
        public int WorkOrderID { get; set; }
        public int ClientID { get; set; }
        public int StateID { get; set; }
        public int StatusID { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CreatedDate { get; set; }
        public string ETA { get; set; }
        public string ClientName { get; set; }
        public string StatusName { get; set; }
    }
}
namespace ieRecruitment.Models
{
    public class OfflineSyncRequest
    {
        public string CandidateId { get; set; }
        public string LocalStorageData { get; set; } // The JSON string from local storage
    }
}
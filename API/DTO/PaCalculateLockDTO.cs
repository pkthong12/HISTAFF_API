﻿namespace API.DTO
{
    public class PaCalculateLockDTO
    {
        public long? Id { get; set; }
        public long? PeriodId { get; set; }
        public long? OrgId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

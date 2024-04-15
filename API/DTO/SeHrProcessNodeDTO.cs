﻿namespace API.DTO
{
    public class SeHrProcessNodeDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public string? StartBy { get; set; }
        public string? OrderBy { get; set; }
        public string? IconKey { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

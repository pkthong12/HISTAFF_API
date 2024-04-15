﻿namespace API.DTO
{
    public class SysPositionDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public long? AreaId { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? JobDesc { get; set; }
        public bool? IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

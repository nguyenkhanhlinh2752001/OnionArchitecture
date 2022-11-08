﻿using Domain.Contracts;

namespace Domain.Entities
{
    public class Review : AuditableBaseEntity<int>
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
    }
}
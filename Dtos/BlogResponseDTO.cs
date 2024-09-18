using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Models;

namespace BlogApi2.Dtos
{
    public class BlogResponseDTO
    {
        
        public Guid Id  { get; set; }
        public string CreatorName { get; set; }
        public string BLogTitle  { get; set; }
        public string BlogDesctiption  { get; set; }
        public bool Personal  { get; set; }
    }
}
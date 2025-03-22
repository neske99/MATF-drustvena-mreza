using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Domain.Common;

namespace Post.Domain.Entities
{
    public class User:Aggregate
    {
        public string Username { get; set; }=string.Empty;
        
    }
}
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi_DAL.Model
{
    public enum StatusTask
    {
        ToDo,
        InProgress,
        Done
    }
    // POCO 
    public class EntityTask
    {
        [SwaggerSchema(ReadOnly = true)]
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public StatusTask Status { get; set; }
        public int Priority { get; set; }
        public int ProjectID { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public EntityProject? Project { get; set; }

    }
}

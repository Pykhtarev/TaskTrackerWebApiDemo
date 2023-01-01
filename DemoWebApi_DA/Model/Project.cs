using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoWebApi_DAL.Model
{
      
    public enum StatusProject
    {
        NotStarted,
        Active,
        Completed
    }
    //POCO
    public class EntityProject
    {
        [SwaggerSchema(ReadOnly = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public StatusProject Status { get; set; }
        public int Priority { get; set; }

        public DateTime StartDate { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public DateTime? CompletionDate { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public List<EntityTask>? Tasks { get; set; }

    }
    }


using DemoWebApi_DAL.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoWebApi.Common

{
    //DTO objects give flexability to work with controllers
    public class TaskRequestArg
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public StatusTask? Status { get; set; }
        public int? Priority { get; set; }

    }

    public class ProjectRequestArgs
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public StatusProject? Status { get; set; }
        public int? Priority { get; set; }

        public DateTime? StartDate { get; set; }
        
       
    }
}

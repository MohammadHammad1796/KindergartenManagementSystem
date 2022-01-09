using System.Collections.Generic;

namespace KindergartenManagementSystem.Core.Helpers
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public ICollection<Error> Errors { get; set; }

        public Result()
        {
            Errors = new List<Error>();
        }
    }
}
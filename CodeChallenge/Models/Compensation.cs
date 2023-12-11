using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        /* I know CompensationId wasn't specifically asked for in the requirements, but it will make 
         * audit trail and logging things easier to track if we include it now.  The reqs 
         * said to store "the employee" but I'm going to assume that this can be the EmployeeId.
         * 
         * It looks like the requirements just wanted me to store one compensation for an employee but
         * I'm going to err on the side of providing the functionality to do a history audit.
         */
        public string CompensationId { get; set; }
        public string EmployeeId { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}

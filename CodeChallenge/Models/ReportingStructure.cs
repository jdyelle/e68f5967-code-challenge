namespace CodeChallenge.Models
{
    // TODO: Challenge-2
    /* I'm not sure how I feel about this being a model without a DBSet directly, but I also 
     * don't want to incorporate a brand new DTOs paradigm at this point in case it's less 
     * straightforward to those working with legacy code.  If we get a few more DTOs it will 
     * make sense to refactor.
     */
    public class ReportingStructure
    {
        public Employee employee { get; set; } 
        public int numberOfReports { get; set; }
    }
}

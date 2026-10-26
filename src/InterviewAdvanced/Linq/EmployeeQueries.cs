namespace InterviewAdvanced.Linq;

// Pure functions over IEnumerable<Employee>, so they are easy to unit test.
public static class EmployeeQueries
{
    // Filter + sort. Nothing runs until the result is enumerated (deferred execution).
    public static IEnumerable<Employee> FilterAndSort(
        IEnumerable<Employee> employees,
        string? department = null,
        decimal? minSalary = null,
        bool sortBySalaryDesc = true)
    {
        var query = employees;

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

        if (minSalary is not null)
            query = query.Where(e => e.Salary >= minSalary);

        return sortBySalaryDesc
            ? query.OrderByDescending(e => e.Salary).ThenBy(e => e.Name)
            : query.OrderBy(e => e.Name);
    }

    // Group by department with aggregates.
    public static List<DepartmentSummary> SummarizeByDepartment(IEnumerable<Employee> employees) =>
        employees
            .GroupBy(e => e.Department)
            .Select(g => new DepartmentSummary(
                Department: g.Key,
                Headcount: g.Count(),
                AverageSalary: Math.Round(g.Average(e => e.Salary), 2),
                MaxSalary: g.Max(e => e.Salary),
                TopEarner: g.MaxBy(e => e.Salary)!.Name))
            .OrderByDescending(s => s.AverageSalary)
            .ToList();

    // Same grouping written in query syntax, in case the interviewer asks.
    public static List<(string Department, decimal AverageSalary)> AverageSalaryQuerySyntax(IEnumerable<Employee> employees) =>
        (from e in employees
         group e by e.Department into g
         orderby g.Key
         select (g.Key, g.Average(x => x.Salary)))
        .ToList();
}

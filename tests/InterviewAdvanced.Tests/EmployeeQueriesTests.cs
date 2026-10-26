using InterviewAdvanced.Linq;

namespace InterviewAdvanced.Tests;

public class EmployeeQueriesTests
{
    private static readonly List<Employee> Employees =
    [
        new(1, "Ana",   "Engineering", 100m, new DateOnly(2020, 1, 1)),
        new(2, "Ben",   "Engineering", 200m, new DateOnly(2021, 1, 1)),
        new(3, "Cara",  "Sales",        90m, new DateOnly(2022, 1, 1)),
    ];

    [Fact]
    public void FilterAndSort_ByDepartmentAndMinSalary_ReturnsMatchesHighestFirst()
    {
        var result = EmployeeQueries.FilterAndSort(Employees, department: "engineering", minSalary: 100m).ToList();

        Assert.Equal(["Ben", "Ana"], result.Select(e => e.Name));
    }

    [Fact]
    public void SummarizeByDepartment_CalculatesAggregates()
    {
        var summary = EmployeeQueries.SummarizeByDepartment(Employees);

        var engineering = Assert.Single(summary, s => s.Department == "Engineering");
        Assert.Equal(2, engineering.Headcount);
        Assert.Equal(150m, engineering.AverageSalary);
        Assert.Equal("Ben", engineering.TopEarner);

        // Ordered by average salary, highest first.
        Assert.Equal("Engineering", summary[0].Department);
    }
}

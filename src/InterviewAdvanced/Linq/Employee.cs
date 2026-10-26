namespace InterviewAdvanced.Linq;

public record Employee(int Id, string Name, string Department, decimal Salary, DateOnly HireDate);

public record DepartmentSummary(
    string Department,
    int Headcount,
    decimal AverageSalary,
    decimal MaxSalary,
    string TopEarner);

public static class EmployeeData
{
    public static IReadOnlyList<Employee> Seed { get; } =
    [
        new(1, "Ana",     "Engineering", 105_000m, new DateOnly(2019, 3, 1)),
        new(2, "Brian",   "Engineering",  92_000m, new DateOnly(2021, 7, 15)),
        new(3, "Carla",   "Engineering", 130_000m, new DateOnly(2016, 1, 10)),
        new(4, "David",   "Sales",        65_000m, new DateOnly(2022, 5, 2)),
        new(5, "Elena",   "Sales",        78_000m, new DateOnly(2018, 11, 20)),
        new(6, "Frank",   "HR",           60_000m, new DateOnly(2020, 2, 3)),
        new(7, "Grace",   "HR",           72_000m, new DateOnly(2017, 9, 30)),
        new(8, "Hector",  "Marketing",    70_000m, new DateOnly(2023, 4, 12)),
    ];
}

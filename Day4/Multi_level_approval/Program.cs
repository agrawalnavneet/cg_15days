using System;
using System.Collections.Generic;


public class ExpenseRequest
{
    public string EmployeeName { get; set; }
    public double Amount { get; set; }

    public ExpenseRequest(string employeeName, double amount)
    {
        EmployeeName = employeeName;
        Amount = amount;
    }
}

public interface IApprover
{
    void SetNextApprover(IApprover nextApprover);
    void Approve(ExpenseRequest request);
}

// Question 3: Abstract Approver class
public abstract class Approver : IApprover
{
    protected IApprover? nextApprover;

    public void SetNextApprover(IApprover nextApprover)
    {
        this.nextApprover = nextApprover;
    }

    public abstract void Approve(ExpenseRequest request);
}


// Team Lead can approve up to ₹10,000
public class TeamLead : Approver
{
    public override void Approve(ExpenseRequest request)
    {
        if (request.Amount <= 10000)
        {
            Console.WriteLine(
                $"Team Lead approved ₹{request.Amount} for {request.EmployeeName}."
            );
        }
        else if (nextApprover != null)
        {
            nextApprover.Approve(request);
        }
    }
}

// Manager can approve up to ₹50,000
public class Manager : Approver
{
    public override void Approve(ExpenseRequest request)
    {
        if (request.Amount <= 50000)
        {
            Console.WriteLine(
                $"Manager approved ₹{request.Amount} for {request.EmployeeName}."
            );
        }
        else if (nextApprover != null)
        {
            nextApprover.Approve(request);
        }
    }
}


// Director can approve any amount
public class Director : Approver
{
    public override void Approve(ExpenseRequest request)
    {
        Console.WriteLine(
            $"Director approved ₹{request.Amount} for {request.EmployeeName}."
        );
    }
}

// Question 7: Main program
public class Program
{
    public static void Main()
    {
        // Create approvers
        IApprover teamLead = new TeamLead();
        IApprover manager = new Manager();
        IApprover director = new Director();

        // Create the approval chain
        teamLead.SetNextApprover(manager);
        manager.SetNextApprover(director);

        // Create list of expense requests
        List<ExpenseRequest> requests = new List<ExpenseRequest>
        {
            new ExpenseRequest("Navneet", 5000),
            new ExpenseRequest("Rahul", 25000),
            new ExpenseRequest("Amit", 75000),
            new ExpenseRequest("Priya", 120000)
        };

        // Process all expense requests
        foreach (ExpenseRequest request in requests)
        {
            teamLead.Approve(request);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

// Interface
public interface IPrintable
{
    void Print();
}

// Abstract Person class
public abstract class Person : IPrintable
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    protected Person(string id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public abstract void Print();
}

// Doctor class
public class Doctor : Person
{
    public string Specialization { get; set; }

    public Doctor(string id, string name, int age, string specialization)
        : base(id, name, age)
    {
        Specialization = specialization;
    }

    public override void Print()
    {
        Console.WriteLine($"Doctor: {Name}, Specialization: {Specialization}");
    }
}

// Patient class
public class Patient : Person
{
    public string Disease { get; set; }

    public Patient(string id, string name, int age, string disease)
        : base(id, name, age)
    {
        Disease = disease;
    }

    public override void Print()
    {
        Console.WriteLine($"Patient: {Name}, Disease: {Disease}");
    }
}

// Appointment class
public class Appointment : IPrintable
{
    public string AppointmentId { get; set; }
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public DateTime Date { get; set; }

    public void Print()
    {
        Console.WriteLine(
            $"Appointment: {AppointmentId}, Patient: {PatientId}, Doctor: {DoctorId}, Date: {Date:dd-MM-yyyy}"
        );
    }
}

// Billing class
public class Billing : IPrintable
{
    public string BillId { get; set; }
    public string PatientId { get; set; }
    public decimal Amount { get; set; }

    public void Print()
    {
        Console.WriteLine(
            $"Bill: {BillId}, Patient: {PatientId}, Amount: ₹{Amount}"
        );
    }
}

// Generic Repository
public class Repository<T> where T : class
{
    private readonly Dictionary<string, T> items = new Dictionary<string, T>();

    public void Add(string id, T item)
    {
        items[id] = item;
    }

    public T Get(string id)
    {
        items.TryGetValue(id, out T item);
        return item;
    }

    public IEnumerable<T> GetAll()
    {
        return items.Values;
    }

    public bool Remove(string id)
    {
        return items.Remove(id);
    }

    public int Count()
    {
        return items.Count;
    }
}

// Hospital Management System
public class HospitalManagementSystem
{
    public Repository<Doctor> Doctors { get; } = new Repository<Doctor>();
    public Repository<Patient> Patients { get; } = new Repository<Patient>();
    public Repository<Appointment> Appointments { get; } = new Repository<Appointment>();
    public Repository<Billing> Bills { get; } = new Repository<Billing>();

    public object GenerateDashboard()
    {
        return new
        {
            TotalPatients = Patients.Count(),
            TotalDoctors = Doctors.Count(),
            Revenue = Bills.GetAll().Sum(b => b.Amount)
        };
    }
}

// Main Program
public class Program
{
    public static void Main()
    {
        // Doctors using object initializers
        var doctor1 = new Doctor(
            "D001",
            "Dr. Sharma",
            45,
            "Cardiologist"
        );

        var doctor2 = new Doctor(
            "D002",
            "Dr. Verma",
            40,
            "Neurologist"
        );

        // Patients using object initializers
        var patient1 = new Patient(
            "P1001",
            "Rahul",
            30,
            "Fever"
        );

        var patient2 = new Patient(
            "P1002",
            "Amit",
            35,
            "Diabetes"
        );

        var patient3 = new Patient(
            "P1003",
            "Priya",
            28,
            "Migraine"
        );

        // Appointments
        var appointment1 = new Appointment
        {
            AppointmentId = "A001",
            PatientId = "P1001",
            DoctorId = "D001",
            Date = new DateTime(2026, 8, 15)
        };

        var appointment2 = new Appointment
        {
            AppointmentId = "A002",
            PatientId = "P1002",
            DoctorId = "D002",
            Date = new DateTime(2026, 8, 16)
        };

        // Bills
        var bill1 = new Billing
        {
            BillId = "B001",
            PatientId = "P1001",
            Amount = 50000
        };

        var bill2 = new Billing
        {
            BillId = "B002",
            PatientId = "P1002",
            Amount = 75000
        };

        var bill3 = new Billing
        {
            BillId = "B003",
            PatientId = "P1003",
            Amount = 25000
        };

        // Create hospital system
        var hospital = new HospitalManagementSystem();

        // Add doctors
        hospital.Doctors.Add(doctor1.Id, doctor1);
        hospital.Doctors.Add(doctor2.Id, doctor2);

        // Add patients
        hospital.Patients.Add(patient1.Id, patient1);
        hospital.Patients.Add(patient2.Id, patient2);
        hospital.Patients.Add(patient3.Id, patient3);

        // Add appointments
        hospital.Appointments.Add(
            appointment1.AppointmentId,
            appointment1
        );

        hospital.Appointments.Add(
            appointment2.AppointmentId,
            appointment2
        );

        // Add bills
        hospital.Bills.Add(bill1.BillId, bill1);
        hospital.Bills.Add(bill2.BillId, bill2);
        hospital.Bills.Add(bill3.BillId, bill3);

        // Indexer example
        Console.WriteLine("Patient using indexer:");
        var patient = hospital.Patients.Get("P1001");

        if (patient != null)
        {
            patient.Print();
        }

        Console.WriteLine();

        // Print doctors
        Console.WriteLine("Doctors:");
        foreach (var doctor in hospital.Doctors.GetAll())
        {
            doctor.Print();
        }

        Console.WriteLine();

        // Print patients
        Console.WriteLine("Patients:");
        foreach (var p in hospital.Patients.GetAll())
        {
            p.Print();
        }

        Console.WriteLine();

        // Print appointments
        Console.WriteLine("Appointments:");
        foreach (var appointment in hospital.Appointments.GetAll())
        {
            appointment.Print();
        }

        Console.WriteLine();

        // Print bills
        Console.WriteLine("Bills:");
        foreach (var bill in hospital.Bills.GetAll())
        {
            bill.Print();
        }

        Console.WriteLine();

        // Anonymous-type dashboard
        var dashboard = hospital.GenerateDashboard();

        Console.WriteLine("Hospital Dashboard");
        Console.WriteLine($"Total Patients: {dashboard.GetType().GetProperty("TotalPatients").GetValue(dashboard)}");
        Console.WriteLine($"Total Doctors: {dashboard.GetType().GetProperty("TotalDoctors").GetValue(dashboard)}");
        Console.WriteLine($"Revenue: ₹{dashboard.GetType().GetProperty("Revenue").GetValue(dashboard)}");
    }
}
using System;
using System.Collections.Generic;

class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int Marks { get; set; }

    public Student(string name, int age, int marks)
    {
        Name = name;
        Age = age;
        Marks = marks;
    }

    public override string ToString()
    {
        return $"{Name} {Age} {Marks}";
    }
}

class StudentComparer : IComparer<Student>
{
    public int Compare(Student x, Student y)
    {
        // Highest Marks first
        int marksComparison = y.Marks.CompareTo(x.Marks);

        if (marksComparison != 0)
            return marksComparison;

        // Youngest Age first
        return x.Age.CompareTo(y.Age);
    }
}

class Program
{
    static void Main()
    {
        List<Student> students = new List<Student>
        {
            new Student("Amit", 22, 85),
            new Student("Rahul", 20, 90),
            new Student("Neha", 19, 90),
            new Student("Priya", 21, 85)
        };

        students.Sort(new StudentComparer());

        foreach (Student student in students)
        {
            Console.WriteLine(student);
        }
    }
}
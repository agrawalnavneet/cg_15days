using System;
using System.Collections.Generic;

public class QueryBuilder
{
    private readonly List<object> _clauses = new List<object>();

    public QueryBuilder AddWhereClause(string clause)
    {
        _clauses.Add(clause);
        return this;
    }

    public QueryBuilder AddWhereClause(params Action<QueryBuilder>[] nestedClauses)
    {
        foreach (var action in nestedClauses)
        {
            var nestedBuilder = new QueryBuilder();
            action(nestedBuilder);
            _clauses.Add(nestedBuilder);
        }
        return this;
    }

    public string BuildSql()
    {
        int indentLevel = 0;

        string BuildClauseList(QueryBuilder qb, ref int level)
        {
            string op = level % 2 == 0 ? "AND" : "OR";
            string indent = new string(' ', level * 2);
            var parts = new List<string>();

            foreach (var clause in qb._clauses)
            {
                if (clause is string simple)
                {
                    parts.Add(simple);
                }
                else if (clause is QueryBuilder nested)
                {
                    level++;
                    string inner = BuildClauseList(nested, ref level);
                    level--;

                    string innerIndent = new string(' ', (level + 1) * 2);
                    parts.Add($"(\n{innerIndent}{inner}\n{indent})");
                }
            }

            return string.Join($"\n{indent}{op} ", parts);
        }

        return "WHERE " + BuildClauseList(this, ref indentLevel);
    }
}

public class Program
{
    public static void Main()
    {
        var qb = new QueryBuilder();
        qb.AddWhereClause("Status = 'Active'");
        qb.AddWhereClause(builder =>
        {
            builder.AddWhereClause("Age > 18");
            builder.AddWhereClause("Age < 65");
        });

        Console.WriteLine(qb.BuildSql());
    }
}
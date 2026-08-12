using System;

public class RobotSafetyException : Exception
{
    public RobotSafetyException(string message) : base(message)
    {
    }
}
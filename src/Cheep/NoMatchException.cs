using System;

public class NoMatchRegexException : Exception
{
    public NoMatchRegexException()
    {
    }

    public NoMatchRegexException(string message)
        : base(message)
    {
    }

    public NoMatchRegexException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
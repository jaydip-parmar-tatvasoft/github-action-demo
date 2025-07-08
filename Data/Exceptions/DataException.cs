namespace Data.Exceptions;
/// <summary>
/// An exception that occurred in the Data Access Layer
/// </summary>
public abstract class DataException : Exception
{
    /// <summary>
    /// Constructor requiring an error message
    /// </summary>
    /// <param name="message">a message describing the exception</param>
    protected DataException(string message) : base(message) { }
}

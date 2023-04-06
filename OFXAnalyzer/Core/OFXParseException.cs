using System;

namespace OFXAnalyzer.Core;

internal class OfxParseException : Exception
{
    public OfxParseException(string message) : base(message)
    {
    }
}
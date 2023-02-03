namespace Foxite.Text.Parsers; 

public abstract class Parser {
	public abstract IText Parse(string text);
}
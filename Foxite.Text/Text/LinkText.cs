namespace Foxite.Text;

public record LinkText(
	Uri Uri,
	IText Text
) : IText;

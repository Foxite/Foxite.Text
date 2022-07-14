namespace Foxite.Text;

public record StyledText(
	Style Style,
	IText Text
) : IText;

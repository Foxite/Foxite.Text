namespace Foxite.Text;

public record StyledText(
	Style Style,
	IText Text
) : IText {
	public virtual bool Equals(IText? other) {
		return other is StyledText otherStyled && Style == otherStyled.Style && Text.Equals(otherStyled.Text);
	}
}

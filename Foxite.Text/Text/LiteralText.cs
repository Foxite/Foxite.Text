namespace Foxite.Text;

public record LiteralText(
	string Contents
) : IText {
	public virtual bool Equals(IText? other) {
		return other is LiteralText otherLiteral && Contents == otherLiteral.Contents;
	}
}

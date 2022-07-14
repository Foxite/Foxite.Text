namespace Foxite.Text;

public record CompositeText(
	IReadOnlyList<IText> Children
) : IText {
	public CompositeText(params IText[] children) : this((IReadOnlyList<IText>) children) { }
}

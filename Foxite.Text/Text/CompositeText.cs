namespace Foxite.Text;

public record CompositeText(
	IReadOnlyList<IText> Children
) : IText {
	public CompositeText(params IText[] children) : this((IReadOnlyList<IText>) children) { }
	
	public virtual bool Equals(IText? other) {
		return other is CompositeText otherComposite && Children.SequenceEqual(otherComposite.Children);
	}

	public override string ToString() {
		return $"CompositeText {{ {string.Join(", ", Children)} }}";
	}
}

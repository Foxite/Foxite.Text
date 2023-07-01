namespace Foxite.Text;

public class CompositeText : IText {
	public IList<IText> Children { get; }
	
	public CompositeText(params IText[] children) : this((IList<IText>) children) { }
	public CompositeText(IList<IText> children) {
		this.Children = children;
	}

	public virtual bool Equals(IText? other) {
		return other is CompositeText otherComposite && Children.SequenceEqual(otherComposite.Children);
	}

	public override string ToString() {
		return $"CompositeText {{ {string.Join(", ", Children)} }}";
	}
}

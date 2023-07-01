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
	
	public void Visit(IText.Visitor visitor) {
		foreach (IText child in Children) {
			visitor(this, child);
			child.Visit(visitor);
		}
	}
	
	public void VisitReplace(IText.ReplaceVisitor visitor) {
		for (int i = Children.Count - 1; i >= 0; i--) {
			IText? replacement = visitor(this, Children[i]);
			if (replacement == null) {
				Children.RemoveAt(i);
			} else {
				Children[i] = replacement;
				Children[i].VisitReplace(visitor);
			}
		}
	}
}

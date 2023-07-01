namespace Foxite.Text;

public record ListText(
	bool IsNumbered,
	IReadOnlyList<IText> Items
) : IText {
	public ListText(bool isNumbered, params IText[] items) : this(isNumbered, (IReadOnlyList<IText>) items) { }
	
	public virtual bool Equals(IText? other) {
		return other is ListText otherBl && IsNumbered == otherBl.IsNumbered && Items.SequenceEqual(otherBl.Items);
	}
	
	public override string ToString() {
		return $"ListText(IsNumbered = {IsNumbered} ) {{ {string.Join(", ", Items)} }}";
	}
}

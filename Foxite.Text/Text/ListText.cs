namespace Foxite.Text;

public class ListText : IText {
	public bool IsNumbered { get; set; }
	public IList<IText> Items { get; }
	
	public ListText(bool isNumbered, params IText[] items) : this(isNumbered, (IList<IText>) items) { }
	public ListText(bool isNumbered, IList<IText> items) {
		this.IsNumbered = isNumbered;
		this.Items = items;
	}

	public virtual bool Equals(IText? other) {
		return other is ListText otherBl && IsNumbered == otherBl.IsNumbered && Items.SequenceEqual(otherBl.Items);
	}
	
	public override string ToString() {
		return $"ListText(IsNumbered = {IsNumbered} ) {{ {string.Join(", ", Items)} }}";
	}
}

namespace Foxite.Text;

public record LinkText(
	Uri Uri,
	IText Text
) : IText {
	public virtual bool Equals(IText? other) {
		return other is LinkText otherLink && Uri.Equals(otherLink.Uri) && Text.Equals(otherLink.Text);
	}
}

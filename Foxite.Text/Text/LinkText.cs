namespace Foxite.Text;

public class LinkText : IText {
	public Uri Uri { get; set; }
	public IText Text { get; set; }
	
	public LinkText(Uri uri, IText text) {
		this.Uri = uri;
		this.Text = text;
	}
	
	public virtual bool Equals(IText? other) {
		return other is LinkText otherLink && Uri.Equals(otherLink.Uri) && Text.Equals(otherLink.Text);
	}

	public void Visit(IText.Visitor visitor) {
		visitor(this, Text);
		Text.Visit(visitor);
	}

	public void VisitReplace(IText.ReplaceVisitor visitor) {
		Text = visitor(this, Text) ?? throw new InvalidOperationException("Cannot remove the child Text of a LinkText");
	}
}

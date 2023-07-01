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
}

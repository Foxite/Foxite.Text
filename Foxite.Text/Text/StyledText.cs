namespace Foxite.Text;

public class StyledText : IText {
	public Style Style { get; set; }
	public IText Text { get; set; }
	
	public StyledText(Style style, IText text) {
		this.Style = style;
		this.Text = text;
	}
	
	public virtual bool Equals(IText? other) {
		return other is StyledText otherStyled && Style == otherStyled.Style && Text.Equals(otherStyled.Text);
	}
}

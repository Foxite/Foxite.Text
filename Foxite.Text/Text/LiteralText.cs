namespace Foxite.Text;

public class LiteralText : IText {
	public string Contents { get; set; }
	
	public LiteralText(string contents) {
		this.Contents = contents;
	}
	
	public virtual bool Equals(IText? other) {
		return other is LiteralText otherLiteral && Contents == otherLiteral.Contents;
	}
}

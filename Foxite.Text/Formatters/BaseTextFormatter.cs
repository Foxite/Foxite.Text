using System.Text;

namespace Foxite.Text; 

public abstract class BaseTextFormatter : ITextFormatter {
	
	public string Format(IText text) {
		var sb = new StringBuilder();
		FormatInternal(text, sb);
		return sb.ToString();
	}

	private void FormatInternal(IText text, StringBuilder builder) {
		if (text is CompositeText compositeText) {
			AppendCompositeText(compositeText, builder);
		} else if (text is StyledText styledText) {
			AppendStyledText(styledText, builder);
		} else if (text is LinkText linkText) {
			AppendLinkText(linkText, builder);
		} else if (text is LiteralText literalText) {
			AppendLiteralText(literalText, builder);
		} else {
			AppendUnknownText(text, builder);
		}
	}

	protected abstract void AppendStyledText(StyledText styledText, StringBuilder builder);
	protected abstract void AppendLinkText(LinkText linkText, StringBuilder builder);

	protected virtual void AppendCompositeText(CompositeText compositeText, StringBuilder builder) {
		foreach (IText childText in compositeText.Children) {
			//FormatInternal(childText, builder);
			builder.Append(Format(childText));
		}
	}

	protected virtual void AppendLiteralText(LiteralText literalText, StringBuilder builder) {
		builder.Append(literalText.Contents);
	}

	protected virtual void AppendUnknownText(IText text, StringBuilder builder) {
		throw new UnknownTextException(text);
	}
}

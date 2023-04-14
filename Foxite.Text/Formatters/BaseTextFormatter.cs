using System.Text;

namespace Foxite.Text; 

public abstract class BaseTextFormatter : ITextFormatter {
	public string Format(IText text) {
		var sb = new StringBuilder();
		FormatInternal(text, sb);
		return sb.ToString();
	}

	private void FormatInternal(IText text, StringBuilder builder) {
		if (text is CompositeText composite) {
			AppendCompositeText(composite, builder);
		} else if (text is StyledText styled) {
			AppendStyledText(styled, builder);
		} else if (text is LinkText link) {
			AppendLinkText(link, builder);
		} else if (text is LiteralText literal) {
			AppendLiteralText(literal, builder);
		} else {
			AppendUnknownText(text, builder);
		}
	}

	protected abstract void AppendLinkText(LinkText linkText, StringBuilder builder);

	protected virtual void AppendCompositeText(CompositeText compositeText, StringBuilder builder) {
		foreach (IText childText in compositeText.Children) {
			// The following line was originally commented:
			FormatInternal(childText, builder);
			// And this line was there instead:
			//builder.Append(Format(childText));
			// Tell me if you can figure out why this was
		}
	}

	protected virtual void AppendStyledText(StyledText styledText, StringBuilder builder) {
#pragma warning disable CS8524
		Action<IText, StringBuilder> func = styledText.Style switch {
#pragma warning restore CS8524
			Style.Bold => AppendBoldText,
			Style.Italic => AppendItalicText,
			Style.Strikethrough => AppendStrikethroughText,
			Style.Underline => AppendUnderlineText,
		};

		func(styledText.Text, builder);
	}
	
	protected abstract void AppendBoldText(IText inner, StringBuilder builder);
	protected abstract void AppendItalicText(IText inner, StringBuilder builder);
	protected abstract void AppendStrikethroughText(IText inner, StringBuilder builder);
	protected abstract void AppendUnderlineText(IText inner, StringBuilder builder);

	protected virtual void AppendLiteralText(LiteralText literalText, StringBuilder builder) {
		builder.Append(literalText.Contents);
	}

	protected virtual void AppendUnknownText(IText text, StringBuilder builder) {
		throw new UnknownTextException(text);
	}
}
using System.Text;

namespace Foxite.Text; 

[Obsolete("Use " + nameof(ModularTextFormatter))]
public abstract class BaseTextFormatter : ITextFormatter {
	public string Format(IText text) {
		var sb = new StringBuilder();
		AppendText(text, sb, new Stack<string>());
		return sb.ToString();
	}

	protected void AppendText(IText text, StringBuilder builder, Stack<string> formatStack) {
		if (text is CompositeText composite) {
			AppendCompositeText(composite, builder, formatStack);
		} else if (text is StyledText styled) {
			AppendStyledText(styled, builder, formatStack);
		} else if (text is LinkText link) {
			AppendLinkText(link, builder, formatStack);
		} else if (text is LiteralText literal) {
			AppendLiteralText(literal, builder, formatStack);
		} else if (text is ListText list) {
			AppendListText(list, builder, formatStack);
		} else {
			AppendUnknownText(text, builder, formatStack);
		}
	}

	protected abstract void AppendLinkText(LinkText linkText, StringBuilder builder, Stack<string> formatStack);

	protected virtual void AppendCompositeText(CompositeText compositeText, StringBuilder builder, Stack<string> formatStack) {
		foreach (IText childText in compositeText.Children) {
			// The following line was originally commented:
			AppendText(childText, builder, formatStack);
			// And this line was there instead:
			//builder.Append(Format(childText));
			// Tell me if you can figure out why this was
		}
	}

	protected abstract void AppendListText(ListText listText, StringBuilder builder, Stack<string> formatStack);

	protected virtual void AppendStyledText(StyledText styledText, StringBuilder builder, Stack<string> formatStack) {
#pragma warning disable CS8524
		Action<IText, StringBuilder, Stack<string>> func = styledText.Style switch {
#pragma warning restore CS8524
			Style.Bold => AppendBoldText,
			Style.Italic => AppendItalicText,
			Style.Strikethrough => AppendStrikethroughText,
			Style.Underline => AppendUnderlineText,
		};

		func(styledText.Text, builder, formatStack);
	}
	
	protected abstract void AppendBoldText(IText inner, StringBuilder builder, Stack<string> formatStack);
	protected abstract void AppendItalicText(IText inner, StringBuilder builder, Stack<string> formatStack);
	protected abstract void AppendStrikethroughText(IText inner, StringBuilder builder, Stack<string> formatStack);
	protected abstract void AppendUnderlineText(IText inner, StringBuilder builder, Stack<string> formatStack);

	protected virtual void AppendLiteralText(LiteralText literalText, StringBuilder builder, Stack<string> formatStack) {
		builder.Append(literalText.Contents);
	}

	protected virtual void AppendUnknownText(IText text, StringBuilder builder, Stack<string> formatStack) {
		throw new UnknownTextException(text);
	}
}
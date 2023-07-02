using System.Text;

namespace Foxite.Text;

[Obsolete("Use " + nameof(ModularTextFormatter) + "." + nameof(ModularTextFormatter.Markdown))]
public class MarkdownBaseTextFormatter : BaseTextFormatter {
	protected override void AppendBoldText         (IText text, StringBuilder sb, Stack<string> formatStack) => sb.Append($"**{Format(text)}**");
	protected override void AppendItalicText       (IText text, StringBuilder sb, Stack<string> formatStack) => sb.Append($"*{Format(text)}*");
	protected override void AppendStrikethroughText(IText text, StringBuilder sb, Stack<string> formatStack) => sb.Append($"~~{Format(text)}~~");
	protected override void AppendUnderlineText    (IText text, StringBuilder sb, Stack<string> formatStack) => sb.Append($"__{Format(text)}__");

	protected override void AppendLinkText(LinkText linkText, StringBuilder builder, Stack<string> formatStack) {
		builder.Append('[');
		
		formatStack.Push(MarkdownUtils.StackLinkTextFormat);
		AppendText(linkText.Text, builder, formatStack);
		formatStack.Pop();
		
		builder.Append("](");
		
		formatStack.Push(MarkdownUtils.StackLinkUrlFormat);
		builder.Append(MarkdownUtils.Sanitize(linkText.Uri.ToString(), formatStack));
		formatStack.Pop();

		builder.Append(')');
	}

	protected override void AppendLiteralText(LiteralText literalText, StringBuilder builder, Stack<string> formatStack) {
		builder.Append(MarkdownUtils.Sanitize(literalText.Contents, formatStack));
	}
	
	protected override void AppendListText(ListText listText, StringBuilder builder, Stack<string> formatStack) {
		int i = 1;
		foreach (IText item in listText.Items) {
			if (i != 1 || (builder.Length > 0 && builder[^1] != '\n')) {
				builder.Append('\n');
			}
			
			if (listText.IsNumbered) {
				builder.Append($"{i}.");
			} else {
				builder.Append('-');
			}
			
			builder.Append($" {Format(item)}");
			i++;
		}
	}
}

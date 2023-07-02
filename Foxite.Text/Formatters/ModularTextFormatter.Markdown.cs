using System.Text;

namespace Foxite.Text;

public partial class ModularTextFormatter {
	public static ModularTextFormatter Markdown() {
		var ret = new ModularTextFormatter();

		ret.AddTypeFormatter(new MarkdownLiteralTextFormatter());
		ret.AddTypeFormatter(new MarkdownStyledTextFormatter());
		ret.AddTypeFormatter(new MarkdownLinkTextFormatter());
		ret.AddTypeFormatter(new MarkdownListTextFormatter());

		return ret;
	}

	private class MarkdownLiteralTextFormatter : TypeFormatter<LiteralText> {
		protected override void AppendFormattedText(LiteralText text, StringBuilder builder, Stack<string> formatStack) {
			builder.Append(MarkdownUtils.Sanitize(text.Contents, formatStack));
		}
	}

	private class MarkdownStyledTextFormatter : TypeFormatter<StyledText> {
		protected override void AppendFormattedText(StyledText text, StringBuilder builder, Stack<string> formatStack) {
#pragma warning disable CS8524
			string delimiter = text.Style switch {
#pragma warning restore CS8524
				Style.Bold          => "**",
				Style.Italic        => "*",
				Style.Strikethrough => "~~",
				Style.Underline     => "__",
			};

			builder.Append(delimiter);
			AppendRecursive(text.Text, builder, formatStack);
			builder.Append(delimiter);
		}
	}

	private class MarkdownLinkTextFormatter : TypeFormatter<LinkText> {
		protected override void AppendFormattedText(LinkText link, StringBuilder builder, Stack<string> formatStack) {
			builder.Append('[');
		
			formatStack.Push(MarkdownUtils.StackLinkTextFormat);
			AppendRecursive(link.Text, builder, formatStack);
			formatStack.Pop();
		
			builder.Append("](");
		
			formatStack.Push(MarkdownUtils.StackLinkUrlFormat);
			builder.Append(MarkdownUtils.Sanitize(link.Uri.ToString(), formatStack));
			formatStack.Pop();

			builder.Append(')');
		}
	}

	private class MarkdownListTextFormatter : TypeFormatter<ListText> {
		protected override void AppendFormattedText(ListText listText, StringBuilder builder, Stack<string> formatStack) {
			int i = 1;
			foreach (IText item in listText.Items) {
				if (i != 1 || (builder.Length > 0 && builder[^1] != '\n')) {
					builder.Append('\n');
				}
			
				if (listText.IsNumbered) {
					builder.Append($"{i}. ");
				} else {
					builder.Append("- ");
				}
			
				AppendRecursive(item, builder, formatStack);
				
				i++;
			}
		}
	}
}

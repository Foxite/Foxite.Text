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
		protected override void AppendFormattedText(LiteralText text, StringBuilder builder) {
			builder.Append(MarkdownUtils.Sanitize(text.Contents));
		}
	}

	private class MarkdownStyledTextFormatter : TypeFormatter<StyledText> {
		protected override void AppendFormattedText(StyledText text, StringBuilder builder) {
#pragma warning disable CS8524
			string delimiter = text.Style switch {
#pragma warning restore CS8524
				Style.Bold          => "**",
				Style.Italic        => "*",
				Style.Strikethrough => "~~",
				Style.Underline     => "__",
			};

			builder.Append(delimiter);
			AppendRecursive(text.Text, builder);
			builder.Append(delimiter);
		}
	}

	private class MarkdownLinkTextFormatter : TypeFormatter<LinkText> {
		protected override void AppendFormattedText(LinkText text, StringBuilder builder) {
			builder.Append('[');
			AppendRecursive(text.Text, builder);
			builder.Append("](");
			builder.Append(text.Uri);
			builder.Append(')');
		}
	}

	private class MarkdownListTextFormatter : TypeFormatter<ListText> {
		protected override void AppendFormattedText(ListText listText, StringBuilder builder) {
			int i = 1;
			foreach (IText item in listText.Items) {
				if (i != 1) {
					builder.Append('\n');
				}
			
				if (listText.IsNumbered) {
					builder.Append($"{i}. ");
				} else {
					builder.Append("- ");
				}
			
				AppendRecursive(item, builder);
				
				i++;
			}
		}
	}
}

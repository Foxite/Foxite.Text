using System.Text;
using System.Text.RegularExpressions;

namespace Foxite.Text;

public class DiscordTextFormatter : TextFormatter {
	private static Regex SanitizeRegex { get; } = new Regex(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript);
	
	public override string Format(IText text) {
		var sb = new StringBuilder();
		FormatInternal(text, sb);
		return sb.ToString();
	}

	private void FormatInternal(IText text, StringBuilder builder) {
		if (text is CompositeText compositeText) {
			foreach (IText childText in compositeText.Children) {
				FormatInternal(childText, builder);
			}
		} else if (text is StyledText styleText) {
			Func<string, string> transformer = s => s;

			void AddTransformer(Func<string, string> addTransformer) {
				Func<string, string> currentTransformer = transformer;
				transformer = s => addTransformer(currentTransformer(s));
			}
			
			if ((styleText.Style & Style.Bold) != 0) {
				AddTransformer(s => $"**{s}**");
			}
			
			if ((styleText.Style & Style.Italic) != 0) {
				AddTransformer(s => $"*{s}*");
			}
			
			if ((styleText.Style & Style.Strikethrough) != 0) {
				AddTransformer(s => $"~~{s}~~");
			}
			
			if ((styleText.Style & Style.Underline) != 0) {
				AddTransformer(s => $"__{s}__");
			}

			builder.Append(transformer(Format(styleText.Text)));
		} else if (text is LinkText linkText) {
			builder.Append($"[{Format(linkText.Text)}]({linkText.Uri})");
		} else if (text is LiteralText literalText) {
			builder.Append(Sanitize(literalText.Contents));
		} else {
			throw new UnknownTextException(text);
		}
	}
	
	private static string Sanitize(string text) => SanitizeRegex.Replace(text, m => $"\\{m.Groups[1].Value}");
}

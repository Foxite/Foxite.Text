using System.Text;
using System.Text.RegularExpressions;

namespace Foxite.Text;

public class DiscordTextFormatter : BaseTextFormatter {
	private static Regex SanitizeRegex { get; } = new Regex(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript);
	
	protected override void AppendStyledText(StyledText styleText, StringBuilder builder) {
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
	}

	protected override void AppendLinkText(LinkText linkText, StringBuilder builder) {
		builder.Append($"[{Format(linkText.Text)}]({linkText.Uri})");
	}

	protected override void AppendLiteralText(LiteralText literalText, StringBuilder builder) {
		builder.Append(Sanitize(literalText.Contents));
	}
	
	private static string Sanitize(string text) => SanitizeRegex.Replace(text, m => $"\\{m.Groups[1].Value}");
}

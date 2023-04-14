using System.Text;
using System.Text.RegularExpressions;

namespace Foxite.Text;

public class MarkdownTextFormatter : BaseTextFormatter {
	private static Regex SanitizeRegex { get; } = new Regex(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript);
	
	protected override void AppendBoldText         (IText text, StringBuilder sb) => sb.Append($"**{Format(text)}**");
	protected override void AppendItalicText       (IText text, StringBuilder sb) => sb.Append($"*{Format(text)}*");
	protected override void AppendStrikethroughText(IText text, StringBuilder sb) => sb.Append($"~~{Format(text)}~~");
	protected override void AppendUnderlineText    (IText text, StringBuilder sb) => sb.Append($"__{Format(text)}__");

	protected override void AppendLinkText(LinkText linkText, StringBuilder builder) {
		builder.Append($"[{Format(linkText.Text)}]({linkText.Uri})");
	}

	protected override void AppendLiteralText(LiteralText literalText, StringBuilder builder) {
		builder.Append(Sanitize(literalText.Contents));
	}
	
	private static string Sanitize(string text) => SanitizeRegex.Replace(text, m => $"\\{m.Groups[1].Value}");
}

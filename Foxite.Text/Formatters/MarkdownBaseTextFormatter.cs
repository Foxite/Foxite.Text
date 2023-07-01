using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Foxite.Text;

public class MarkdownBaseTextFormatter : BaseTextFormatter {
	protected override void AppendBoldText         (IText text, StringBuilder sb) => sb.Append($"**{Format(text)}**");
	protected override void AppendItalicText       (IText text, StringBuilder sb) => sb.Append($"*{Format(text)}*");
	protected override void AppendStrikethroughText(IText text, StringBuilder sb) => sb.Append($"~~{Format(text)}~~");
	protected override void AppendUnderlineText    (IText text, StringBuilder sb) => sb.Append($"__{Format(text)}__");

	protected override void AppendLinkText(LinkText linkText, StringBuilder builder) {
		builder.Append($"[{Format(linkText.Text)}]({linkText.Uri})");
	}

	protected override void AppendLiteralText(LiteralText literalText, StringBuilder builder) {
		builder.Append(MarkdownUtils.Sanitize(literalText.Contents));
	}
	
	protected override void AppendListText(ListText listText, StringBuilder builder) {
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

internal static class MarkdownUtils {
	private static Regex SanitizeRegex { get; } = new Regex(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript);
	
	public static string Sanitize(string text) => MarkdownUtils.SanitizeRegex.Replace(text, m => $"\\{m.Groups[1].Value}");
}
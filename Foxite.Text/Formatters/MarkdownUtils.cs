using System.Text.RegularExpressions;

namespace Foxite.Text;

public static class MarkdownUtils {
	internal const string StackLinkTextFormat = "LinkText";
	internal const string StackLinkUrlFormat = "LinkUrl";

	private static Regex GetSanitizeRegex(Stack<string> markdownFormats) {
		string characterClass = "";

		if (markdownFormats.Contains("LinkText")) {
			characterClass += @"\[\]";
		}

		if (markdownFormats.Contains("LinkUrl")) {
			characterClass += @"\(\)";
		} else {
			characterClass += @"`\*_~<>\|";
		}
		
		return new Regex($"([{characterClass}])", RegexOptions.ECMAScript);
	}

	public static string Sanitize(string text, Stack<string> markdownFormats) {
		return GetSanitizeRegex(markdownFormats).Replace(text, m => $"\\{m.Groups[1].Value}");
	}
}

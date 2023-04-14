using Foxite.Text.Parsers;

namespace Foxite.Text; 

public class MarkdownParserTests {
	private MarkdownParser m_Parser;

	[SetUp]
	public void Setup() {
		m_Parser = new MarkdownParser();
	}
	
	public static object[][] StyleTestCases => new [] {
		new object[] {      "**Hello**"     , new[] { Style.Bold                                                        } },
		new object[] {       "*Hello*"      , new[] {              Style.Italic                                         } },
		new object[] {     "***Hello***"    , new[] { Style.Bold , Style.Italic                                         } },
		new object[] {      "~~Hello~~"     , new[] {                             Style.Strikethrough                   } },
		new object[] {    "~~**Hello**~~"   , new[] { Style.Bold                , Style.Strikethrough                   } },
		new object[] {     "~~*Hello*~~"    , new[] {              Style.Italic , Style.Strikethrough                   } },
		new object[] {   "~~***Hello***~~"  , new[] { Style.Bold , Style.Italic , Style.Strikethrough                   } },
		new object[] {      "__Hello__"     , new[] {                                                   Style.Underline } },
		new object[] {    "__**Hello**__"   , new[] { Style.Bold                                      , Style.Underline } },
		new object[] {     "__*Hello*__"    , new[] {              Style.Italic                       , Style.Underline } },
		new object[] {   "__***Hello***__"  , new[] { Style.Bold , Style.Italic                       , Style.Underline } },
		new object[] {    "__~~Hello~~__"   , new[] {                             Style.Strikethrough , Style.Underline } },
		new object[] {  "__~~**Hello**~~__" , new[] { Style.Bold                , Style.Strikethrough , Style.Underline } },
		new object[] {   "__~~*Hello*~~__"  , new[] {              Style.Italic , Style.Strikethrough , Style.Underline } },
		new object[] { "__~~***Hello***~~__", new[] { Style.Bold , Style.Italic , Style.Strikethrough , Style.Underline } },
	};

	[Test]
	[TestCaseSource(nameof(StyleTestCases))]
	public void StyleTests(string expected, Style[] styles) {
		IText text = new LiteralText("Hello");

		foreach (Style style in styles) {
			text = new StyledText(style, text);
		}
		
		Assert.That(m_Parser.Parse(expected), Is.EqualTo(text));
	}

	[Test]
	public void LinkTests() {
		Assert.Multiple(() => {
			// These first two tests are to clarify the fact that Uri.ToString() may not return exactly the string that was passed into its constructor.
			Assert.That(new Uri("https://github.com/").ToString(), Is.EqualTo("https://github.com/"));
			Assert.That(new Uri("https://github.com" ).ToString(), Is.EqualTo("https://github.com/"));

			Assert.That(m_Parser.Parse("[Hello](https://github.com/)"), Is.EqualTo(new LinkText(new Uri("https://github.com/"), new LiteralText("Hello"))));
		});
	}

	[Test]
	public void LiteralTests() {
		Assert.That(m_Parser.Parse("Hello"), Is.EqualTo(new LiteralText("Hello")));
	}
}

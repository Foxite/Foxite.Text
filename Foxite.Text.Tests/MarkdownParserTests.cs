using Foxite.Text.Parsers;
using NUnit.Framework.Constraints;

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
		
		Assert.That(m_Parser.Parse(expected), Util.IsEqualText(text));
	}

	[Test]
	public void LinkTests() {
		Assert.Multiple(() => {
			// These first two tests are to clarify the fact that Uri.ToString() may not return exactly the string that was passed into its constructor.
			Assert.That(new Uri("https://github.com/").ToString(), Is.EqualTo("https://github.com/"));
			Assert.That(new Uri("https://github.com" ).ToString(), Is.EqualTo("https://github.com/"));

			Assert.That(m_Parser.Parse("[Hello](https://github.com/)"), Util.IsEqualText(new LinkText(new Uri("https://github.com/"), new LiteralText("Hello"))));
		});
	}

	[Test]
	public void LiteralTests() {
		Assert.That(m_Parser.Parse("Hello"), Util.IsEqualText(new LiteralText("Hello")));
	}
	
	[Test]
	public void CompositeTests() {
		Assert.That(m_Parser.Parse("Hello **Hello** Hello"), Util.IsEqualText(new CompositeText(new LiteralText("Hello "), new StyledText(Style.Bold, new LiteralText("Hello")), new LiteralText(" Hello"))));
	}

	[Test]
	public void ListTests() {
        Assert.Multiple(() => {
            Assert.That(m_Parser.Parse("- Hello\n- My name is"), Util.IsEqualText(new ListText(false, new LiteralText("Hello"), new LiteralText("My name is"))));
            Assert.That(m_Parser.Parse("* Hello\n* My name is"), Util.IsEqualText(new ListText(false, new LiteralText("Hello"), new LiteralText("My name is"))));
            Assert.That(m_Parser.Parse("1. Hello\n2. My name is"), Util.IsEqualText(new ListText(true, new LiteralText("Hello"), new LiteralText("My name is"))));
        });
    }
}

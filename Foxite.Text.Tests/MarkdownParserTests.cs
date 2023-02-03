using Foxite.Text.Parsers;

namespace Foxite.Text; 

public class MarkdownParserTests {
	private MarkdownParser m_Parser;

	[SetUp]
	public void Setup() {
		m_Parser = new MarkdownParser();
	}

	[Test]
	public void StyleTests() {
		Assert.Multiple(() => {
			Assert.That(m_Parser.Parse(     "**Hello**"     ), Is.EqualTo(new StyledText(Style.Bold                                                       , new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(      "*Hello*"      ), Is.EqualTo(new StyledText(             Style.Italic                                        , new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(    "***Hello***"    ), Is.EqualTo(new StyledText(Style.Bold | Style.Italic                                        , new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(     "~~Hello~~"     ), Is.EqualTo(new StyledText(                            Style.Strikethrough                  , new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(   "~~**Hello**~~"   ), Is.EqualTo(new StyledText(Style.Bold                | Style.Strikethrough                  , new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(    "~~*Hello*~~"    ), Is.EqualTo(new StyledText(             Style.Italic | Style.Strikethrough                  , new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(  "~~***Hello***~~"  ), Is.EqualTo(new StyledText(Style.Bold | Style.Italic | Style.Strikethrough                  , new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(     "__Hello__"     ), Is.EqualTo(new StyledText(                                                  Style.Underline, new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(   "__**Hello**__"   ), Is.EqualTo(new StyledText(Style.Bold                                      | Style.Underline, new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(    "__*Hello*__"    ), Is.EqualTo(new StyledText(             Style.Italic                       | Style.Underline, new LiteralText("Hello"))));
			Assert.That(m_Parser.Parse(  "__***Hello***__"  ), Is.EqualTo(new StyledText(Style.Bold | Style.Italic                       | Style.Underline, new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(   "__~~Hello~~__"   ), Is.EqualTo(new StyledText(                            Style.Strikethrough | Style.Underline, new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse( "__~~**Hello**~~__" ), Is.EqualTo(new StyledText(Style.Bold                | Style.Strikethrough | Style.Underline, new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse(  "__~~*Hello*~~__"  ), Is.EqualTo(new StyledText(             Style.Italic | Style.Strikethrough | Style.Underline, new LiteralText("Hello"))));
			// Assert.That(m_Parser.Parse("__~~***Hello***~~__"), Is.EqualTo(new StyledText(Style.Bold | Style.Italic | Style.Strikethrough | Style.Underline, new LiteralText("Hello"))));
		});
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
